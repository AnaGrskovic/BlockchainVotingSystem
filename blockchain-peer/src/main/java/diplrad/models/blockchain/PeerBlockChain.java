package diplrad.models.blockchain;

import com.google.gson.annotations.Expose;
import diplrad.models.peer.PeerRequest;

public class PeerBlockChain {

    @Expose
    private PeerRequest peer;

    @Expose
    private VotingBlockChain blockChain;

    public PeerBlockChain(PeerRequest peer, VotingBlockChain blockChain) {
        this.peer = peer;
        this.blockChain = blockChain;
    }

}
