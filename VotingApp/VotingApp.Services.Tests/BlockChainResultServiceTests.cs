using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using VotingApp.Contracts.Dtos;
using VotingApp.Contracts.Entities;
using VotingApp.Contracts.Exceptions;
using VotingApp.Contracts.Services;
using VotingApp.Contracts.Settings;
using VotingApp.Contracts.UoW;
using Xunit;

namespace VotingApp.Services.Tests;

public class BlockChainResultServiceTests
{
    private BlockChainResultService _sut;
    private Mock<ITimeService> _timeServiceMock;
    private Mock<IBackupService> _backupServiceMock;
    private Mock<IBlockChainService> _blockChainService;

    public BlockChainResultServiceTests()
    {
        _timeServiceMock = new Mock<ITimeService>();
        _backupServiceMock = new Mock<IBackupService>();
        _blockChainService = new Mock<IBlockChainService>();

        var thresholdsSettings = new OptionsWrapper<ThresholdsSettings>(
            new ThresholdsSettings
            {
                MinimalPercentageOfCorrectBlockChains = 50,
            });
        var candidatesSettings = new OptionsWrapper<CandidatesSettings>(
            new CandidatesSettings
            {
                Candidates = ["Stephen Hawking", "Marie Curie", "Albert Einstein", "Ada Lovelace", "Nikola Tesla", "Alan Turing"]
            });

        _sut = new BlockChainResultService(
            _timeServiceMock.Object,
            _backupServiceMock.Object,
            _blockChainService.Object,
            thresholdsSettings,
            candidatesSettings);
    }

    [Fact]
    public async Task GetVotingResultAsync_WhenTooEarly_ReturnJustNumberOfVOtes()
    {
        // Arrange
        _timeServiceMock.Setup(x => x.CanResultsBeShown()).Returns(false);
        _backupServiceMock.Setup(x => x.GetCountAsync()).ReturnsAsync(100);

        // Act
        var actual = await _sut.GetVotingResultAsync();

        // Assert
        actual.NumberOfVotes.Should().Be(100);
        actual.NumberOfVotesPerCandidate.Should().BeNull();
    }

    [Fact]
    public async Task GetVotingResultAsync_WhenLargestGroupIsTooSmall_ThrowVotingResultUnacceptableException()
    {
        // Arrange
        _timeServiceMock.Setup(x => x.CanResultsBeShown()).Returns(true);
        _blockChainService.Setup(x => x.GetAllAsync()).ReturnsAsync(GetNotOkMockBlockChains("Stephen Hawking", "Alan Turing", "Nikola Tesla"));

        // Act
        var f = async () => { await _sut.GetVotingResultAsync(); };

        // Assert
        await f.Should().ThrowAsync<VotingResultUnacceptableException>().WithMessage("Due to a too big number of incorrect blockchains, the results are not acceptable.");
    }

    private List<BlockChain> GetOkMockBlockChains()
    {
        return new List<BlockChain>
        {
            new BlockChain(GetOkMockBlockChainDto()),
            new BlockChain(GetOkMockBlockChainDto()),
            new BlockChain(GetOkMockBlockChainDto())
        };
    }

    private List<BlockChain> GetNotOkMockBlockChains(string data1, string data2, string data3)
    {
        return new List<BlockChain>
        {
            new BlockChain(GetNotOkMockBlockChainDto(data1)),
            new BlockChain(GetNotOkMockBlockChainDto(data2)),
            new BlockChain(GetNotOkMockBlockChainDto(data3))
        };
    }

    private BlockChainDto GetOkMockBlockChainDto()
    {
        return new BlockChainDto
        {
            Candidates = ["Stephen Hawking", "Marie Curie", "Albert Einstein", "Ada Lovelace", "Nikola Tesla", "Alan Turing"],
            Blocks = new List<BlockDto>
            {
                new BlockDto
                {
                    Nonce = 1,
                    TimeStamp = 1,
                    Data = "Marie Curie",
                    PreviousHash = "previous hash",
                    Hash = "hash"
                },
                new BlockDto
                {
                    Nonce = 1,
                    TimeStamp = 1,
                    Data = "Nikola Tesla",
                    PreviousHash = "previous hash",
                    Hash = "hash"
                },
                new BlockDto
                {
                    Nonce = 1,
                    TimeStamp = 1,
                    Data = "Ada Lovelace",
                    PreviousHash = "previous hash",
                    Hash = "hash"
                },
            }
        };
    }

    private BlockChainDto GetNotOkMockBlockChainDto(string data)
    {
        return new BlockChainDto
        {
            Candidates = ["Stephen Hawking", "Marie Curie", "Albert Einstein", "Ada Lovelace", "Nikola Tesla", "Alan Turing"],
            Blocks = new List<BlockDto>
            {
                new BlockDto
                {
                    Nonce = 1,
                    TimeStamp = 1,
                    Data = data,
                    PreviousHash = "previous hash",
                    Hash = "hash"
                },
                new BlockDto
                {
                    Nonce = 1,
                    TimeStamp = 1,
                    Data = "Nikola Tesla",
                    PreviousHash = "previous hash",
                    Hash = "hash"
                },
                new BlockDto
                {
                    Nonce = 1,
                    TimeStamp = 1,
                    Data = "Ada Lovelace",
                    PreviousHash = "previous hash",
                    Hash = "hash"
                },
            }
        };
    }
}
