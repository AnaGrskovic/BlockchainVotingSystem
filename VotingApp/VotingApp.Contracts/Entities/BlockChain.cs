using System.Text.Json;
using VotingApp.Contracts.Dtos;

namespace VotingApp.Contracts.Entities;

public class BlockChain
{
    public Guid Id { get; set; } = default!;
    public string Blocks { get; set; } = default!;

    public BlockChain(BlockChainDto blockChainDto)
    {
        Blocks = JsonSerializer.Serialize(blockChainDto.Blocks);
    }

    public BlockChain()
    {

    }

    public bool Equals(BlockChain other)
    {
        return other != null && Blocks.Equals(other.Blocks);
    }
}
