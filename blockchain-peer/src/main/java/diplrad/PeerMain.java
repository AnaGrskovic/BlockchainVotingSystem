package diplrad;

import com.google.gson.Gson;
import com.google.gson.GsonBuilder;
import diplrad.constants.LogMessages;
import diplrad.cryptography.CryptographyHelper;
import diplrad.exceptions.*;
import diplrad.queue.AzureMessageQueueClient;
import diplrad.tcp.blockchain.BlockChainTcpClientHelper;
import diplrad.http.PeerHttpHelper;
import diplrad.http.HttpSender;
import diplrad.models.blockchain.VotingBlockChainSingleton;
import diplrad.tcp.TcpServer;

import static diplrad.helpers.ExceptionHandler.handleFatalException;
import static diplrad.models.peer.PeersSingleton.ownPeer;

public class PeerMain {

    private static Gson gson = new GsonBuilder().create();

    public static void main(String[] args) {

        try {
            String tcpPortString = args[0];
            TcpServer.tcpServerPort = Integer.parseInt(tcpPortString);
        } catch (Exception e) {
            System.out.println(LogMessages.tcpServerPortArgumentFailMessage);
            System.exit(1);
        }

        try {

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

        } catch (IpException | ParseException | HttpException | TcpException | CryptographyException e) {
            handleFatalException(e);
        }

        AzureMessageQueueClient azureMessageQueueClient = new AzureMessageQueueClient(gson);
        while (true) {
            azureMessageQueueClient.receiveAndHandleQueueMessage();
        }

    }

}
