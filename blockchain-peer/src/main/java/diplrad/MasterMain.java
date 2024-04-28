package diplrad;

import com.google.gson.Gson;
import com.google.gson.GsonBuilder;
import diplrad.constants.LogMessages;
import diplrad.exceptions.*;
import diplrad.http.PeerHttpHelper;
import diplrad.http.HttpSender;
import diplrad.models.blockchain.VotingBlockChainSingleton;
import diplrad.queue.AzureMessageQueueClient;
import diplrad.tcp.TcpServer;

import static diplrad.helpers.ExceptionHandler.handleFatalException;
import static diplrad.helpers.FileReader.readCandidatesFromFile;
import static diplrad.models.peer.PeersSingleton.ownPeer;

public class MasterMain {

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

            VotingBlockChainSingleton.createInstance(readCandidatesFromFile());
            System.out.printf((LogMessages.createdBlockChainMessage) + "%n", gson.toJson(VotingBlockChainSingleton.getInstance()));

            HttpSender httpSender = new HttpSender();
            ownPeer = PeerHttpHelper.createOwnPeer(httpSender);
            PeerHttpHelper.getPeersInitial(httpSender, ownPeer);
            System.out.println(LogMessages.registeredOwnPeer);

            TcpServer.TcpServerThread tcpServerThread = new TcpServer.TcpServerThread();
            tcpServerThread.start();
            System.out.printf((LogMessages.startedTcpServer) + "%n", TcpServer.tcpServerPort);


        } catch (InvalidFileException | ReadFromFileException | IpException | ParseException | HttpException e) {
            handleFatalException(e);
        }

        AzureMessageQueueClient azureMessageQueueClient = new AzureMessageQueueClient(gson);
        while (true) {
            azureMessageQueueClient.receiveAndHandleQueueMessage();
        }

    }

}
