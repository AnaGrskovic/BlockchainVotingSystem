package diplrad.tcp.blockchain;

import com.google.gson.Gson;
import diplrad.constants.Constants;
import diplrad.helpers.CryptographyHelper;
import diplrad.models.blockchain.VotingBlockChain;
import diplrad.models.blockchain.VotingBlockChainSingleton;
import diplrad.models.peer.Peer;
import diplrad.tcp.TcpClient;

import javax.crypto.BadPaddingException;
import javax.crypto.IllegalBlockSizeException;
import javax.crypto.NoSuchPaddingException;
import java.io.IOException;
import java.security.InvalidAlgorithmParameterException;
import java.security.InvalidKeyException;
import java.security.NoSuchAlgorithmException;

public class BlockChainTcpClient extends TcpClient {

    public static String password = "e52217e3ee213ef1ffdee3a192e2ac7e";
    public static String initializationVector = "000102030405060708090a0b0c0d0e0f";

    public void sendConnect(Gson gson, Peer ownPeer) throws IOException {

        try {
            String originalText = "baeldung";
            String encryptedText = CryptographyHelper.encryptWithAes("e52217e3ee213ef1ffdee3a192e2ac7e", "000102030405060708090a0b0c0d0e0f", originalText);
            String decryptedText = CryptographyHelper.decryptWithAes("e52217e3ee213ef1ffdee3a192e2ac7e", "000102030405060708090a0b0c0d0e0f", encryptedText);
            var a = 9;
        } catch (NoSuchPaddingException | NoSuchAlgorithmException | InvalidAlgorithmParameterException |
                 InvalidKeyException | IllegalBlockSizeException | BadPaddingException e) {
            var s = 8;
        }

        String response = sendMessage(Constants.TCP_CONNECT + " " + gson.toJson(ownPeer));
        VotingBlockChain blockchain = gson.fromJson(response, VotingBlockChain.class);
        BlockChainTcpMessageObserver observer = new BlockChainTcpMessageObserver(gson);
        observer.blockChainMessageReceived(blockchain);
    }

    public void sendDisconnect(Gson gson, Peer ownPeer) throws IOException {
        sendMessage(Constants.TCP_DISCONNECT + " " + gson.toJson(ownPeer));
    }

    public void sendBlockchain(Gson gson) throws IOException {
        sendMessage(gson.toJson(VotingBlockChainSingleton.getInstance()));
    }



}
