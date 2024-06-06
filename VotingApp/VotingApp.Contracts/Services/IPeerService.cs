using VotingApp.Contracts.Dtos;

namespace VotingApp.Contracts.Services;

public interface IPeerService
{
    Task<PeerDto> CreateAsync(PeerDto dto);
    Task<bool> DoesAlreadyExistAsync(PeerDto dto);
}
