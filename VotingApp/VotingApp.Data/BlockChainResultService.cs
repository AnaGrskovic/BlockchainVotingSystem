using Microsoft.Extensions.Options;
using System.Text.Json;
using VotingApp.Contracts.Dtos;
using VotingApp.Contracts.Entities;
using VotingApp.Contracts.Exceptions;
using VotingApp.Contracts.Services;
using VotingApp.Contracts.Settings;

namespace VotingApp.Services;

public class BlockChainResultService : IBlockChainResultService
{
    private readonly ITimeService _timeService;
    private readonly IBackupService _backupService;
    private readonly IBlockChainService _blockChainService;
    private readonly ThresholdsSettings _thresholdsSettings;
    private readonly CandidatesSettings _candidatesSettings;

    public BlockChainResultService(ITimeService timeService, IBackupService backupService, IBlockChainService blockChainService, IOptions<ThresholdsSettings> thresholdsSettings, IOptions<CandidatesSettings> candidatesSettings)
    {
        _timeService = timeService;
        _backupService = backupService;
        _blockChainService = blockChainService;
        _thresholdsSettings = thresholdsSettings.Value;
        _candidatesSettings = candidatesSettings.Value;
    }

    public async Task<VotingResultDto> GetVotingResultAsync()
    {
        if (_timeService.IsBeforeVotingTime())
        {
            return new VotingResultDto();
        }
        else if (!_timeService.IsAfterStabilizationTime())
        {
            var estimatedTotalNumberOfVotes = await _backupService.GetCountAsync();
            return new VotingResultDto(estimatedTotalNumberOfVotes);
        }

        List<BlockChain> blockChains = await _blockChainService.GetAllAsync();

        Dictionary<BlockChain, int> blockChainGroups = GetBlockChainGroups(blockChains);

        BlockChain largestBlockChainGroup = GetLargestBlockChainGroup(blockChainGroups);

        bool areResultsAcceptable = CheckIfLargestBlockChainGroupIsLargeEnough(blockChainGroups, blockChains.Count);
        if (!areResultsAcceptable)
        {
            throw new VotingResultUnacceptableException("Due to a too big number of incorrect blockchains, the results are not acceptable.");
        }

        return CalculateVotingResult(largestBlockChainGroup);
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

        double percentageOfCorrectBlockChains = 100 * (double)largestBlockChainGroupSize / (double)totalNumberOfBlockChains;
        return percentageOfCorrectBlockChains > _thresholdsSettings.MinimalPercentageOfCorrectBlockChains;
    }

    private VotingResultDto CalculateVotingResult(BlockChain blockChain)
    {
        Dictionary<string, int> numberOfVotesPerCandidate = new Dictionary<string, int>();

        foreach (string candidate in _candidatesSettings.Candidates)
        {
            numberOfVotesPerCandidate.Add(candidate, 0);
        }

        IEnumerable<BlockDto> blocks = JsonSerializer.Deserialize<IEnumerable<BlockDto>>(blockChain.Blocks)
            ?? throw new UnsuccessfulSerializationException("Unable to deserialize the blockchain.");

        foreach (BlockDto block in blocks.ToList().GetRange(1, blocks.Count() - 1))
        {
            if (!numberOfVotesPerCandidate.ContainsKey(block.Data))
            {
                throw new VotingResultUnacceptableException("Voting results are invalid because they contain invalid candidates.");
            }
            numberOfVotesPerCandidate[block.Data]++;
        }

        int totalNumberOfVotes = blocks.Count() - 1;

        return new VotingResultDto(totalNumberOfVotes, numberOfVotesPerCandidate);
    }
}
