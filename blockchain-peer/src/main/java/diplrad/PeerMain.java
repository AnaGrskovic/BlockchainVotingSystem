package diplrad;

import com.google.gson.Gson;
import com.google.gson.GsonBuilder;
import diplrad.constants.Constants;
import diplrad.helpers.VoteMocker;
import diplrad.http.HttpSender;
import diplrad.models.blockchain.VotingBlockChainSingleton;
import diplrad.models.peer.Peer;
import diplrad.models.peer.PeerRequest;
import diplrad.models.peer.PeersInstance;
import diplrad.tcp.blockchain.BlockChainTcpClient;
import diplrad.tcp.TcpServer;

import java.io.IOException;
import java.util.List;

import static diplrad.helpers.IpHelper.getOwnIpAddress;

public class PeerMain {

    private static Gson gson = new GsonBuilder().create();

    public static void main(String[] args) {

        TcpServer.TcpServerThread t = new TcpServer.TcpServerThread();
        t.start();

        System.out.println("TCP server started");

        HttpSender httpSender = new HttpSender();
        PeerRequest ownPeerRequest = new PeerRequest(getOwnIpAddress().getHostAddress(), Constants.TCP_SERVER_PORT);
        Peer ownPeer = httpSender.registerPeer(ownPeerRequest);
        PeersInstance.createInstance(httpSender.getPeers(ownPeer));

        System.out.println("Registered peer");

        for (Peer peer : PeersInstance.getInstance()) {
            try {
                BlockChainTcpClient client = new BlockChainTcpClient();
                client.startConnection(peer.getIpAddress(), peer.getPort());
                client.sendBlockchainRequest(gson, ownPeer);
                client.stopConnection();
            } catch (IOException e) {
                System.out.println("TCP client encountered an error");
                System.exit(1);
            }
        }

        System.out.println("Sent blockchain requests and updated current blockchain:" + gson.toJson(VotingBlockChainSingleton.getInstance()));

        try {

            for (int i = 0; i < 5; i++) {

                Thread.sleep((long)(Math.random() * 100000));

                VoteMocker.generateRandomVotes(VotingBlockChainSingleton.getInstance());

                for (Peer peer : PeersInstance.getInstance()) {
                    try {
                        BlockChainTcpClient client = new BlockChainTcpClient();
                        client.startConnection(peer.getIpAddress(), peer.getPort());
                        client.sendBlockchain(gson);
                        client.stopConnection();
                    } catch (IOException e) {
                        System.out.println("TCP client encountered an error");
                        System.exit(1);
                    }
                }

            }

        } catch (InterruptedException e) {
            throw new RuntimeException(e);
        }

    }

}
