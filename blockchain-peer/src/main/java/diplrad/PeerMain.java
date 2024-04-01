package diplrad;

import diplrad.constants.Constants;
import diplrad.http.HttpSender;
import diplrad.models.peer.Peer;
import diplrad.models.peer.PeerRequest;
import diplrad.tcp.blockchain.BlockChainTcpClient;
import diplrad.tcp.TcpServer;

import java.io.IOException;
import java.util.List;

import static diplrad.helpers.IpHelper.getOwnIpAddress;

public class PeerMain {

    public static void main(String[] args) {

        TcpServer.TcpServerThread t = new TcpServer.TcpServerThread();
        t.start();

        HttpSender httpSender = new HttpSender();
        PeerRequest ownPeerRequest = new PeerRequest(getOwnIpAddress(), Constants.TCP_SERVER_PORT);
        Peer ownPeer = httpSender.registerPeer(ownPeerRequest);
        List<Peer> peers = httpSender.getPeers();

        for (Peer peer : peers) {

            try {
                BlockChainTcpClient client = new BlockChainTcpClient();
                client.startConnection(peer.getIpAddress().getHostAddress(), peer.getPort());
                client.sendBlockchainRequest();
                client.stopConnection();
            } catch (IOException e) {
                System.out.println("Unable to start TCP client.");
                System.exit(1);
            }

        }

    }

}
