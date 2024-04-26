package diplrad.models.blockchain;

import diplrad.constants.LogMessages;

import java.util.List;

public class VotingBlockChainSingleton {

    public final static Object lock = new Object();

    private static VotingBlockChain INSTANCE;

    public synchronized static void createInstance(List<String> candidates) {
        if (INSTANCE != null) {
            System.out.println(LogMessages.votingBlockChainAlreadyCreatedMessage);
        }
        INSTANCE = new VotingBlockChain(candidates);
    }

    public static VotingBlockChain getInstance() {
        return INSTANCE;
    }

    public synchronized static void setInstance(VotingBlockChain instance) {
        INSTANCE = instance;
    }

}
