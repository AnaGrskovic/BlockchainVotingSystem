package diplrad.tcp.blockchain;

import com.google.gson.Gson;
import com.google.gson.GsonBuilder;
import diplrad.constants.Constants;
import diplrad.cryptography.CryptographyHelper;
import diplrad.exceptions.CryptographyException;
import diplrad.exceptions.TcpException;
import diplrad.models.blockchain.Block;
import diplrad.models.blockchain.VotingBlockChain;
import diplrad.models.blockchain.VotingBlockChainSingleton;
import diplrad.models.peer.Peer;
import diplrad.models.peer.PeersSingleton;
import org.junit.Test;
import org.mockito.MockedStatic;

import java.util.List;
import java.util.UUID;

import static diplrad.mocks.BlockChainMocks.setUpVotingBlockChain;
import static org.mockito.ArgumentMatchers.any;
import static org.mockito.Mockito.mockStatic;
import static org.mockito.Mockito.times;

public class BlockChainTcpMessageObserverTest {

    private final Gson gson = new GsonBuilder().create();
    private final BlockChainTcpMessageObserver observer = new BlockChainTcpMessageObserver(gson);

    public static void setUpPeers() throws CryptographyException {
        Peer peer1 = new Peer(UUID.randomUUID(), "168.198.2.23", 5000);
        Peer peer2 = new Peer(UUID.randomUUID(), "168.198.2.24", 5000);
        Peer peer3 = new Peer(UUID.randomUUID(), "168.198.2.25", 5000);
        List<Peer> peers = List.of(peer1, peer2, peer3);
        PeersSingleton.createInstance(peers);
        CryptographyHelper.loadCryptographyProperties();
    }

    @Test
    public void messageReceived_whenMessageIsConnect_thenAddPeer() throws TcpException, CryptographyException {

        setUpPeers();

        try (MockedStatic<PeersSingleton> mockedStatic = mockStatic(PeersSingleton.class)) {

            // Arrange
            Peer peer = new Peer(UUID.randomUUID(), "168.182.1.11", 5000);

            String message = Constants.TCP_CONNECT + " " + gson.toJson(peer);
            String encryptedMessage = CryptographyHelper.encrypt(message);

            // Act
            observer.messageReceived(encryptedMessage);

            // Assert
            mockedStatic.verify(() -> PeersSingleton.addPeer(any(Peer.class)), times(1));

        }

    }

    @Test
    public void messageReceived_whenMessageIsConnect_thenRespondWithBlockChainInstance() throws TcpException, CryptographyException {

        setUpPeers();

        try (MockedStatic<VotingBlockChainSingleton> mockedStatic = mockStatic(VotingBlockChainSingleton.class)) {

            // Arrange
            Peer peer = new Peer(UUID.randomUUID(), "168.182.1.11", 5000);

            String message = Constants.TCP_CONNECT + " " + gson.toJson(peer);
            String encryptedMessage = CryptographyHelper.encrypt(message);

            // Act
            observer.messageReceived(encryptedMessage);

            // Assert
            mockedStatic.verify(VotingBlockChainSingleton::getInstance, times(1));

        }

    }

    @Test
    public void messageReceived_whenMessageIsDisconnect_thenRemovePeer() throws TcpException, CryptographyException {

        setUpPeers();

        try (MockedStatic<PeersSingleton> mockedStatic = mockStatic(PeersSingleton.class)) {

            // Arrange
            Peer peer = new Peer(UUID.randomUUID(), "168.182.1.11", 5000);

            String message = Constants.TCP_DISCONNECT + " " + gson.toJson(peer);
            String encryptedMessage = CryptographyHelper.encrypt(message);

            // Act
            observer.messageReceived(encryptedMessage);

            // Assert
            mockedStatic.verify(() -> PeersSingleton.removePeer(any(Peer.class)), times(1));

        }

    }

    @Test
    public void messageReceived_whenMessageIsBlockChainInvalid_thenDoNotSetInstance() throws TcpException, CryptographyException {

        setUpPeers();

        try (MockedStatic<VotingBlockChainSingleton> mockedStatic = mockStatic(VotingBlockChainSingleton.class)) {

            // Arrange
            VotingBlockChain currentBlockChain = setUpVotingBlockChain();
            VotingBlockChain incomingBlockChain = currentBlockChain.copy();

            mockedStatic.when(VotingBlockChainSingleton::getInstance).thenReturn(currentBlockChain);

            Block fourthBlockIncomingBlockChain = new Block("Candidate3", incomingBlockChain.getLastBlockHash());
            incomingBlockChain.mineBlock(fourthBlockIncomingBlockChain);
            incomingBlockChain.getBlock(1).setData("Candidate3");
            incomingBlockChain.getBlock(2).setData("Candidate3");

            String message = gson.toJson(incomingBlockChain);
            String encryptedMessage = CryptographyHelper.encrypt(message);

            // Act
            observer.messageReceived(encryptedMessage);

            // Assert
            mockedStatic.verify(() -> VotingBlockChainSingleton.setInstance(any(VotingBlockChain.class)), times(0));

        }

    }

