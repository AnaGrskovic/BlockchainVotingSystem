using FluentAssertions;
using Moq;
using System.Text.Json;
using VotingApp.Contracts.Dtos;
using VotingApp.Contracts.Exceptions;
using VotingApp.Contracts.Services;
using Xunit;

namespace VotingApp.Services.Tests;

public class VotingServiceTests
{
    private VotingService _sut;
    private Mock<IAuthorizationService> _autorizationServiceMock;
    private Mock<ICandidateService> _candidateServiceMock;
    private Mock<IBackupService> _backupServiceMock;
    private Mock<IMessageQueueService> _messageQueueServiceMock;

    public VotingServiceTests()
    {
        _autorizationServiceMock = new Mock<IAuthorizationService>();
        _candidateServiceMock = new Mock<ICandidateService>();
        _backupServiceMock = new Mock<IBackupService>();
        _messageQueueServiceMock = new Mock<IMessageQueueService>();

        _sut = new VotingService(
            _autorizationServiceMock.Object,
            _candidateServiceMock.Object,
            _backupServiceMock.Object,
            _messageQueueServiceMock.Object);
    }

    [Fact]
    public async Task VoteAsync_WhenTokenIsNull_ThrowTokenNotPresentException()
    {
        // Act
        var f = async () => { await _sut.VoteAsync(null, "vote"); };

        // Assert
        await f.Should().ThrowAsync<TokenNotPresentException>().WithMessage("Token not present in the request.");
    }

    [Fact]
    public async Task VoteAsync_WhenTokenIsNotValid_ThrowTokenNotValidException()
    {
        // Arrange
        var token = "token";
        _autorizationServiceMock.Setup(x => x.CheckTokenAsync(token)).ReturnsAsync(false);

        // Act
        var f = async () => { await _sut.VoteAsync(token, "vote"); };

        // Assert
        await f.Should().ThrowAsync<TokenNotValidException>().WithMessage("Token not present in the request.");
    }

    [Fact]
    public async Task VoteAsync_WhenVoteIsNull_ThrowVoteNotPresentException()
    {
        // Arrange
        var token = "token";
        _autorizationServiceMock.Setup(x => x.CheckTokenAsync(token)).ReturnsAsync(true);

        // Act
        var f = async () => { await _sut.VoteAsync(token, null); };

        // Assert
        await f.Should().ThrowAsync<VoteNotPresentException>().WithMessage("Vote not present in the request.");
    }

    [Fact]
    public async Task VoteAsync_WhenCandidateIsNotValid_ThrowCandidateNotValidException()
    {
        // Arrange
        var token = "token";
        var vote = "vote";
        _autorizationServiceMock.Setup(x => x.CheckTokenAsync(token)).ReturnsAsync(true);
        _candidateServiceMock.Setup(x => x.Check(vote)).Returns(false);

        // Act
        var f = async () => { await _sut.VoteAsync(token, vote); };

        // Assert
        await f.Should().ThrowAsync<CandidateNotValidException>().WithMessage("Candidate is not valid.");
    }

    [Fact]
    public async Task VoteAsync_WhenOk_CreateBackup()
    {
        // Arrange
        var token = "token";
        var vote = "vote";
        _autorizationServiceMock.Setup(x => x.CheckTokenAsync(token)).ReturnsAsync(true);
        _candidateServiceMock.Setup(x => x.Check(vote)).Returns(true);

        // Act
        await _sut.VoteAsync(token, vote);

        // Assert
        _backupServiceMock.Verify(x => x.CreateAsync(vote), Times.Once());
    }

    [Fact]
    public async Task VoteAsync_WhenOk_SendToQueue()
    {
        // Arrange
        var token = "token";
        var vote = "vote";
        _autorizationServiceMock.Setup(x => x.CheckTokenAsync(token)).ReturnsAsync(true);
        _candidateServiceMock.Setup(x => x.Check(vote)).Returns(true);
        var voteDto = new VoteDto(token, vote);
        var serializeOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        var voteMessage = JsonSerializer.Serialize(voteDto, serializeOptions);

        // Act
        await _sut.VoteAsync(token, vote);

        // Assert
        _messageQueueServiceMock.Verify(x => x.SendMessage(voteMessage), Times.Once());
    }
}
