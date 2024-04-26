package diplrad.mocks;

import diplrad.models.blockchain.Block;
import diplrad.models.blockchain.BlockChain;
import diplrad.models.blockchain.VotingBlockChain;

import java.util.List;

public class BlockChainMocks {

    public static BlockChain setUpBlockChain()  {
        BlockChain blockChain = new BlockChain();
        Block firstBlock = new Block("The is the First Block.", blockChain.getLastBlockHash());
        blockChain.mineBlock(firstBlock);
        Block secondBlock = new Block("The is a Second Block.", blockChain.getLastBlockHash());
        blockChain.mineBlock(secondBlock);
        return blockChain;
    }

    public static VotingBlockChain setUpVotingBlockChain()  {
        List<String> candidates = List.of("Candidate1", "Candidate2", "Candidate3");
        VotingBlockChain blockChain = new VotingBlockChain(candidates);
        Block firstBlock = new Block("Candidate1", blockChain.getLastBlockHash());
        blockChain.mineBlock(firstBlock);
        Block secondBlock = new Block("Candidate2", blockChain.getLastBlockHash());
        blockChain.mineBlock(secondBlock);
        Block thirdBlock = new Block("Candidate3", blockChain.getLastBlockHash());
        blockChain.mineBlock(thirdBlock);
        return blockChain;
    }

}
