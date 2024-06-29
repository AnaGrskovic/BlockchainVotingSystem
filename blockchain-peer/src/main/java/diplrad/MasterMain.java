package diplrad;

import com.google.gson.Gson;
import com.google.gson.GsonBuilder;
import diplrad.constants.Constants;
import diplrad.constants.LogMessages;
import diplrad.cryptography.CryptographyHelper;
import diplrad.exceptions.*;
import diplrad.helpers.DigitalSignatureHelper;
import diplrad.helpers.WaitHelper;
import diplrad.http.PeerHttpHelper;
import diplrad.http.HttpSender;
import diplrad.models.blockchain.PeerBlockChain;
import diplrad.models.blockchain.VotingBlockChainSingleton;
import diplrad.models.peer.PeerRequest;
import diplrad.queue.AzureMessageQueueClient;
import diplrad.tcp.TcpServer;
import org.bouncycastle.jce.provider.BouncyCastleProvider;

import java.security.*;
import java.time.LocalDateTime;

import static diplrad.helpers.ArgsProcessHelper.*;
import static diplrad.helpers.ExceptionHandler.handleFatalException;
import static diplrad.helpers.FileReader.readCandidatesFromFile;
import static diplrad.models.peer.PeersSingleton.ownPeer;

public class MasterMain {

    private static final Gson gson = new GsonBuilder().create();

    public static void main(String[] args) {

        initializeTcpServer(args);
        String privateKeyPem = initializePrivateKeyPem(args);
        String publicKeyPem = initializePublicKeyPem(args);

        try {

            Security.addProvider(new BouncyCastleProvider());

            VotingBlockChainSingleton.createInstance(readCandidatesFromFile());
            System.out.printf((LogMessages.createdBlockChainMessage) + "%n", gson.toJson(VotingBlockChainSingleton.getInstance()));

            HttpSender httpSender = new HttpSender();
            ownPeer = PeerHttpHelper.createOwnPeer(httpSender);
            PeerHttpHelper.getPeersInitial(httpSender, ownPeer);
            System.out.println(LogMessages.registeredOwnPeer);

            CryptographyHelper.loadCryptographyProperties();

            TcpServer.TcpServerThread tcpServerThread = new TcpServer.TcpServerThread();
            tcpServerThread.start();
            System.out.printf((LogMessages.startedTcpServer) + "%n", TcpServer.tcpServerPort);

            while (LocalDateTime.now().isBefore(Constants.VOTING_START_DATE_TIME)) {
                Thread.sleep(1000);
            }
            System.out.println(LogMessages.votingTimeStart);

            AzureMessageQueueClient azureMessageQueueClient = new AzureMessageQueueClient(gson);
            while (LocalDateTime.now().isBefore(Constants.VOTING_END_DATE_TIME)) {
                azureMessageQueueClient.receiveAndHandleQueueMessage();
            }

            System.out.println(LogMessages.votingTimeEnd);
            WaitHelper.doUselessWork(Constants.VOTING_STABILIZE_MINUTES);
            System.out.println(LogMessages.voteProcessingTimeEnd);

            var ownPeerRequest = new PeerRequest(ownPeer.getIpAddress(), ownPeer.getPort());
            var finalBlockChain = VotingBlockChainSingleton.getInstance();
            var peerBlockChain = new PeerBlockChain(ownPeerRequest, finalBlockChain);
            var signedPeerBlockChain = DigitalSignatureHelper.signPeerBlockChain(peerBlockChain, privateKeyPem);
            httpSender.createBlockChain(peerBlockChain, signedPeerBlockChain, publicKeyPem);
            System.out.println(LogMessages.sentBlockChainToApi);
            System.exit(0);

        } catch (InvalidFileException | ReadFromFileException | IpException | ParseException | HttpException | CryptographyException | InterruptedException e) {
            handleFatalException(e);
        }

    }

}
