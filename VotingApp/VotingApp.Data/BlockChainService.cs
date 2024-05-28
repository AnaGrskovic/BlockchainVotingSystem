using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Text.Json;
using VotingApp.Contracts.Dtos;
using VotingApp.Contracts.Entities;
using VotingApp.Contracts.Exceptions;
using VotingApp.Contracts.Services;
using VotingApp.Contracts.Settings;
using VotingApp.Contracts.UoW;

namespace VotingApp.Services;

public class BlockChainService : IBlockChainService
{
    private readonly IUnitOfWork _uow;
    private readonly ThresholdsSettings _thresholdsSettings;
    private readonly CandidatesSettings _candidatesSettings;

    public BlockChainService(IUnitOfWork uow, IOptions<ThresholdsSettings> thresholdsSettings, IOptions<CandidatesSettings> candidatesSettings)
    {
        _uow = uow;
        _thresholdsSettings = thresholdsSettings.Value;
        _candidatesSettings = candidatesSettings.Value;
    }

    public async Task CreateAsync(BlockChainDto blockChainDto)
    {
        var blockChain = new BlockChain(blockChainDto);
        _uow.BlockChains.Add(blockChain);
        await _uow.SaveChangesAsync();
    }

    public async Task<VotingResultDto> GetVotingResultAsync()
    { 
        // TODO if before threshold just return number

        List<BlockChain> blockChains = await GetAllAsync();

        Dictionary<BlockChain, int> blockChainGroups = GetBlockChainGroups(blockChains);

        BlockChain largestBlockChainGroup = GetLargestBlockChainGroup(blockChainGroups);

        bool areResultsAcceptable = CheckIfLargestBlockChainGroupIsLargeEnough(blockChainGroups, blockChains.Count);
        if (!areResultsAcceptable)
        {
            throw new VotingResultUnacceptableException("Due to a too big number of incorrect blockchains, the results are not acceptable");
        }

        return CalculateVotingResult(largestBlockChainGroup);
    }

    private async Task<List<BlockChain>> GetAllAsync()
    {
        return await _uow.BlockChains.GetAllAsync();
    }

    private Dictionary<BlockChain, int> GetBlockChainGroups(List<BlockChain> blockChains)
    {
        Dictionary<BlockChain, int> blockChainGroups = new Dictionary<BlockChain, int>();
        foreach (BlockChain blockChain in blockChains)
        {
            if (blockChainGroups.Keys.Count == 0)
            {
                blockChainGroups.Add(blockChain, 1);
            }
            else
            {
                bool didFindGroup = false;
                foreach (BlockChain blockChainGroup in blockChainGroups.Keys)
                {
                    if (blockChain.Equals(blockChainGroup))
                    {
                        blockChainGroups[blockChainGroup]++;
                        didFindGroup = true;
                    }
                }
                if (!didFindGroup)
                {
                    blockChainGroups.Add(blockChain, 1);
                }
            }
        }
        return blockChainGroups;
    }

    private BlockChain GetLargestBlockChainGroup(Dictionary<BlockChain, int> blockChainGroups)
    {
        return blockChainGroups.Aggregate((x, y) => x.Value > y.Value ? x : y).Key;
    }

    private bool CheckIfLargestBlockChainGroupIsLargeEnough(Dictionary<BlockChain, int> blockChainGroups, int totalNumberOfBlockChains)
    {
        BlockChain largestBlockChainGroup = GetLargestBlockChainGroup(blockChainGroups);

        int largestBlockChainGroupSize = blockChainGroups[largestBlockChainGroup];

        if (_thresholdsSettings.MinimalPercentageOfCorrectBlockChains < 1 || _thresholdsSettings.MinimalPercentageOfCorrectBlockChains > 100)
        {
            throw new InvalidSettingsException("Minimal percentage of correct blockchains must be a number between 1 and 100.");
        }

        double percentageOfCorrectBlockChains = largestBlockChainGroupSize / totalNumberOfBlockChains;
        return percentageOfCorrectBlockChains < _thresholdsSettings.MinimalPercentageOfCorrectBlockChains;
    }

    private VotingResultDto CalculateVotingResult(BlockChain blockChain)
    {
        Dictionary<string, int> numberOfVotesPerCandidate = new Dictionary<string, int>();

        foreach (string candidate in _candidatesSettings.Candidates) {
            numberOfVotesPerCandidate.Add(candidate, 0);
        }

        IEnumerable<BlockDto> blocks = JsonSerializer.Deserialize<IEnumerable<BlockDto>>(blockChain.Blocks)
            ?? throw new UnsuccessfulSerializationException("Unable to deserialize the blockchain.");


        throw new NotImplementedException();
    }
}