    @Test
    public void messageReceived_whenMessageIsBlockChainButCurrentIsEmpty_thenSetInstance() throws TcpException, CryptographyException {

        setUpPeers();

        try (MockedStatic<VotingBlockChainSingleton> mockedStatic = mockStatic(VotingBlockChainSingleton.class)) {

            // Arrange
            List<String> candidates = List.of("Candidate1", "Candidate2", "Candidate3");
            VotingBlockChain currentBlockChain = new VotingBlockChain(candidates);
            VotingBlockChain incomingBlockChain = currentBlockChain.copy();

            mockedStatic.when(VotingBlockChainSingleton::getInstance).thenReturn(currentBlockChain);

            Block firstBlockIncomingBlockChain = new Block("Candidate1", incomingBlockChain.getLastBlockHash());
            incomingBlockChain.mineBlock(firstBlockIncomingBlockChain);
            Block secondBlockIncomingBlockChain = new Block("Candidate2", incomingBlockChain.getLastBlockHash());
            incomingBlockChain.mineBlock(secondBlockIncomingBlockChain);
            Block thirdBlockIncomingBlockChain = new Block("Candidate3", incomingBlockChain.getLastBlockHash());
            incomingBlockChain.mineBlock(thirdBlockIncomingBlockChain);

            String message = gson.toJson(incomingBlockChain);
            String encryptedMessage = CryptographyHelper.encrypt(message);

            // Act
            observer.messageReceived(encryptedMessage);

            // Assert
            mockedStatic.verify(() -> VotingBlockChainSingleton.setInstance(any(VotingBlockChain.class)), times(1));

        }

    }

    @Test
    public void messageReceived_whenMessageIsBlockChainBiggerAndIncompatibleWithCurrent_thenDoNotSetInstance() throws TcpException, CryptographyException {

        setUpPeers();

        try (MockedStatic<VotingBlockChainSingleton> mockedStatic = mockStatic(VotingBlockChainSingleton.class)) {

            // Arrange
            VotingBlockChain currentBlockChain = setUpVotingBlockChain();
            VotingBlockChain incomingBlockChain = currentBlockChain.copy();

            Block fourthBlockCurrentBlockChain = new Block("Candidate1", currentBlockChain.getLastBlock().getHash());
            currentBlockChain.mineBlock(fourthBlockCurrentBlockChain);
            mockedStatic.when(VotingBlockChainSingleton::getInstance).thenReturn(currentBlockChain);

            Block fourthBlockIncomingBlockChain = new Block("Candidate3", incomingBlockChain.getLastBlockHash());
            incomingBlockChain.mineBlock(fourthBlockIncomingBlockChain);
            Block fifthBlockIncomingBlockChain = new Block("Candidate2", incomingBlockChain.getLastBlockHash());
            incomingBlockChain.mineBlock(fifthBlockIncomingBlockChain);

            String message = gson.toJson(incomingBlockChain);
            String encryptedMessage = CryptographyHelper.encrypt(message);

            // Act
            observer.messageReceived(encryptedMessage);

            // Assert
            mockedStatic.verify(() -> VotingBlockChainSingleton.setInstance(any(VotingBlockChain.class)), times(0));

        }

    }

    @Test
    public void messageReceived_whenMessageIsBlockChainBiggerThanCurrent_thenSetInstance() throws TcpException, CryptographyException {

        setUpPeers();

        try (MockedStatic<VotingBlockChainSingleton> mockedStatic = mockStatic(VotingBlockChainSingleton.class)) {

            // Arrange
            VotingBlockChain currentBlockChain = setUpVotingBlockChain();
            VotingBlockChain incomingBlockChain = currentBlockChain.copy();

            mockedStatic.when(VotingBlockChainSingleton::getInstance).thenReturn(currentBlockChain);

            Block fourthBlockIncomingBlockChain = new Block("Candidate2", incomingBlockChain.getLastBlockHash());
            incomingBlockChain.mineBlock(fourthBlockIncomingBlockChain);

            String message = gson.toJson(incomingBlockChain);
            String encryptedMessage = CryptographyHelper.encrypt(message);

            // Act
            observer.messageReceived(encryptedMessage);

            // Assert
            mockedStatic.verify(() -> VotingBlockChainSingleton.setInstance(any(VotingBlockChain.class)), times(1));

        }

    }

