package diplrad.queue;

import com.google.gson.Gson;
import com.google.gson.GsonBuilder;
import diplrad.models.blockchain.Block;
import diplrad.models.blockchain.VotingBlockChain;
import diplrad.models.blockchain.VotingBlockChainSingleton;
import diplrad.models.peer.PeersSingleton;
import org.junit.Test;
import org.mockito.MockedStatic;

import java.util.List;

import static org.mockito.Mockito.mockStatic;
import static org.mockito.Mockito.times;

public class AzureMessageQueueClientTest {

    private static Gson gson = new GsonBuilder().create();

    public VotingBlockChain setUpVotingBlockChain()  {
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

    @Test
    public void handleQueueMessage_whenVoteIsInvalid_thenDontFetchPeersToSendThemTheBlockChain() {

        try (MockedStatic<VotingBlockChainSingleton> mockedStaticBlockChain = mockStatic(VotingBlockChainSingleton.class)) {
            try (MockedStatic<PeersSingleton> mockedStaticPeers = mockStatic(PeersSingleton.class)) {

                // Arrange
                AzureMessageQueueClient azureMessageQueueClient = new AzureMessageQueueClient(gson);
                mockedStaticBlockChain.when(VotingBlockChainSingleton::getInstance).thenReturn(setUpVotingBlockChain());

                // Act
                azureMessageQueueClient.handleQueueMessage("invalid vote");

                // Assert
                mockedStaticPeers.verify(PeersSingleton::getInstance, times(0));

            }
        }

    }

    @Test
    public void handleQueueMessage_whenVoteIsValid_thenFetchPeersToSendThemTheBlockChain() {

        try (MockedStatic<VotingBlockChainSingleton> mockedStaticBlockChain = mockStatic(VotingBlockChainSingleton.class)) {
            try (MockedStatic<PeersSingleton> mockedStaticPeers = mockStatic(PeersSingleton.class)) {

                // Arrange
                AzureMessageQueueClient azureMessageQueueClient = new AzureMessageQueueClient(gson);
                VotingBlockChain blockChain = setUpVotingBlockChain();
                mockedStaticBlockChain.when(VotingBlockChainSingleton::getInstance).thenReturn(blockChain);

                // Act
                azureMessageQueueClient.handleQueueMessage(blockChain.getCandidates().get(0));

                // Assert
                mockedStaticPeers.verify(PeersSingleton::getInstance, times(1));

            }
        }

    }

}
