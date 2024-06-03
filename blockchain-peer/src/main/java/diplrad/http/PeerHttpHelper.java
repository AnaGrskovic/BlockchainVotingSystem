package diplrad.http;

import diplrad.exceptions.HttpException;
import diplrad.exceptions.IpException;
import diplrad.exceptions.ParseException;
import diplrad.models.peer.Peer;
import diplrad.models.peer.PeerRequest;
import diplrad.models.peer.PeersSingleton;
import diplrad.tcp.TcpServer;

import static diplrad.helpers.IpHelper.getOwnIpAddress;
import static diplrad.models.peer.PeersSingleton.ownPeer;

public class PeerHttpHelper {

    public static Peer createOwnPeer(HttpSender httpSender) throws IpException, HttpException, ParseException {
        PeerRequest ownPeerRequest = new PeerRequest(getOwnIpAddress().getHostAddress(), TcpServer.tcpServerPort);
        return httpSender.createPeer(ownPeerRequest);
    }

    public static void getPeersInitial(HttpSender httpSender, Peer ownPeer) throws IpException, HttpException, ParseException {
        PeersSingleton.createInstance(httpSender.getPeers(ownPeer));
    }

    public static void deleteOwnPeer(HttpSender httpSender, Peer ownPeer) throws HttpException {
        if (ownPeer != null) {
            httpSender.deletePeer(ownPeer.getId());
        }
    }

    public static void tryCreateHttpClientAndDeleteOwnPeer() {
        try {
            if (ownPeer != null) {
                HttpSender httpSender = new HttpSender();
                deleteOwnPeer(httpSender, ownPeer);
            }
        } catch (HttpException e) {
            System.out.println(e.getMessage());
        }
    }

}