    @Test
    public void messageReceived_whenMessageIsBlockChainOfSameSizeAndIncompatibleWithCurrent_thenDoNotSetInstance() throws TcpException, CryptographyException {

        setUpPeers();

        try (MockedStatic<VotingBlockChainSingleton> mockedStatic = mockStatic(VotingBlockChainSingleton.class)) {

            // Arrange
            VotingBlockChain currentBlockChain = setUpVotingBlockChain();
            VotingBlockChain incomingBlockChain = currentBlockChain.copy();

            Block fourthBlockCurrentBlockChain = new Block("Candidate1", currentBlockChain.getLastBlockHash());
            currentBlockChain.mineBlock(fourthBlockCurrentBlockChain);
            Block fifthBlockCurrentBlockChain = new Block("Candidate2", currentBlockChain.getLastBlockHash());
            currentBlockChain.mineBlock(fifthBlockCurrentBlockChain);
            mockedStatic.when(VotingBlockChainSingleton::getInstance).thenReturn(currentBlockChain);

            Block fourthBlockIncomingBlockChain = new Block("Candidate3", incomingBlockChain.getLastBlockHash());
            incomingBlockChain.mineBlock(fourthBlockIncomingBlockChain);
            Block fifthBlockIncomingBlockChain = new Block("Candidate3", currentBlockChain.getLastBlockHash());
            incomingBlockChain.mineBlock(fifthBlockIncomingBlockChain);

            String message = gson.toJson(incomingBlockChain);
            String encryptedMessage = CryptographyHelper.encrypt(message);

            // Act
            observer.messageReceived(encryptedMessage);

            // Assert
            mockedStatic.verify(() -> VotingBlockChainSingleton.setInstance(any(VotingBlockChain.class)), times(0));

        }

    }

    @Test
    public void messageReceived_whenMessageIsBlockChainOfSameSizeAsCurrentWithBiggerLastBlockTimeStamp_thenDoNotSetInstance() throws TcpException, CryptographyException {

        setUpPeers();

        try (MockedStatic<VotingBlockChainSingleton> mockedStatic = mockStatic(VotingBlockChainSingleton.class)) {

            // Arrange
            VotingBlockChain currentBlockChain = setUpVotingBlockChain();
            VotingBlockChain incomingBlockChain = currentBlockChain.copy();

            Block fourthBlockCurrentBlockChain = new Block("Candidate1", currentBlockChain.getLastBlockHash());
            currentBlockChain.mineBlock(fourthBlockCurrentBlockChain);
            mockedStatic.when(VotingBlockChainSingleton::getInstance).thenReturn(currentBlockChain);

            Block fourthBlockIncomingBlockChain = new Block("Candidate3", incomingBlockChain.getLastBlockHash());
            incomingBlockChain.mineBlock(fourthBlockIncomingBlockChain);

            String message = gson.toJson(incomingBlockChain);
            String encryptedMessage = CryptographyHelper.encrypt(message);

            // Act
            observer.messageReceived(encryptedMessage);

            // Assert
            mockedStatic.verify(() -> VotingBlockChainSingleton.setInstance(any(VotingBlockChain.class)), times(0));

        } catch (CryptographyException e) {
            throw new RuntimeException(e);
        }

    }

    @Test
    public void messageReceived_whenMessageIsBlockChainTooSmall_thenDoNotSetInstance() throws TcpException, CryptographyException {

        setUpPeers();

        try (MockedStatic<VotingBlockChainSingleton> mockedStatic = mockStatic(VotingBlockChainSingleton.class)) {

            // Arrange
            VotingBlockChain currentBlockChain = setUpVotingBlockChain();
            VotingBlockChain incomingBlockChain = currentBlockChain.copy();

            Block fourthBlockCurrentBlockChain = new Block("Candidate1", currentBlockChain.getLastBlock().getHash());
            currentBlockChain.mineBlock(fourthBlockCurrentBlockChain);
            mockedStatic.when(VotingBlockChainSingleton::getInstance).thenReturn(currentBlockChain);

            String message = gson.toJson(incomingBlockChain);
            String encryptedMessage = CryptographyHelper.encrypt(message);

            // Act
            observer.messageReceived(encryptedMessage);

            // Assert
            mockedStatic.verify(() -> VotingBlockChainSingleton.setInstance(any(VotingBlockChain.class)), times(0));

        }

    }

    @Test
    public void messageReceived_whenMessageIsBlockChainTooBig_thenDoNotSetInstance() throws TcpException, CryptographyException {

        setUpPeers();

        try (MockedStatic<VotingBlockChainSingleton> mockedStatic = mockStatic(VotingBlockChainSingleton.class)) {

            // Arrange
            VotingBlockChain currentBlockChain = setUpVotingBlockChain();
            VotingBlockChain incomingBlockChain = currentBlockChain.copy();

            mockedStatic.when(VotingBlockChainSingleton::getInstance).thenReturn(currentBlockChain);

            Block fourthBlockIncomingBlockChain = new Block("Candidate3", incomingBlockChain.getLastBlockHash());
            incomingBlockChain.mineBlock(fourthBlockIncomingBlockChain);
            Block fifthBlockIncomingBlockChain = new Block("Candidate3", currentBlockChain.getLastBlockHash());
            incomingBlockChain.mineBlock(fifthBlockIncomingBlockChain);

            String message = gson.toJson(incomingBlockChain);
            String encryptedMessage = CryptographyHelper.encrypt(message);

            // Act
            observer.messageReceived(encryptedMessage);

            // Assert
            mockedStatic.verify(() -> VotingBlockChainSingleton.setInstance(any(VotingBlockChain.class)), times(0));

        }

    }

}
