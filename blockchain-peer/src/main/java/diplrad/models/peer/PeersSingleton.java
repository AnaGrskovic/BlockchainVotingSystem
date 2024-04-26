package diplrad.models.peer;

import diplrad.constants.LogMessages;

import java.util.ArrayList;
import java.util.List;

public class PeersSingleton {

    public static Peer ownPeer;

    private static List<Peer> INSTANCE = new ArrayList<>();

    public synchronized static void createInstance(List<Peer> peers) {
        if (!INSTANCE.isEmpty()) {
            System.out.println(LogMessages.listOfPeersAlreadyCreatedMessage);
        }
        INSTANCE = new ArrayList<>(peers);
    }

    public static List<Peer> getInstance() {
        return INSTANCE;
    }

    public synchronized static void addPeer(Peer peer) {
        INSTANCE.add(peer);
    }
    public synchronized static void removePeer(Peer peer) {
        INSTANCE.remove(peer);
    }

}
