package diplrad.models.blockchain;

import com.google.gson.Gson;
import com.google.gson.GsonBuilder;
import diplrad.constants.Constants;
import diplrad.tcp.blockchain.BlockChainTcpMessageObserver;
import org.junit.Test;

import static org.junit.Assert.assertEquals;


public class BlockChainTcpMessageObserverTest {

    @Test
    public void givenBlockchain_whenNewBlockIsMined_thenItsHashBeginsWithPrefixString() {

        Gson gson = new GsonBuilder().setPrettyPrinting().excludeFieldsWithoutExposeAnnotation().create();
        BlockChainTcpMessageObserver observer = new BlockChainTcpMessageObserver(gson);

        String actualResponse = observer.messageReceived(Constants.BLOCKCHAIN_REQUEST);
        String expectedResponse = gson.toJson(VotingBlockChainSingleton.getInstance());

        assertEquals(actualResponse, expectedResponse);
    }

}