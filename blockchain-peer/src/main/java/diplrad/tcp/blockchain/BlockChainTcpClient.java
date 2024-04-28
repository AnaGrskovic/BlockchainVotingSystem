package diplrad.tcp.blockchain;

import com.google.gson.Gson;
import diplrad.constants.Constants;
import diplrad.encryption.CryptographyHelper;
import diplrad.exceptions.CryptographyException;
import diplrad.models.blockchain.VotingBlockChain;
import diplrad.models.blockchain.VotingBlockChainSingleton;
import diplrad.models.peer.Peer;
import diplrad.tcp.TcpClient;

import java.io.IOException;

import static diplrad.helpers.ExceptionHandler.handleFatalException;

public class BlockChainTcpClient extends TcpClient {

    public void sendConnect(Gson gson, Peer ownPeer) throws IOException {
        String message = Constants.TCP_CONNECT + " " + gson.toJson(ownPeer);
        String response = encryptAndSendMessage(message);
        VotingBlockChain blockchain = gson.fromJson(response, VotingBlockChain.class);
        BlockChainTcpMessageObserver observer = new BlockChainTcpMessageObserver(gson);
        observer.blockChainMessageReceived(blockchain);
    }

    public void sendDisconnect(Gson gson, Peer ownPeer) throws IOException {
        String message = Constants.TCP_DISCONNECT + " " + gson.toJson(ownPeer);
        encryptAndSendMessage(message);
    }

    public void sendBlockchain(Gson gson) throws IOException {
        String message = gson.toJson(VotingBlockChainSingleton.getInstance());
        encryptAndSendMessage(message);
    }

    private String encryptAndSendMessage(String message) throws IOException {
        String encryptedMessage = null;
        try {
            encryptedMessage = CryptographyHelper.encrypt(message);
        } catch (CryptographyException e) {
            handleFatalException(e);
        }
        return sendMessage(encryptedMessage);
    }

}
