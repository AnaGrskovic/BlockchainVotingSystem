using VotingApp.Contracts.Dtos;
using VotingApp.Contracts.Entities;
using VotingApp.Contracts.Services;
using VotingApp.Contracts.UoW;

namespace VotingApp.Services;

public class PeerService : IPeerService
{
    private readonly IUnitOfWork _uow;

    public PeerService(
        IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<PeerDto> CreateAsync(PeerDto dto)
    {
        Peer peer = new Peer(dto.IpAddress!, dto.Port!.Value);
        _uow.Peers.Add(peer);
        await _uow.SaveChangesAsync();
        return dto;
    }

    public async Task<bool> DoesAlreadyExistAsync(PeerDto dto)
    {
        List<Peer> peers = await _uow.Peers.GetAllAsync();
        return peers.Any(p => p.IpAddress.Equals(dto.IpAddress) && p.Port.Equals(dto.Port));
    }
}

