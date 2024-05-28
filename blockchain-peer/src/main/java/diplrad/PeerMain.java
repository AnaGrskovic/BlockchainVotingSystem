package diplrad;

import com.google.gson.Gson;
import com.google.gson.GsonBuilder;
import diplrad.constants.Constants;
import diplrad.constants.LogMessages;
import diplrad.cryptography.CryptographyHelper;
import diplrad.exceptions.*;
import diplrad.helpers.DigitalSignatureHelper;
import diplrad.queue.AzureMessageQueueClient;
import diplrad.tcp.blockchain.BlockChainTcpClientHelper;
import diplrad.http.PeerHttpHelper;
import diplrad.http.HttpSender;
import diplrad.models.blockchain.VotingBlockChainSingleton;
import diplrad.tcp.TcpServer;
import org.bouncycastle.jce.provider.BouncyCastleProvider;

import java.security.Security;
import java.time.LocalDateTime;

import static diplrad.helpers.ArgsProcessHelper.initializePrivateKeyPem;
import static diplrad.helpers.ArgsProcessHelper.initializeTcpServer;
import static diplrad.helpers.ExceptionHandler.handleFatalException;
import static diplrad.models.peer.PeersSingleton.ownPeer;

public class PeerMain {

    private static final Gson gson = new GsonBuilder().create();

    public static void main(String[] args) {

        initializeTcpServer(args);

        String privateKeyPem = initializePrivateKeyPem(args);

        try {

            Security.addProvider(new BouncyCastleProvider());

            HttpSender httpSender = new HttpSender();
            ownPeer = PeerHttpHelper.createOwnPeer(httpSender);
            PeerHttpHelper.getPeersInitial(httpSender, ownPeer);
            System.out.println(LogMessages.registeredOwnPeer);

            CryptographyHelper.loadCryptographyProperties();

            TcpServer.TcpServerThread tcpServerThread = new TcpServer.TcpServerThread();
            tcpServerThread.start();
            System.out.printf((LogMessages.startedTcpServer) + "%n", TcpServer.tcpServerPort);

            BlockChainTcpClientHelper.createTcpClientsAndSendConnects(gson, ownPeer);
            System.out.printf((LogMessages.receivedInitialBlockChain) + "%n", gson.toJson(VotingBlockChainSingleton.getInstance()));

            AzureMessageQueueClient azureMessageQueueClient = new AzureMessageQueueClient(gson);
            while (LocalDateTime.now().isBefore(Constants.VOTING_END_DATE_TIME)) {
                azureMessageQueueClient.receiveAndHandleQueueMessage();
            }

            Thread.sleep(Constants.VOTING_STABILIZE_MINUTES * 60 * 1000);

            var finalBlockChain = VotingBlockChainSingleton.getInstance();
            var signedFinalBlockChain = DigitalSignatureHelper.signBlockChain(finalBlockChain, privateKeyPem, gson);
            httpSender.createBlockChain(finalBlockChain, signedFinalBlockChain, privateKeyPem);

        } catch (IpException | ParseException | HttpException | TcpException | CryptographyException | InterruptedException e) {
            handleFatalException(e);
        }

    }

}
