package diplrad.models.blockchain;

import org.junit.Test;

import static diplrad.mocks.BlockChainMocks.setUpVotingBlockChain;
import static org.junit.Assert.assertFalse;
import static org.junit.jupiter.api.Assertions.assertTrue;

public class VotingBlockChainTest {

    @Test
    public void givenBlockchain_whenNotChanged_thenValidationOk() {

        // Arrange
        VotingBlockChain blockChain = setUpVotingBlockChain();

        // Act

        // Assert
        boolean validationResult = blockChain.validate();
        assertTrue(validationResult);

    }

    @Test
    public void givenBlockchain_whenChanged_thenValidationFailed() {

        // Arrange
        BlockChain blockChain = setUpVotingBlockChain();

        // Act
        blockChain.getBlock(1).setData("Candidate3");
        blockChain.getBlock(2).setData("Candidate3");

        // Assert
        boolean validationResult = blockChain.validate();
        assertFalse(validationResult);

    }

    @Test
    public void givenBlockchain_whenDataNotCandidate_thenValidationFailed() {

        // Arrange
        BlockChain blockChain = setUpVotingBlockChain();

        // Act
        Block invalidDataBlock = new Block("Candidate4", blockChain.getLastBlockHash());
        blockChain.mineBlock(invalidDataBlock);

        // Assert
        boolean validationResult = blockChain.validate();
        assertFalse(validationResult);

    }

}
