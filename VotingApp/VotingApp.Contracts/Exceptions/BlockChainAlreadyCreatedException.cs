namespace VotingApp.Contracts.Exceptions;

public class BlockChainAlreadyCreatedException : Exception
{
    public BlockChainAlreadyCreatedException(string message) : base(message)
    {
    }
}
