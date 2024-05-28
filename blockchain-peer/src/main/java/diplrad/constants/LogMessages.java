package diplrad.constants;

public class LogMessages {

    public static final String createdBlockChainMessage = "Created a block chain: %s.";
    public static final String startedTcpServer = "TCP server is started on port %s.";
    public static final String registeredOwnPeer = "Own peer is registered.";
    public static final String receivedInitialBlockChain = "Sent block chain requests and set initial block chain: %s.";
    public static final String votingBlockChainAlreadyCreatedMessage = "Voting block chain is already created";
    public static final String listOfPeersAlreadyCreatedMessage = "List of peers is already created";
    public static final String invalidBlockChainReceivedMessage = "Received block chain is invalid.";
    public static final String overrideBlockChainMessage = "Overridden current block chain with the received instance.";
    public static final String overrideBlockChainDiscardLastBlockMessage = "Overridden current block chain with the received instance. Last block was discarded because it was added after the last block of the received instance.";
    public static final String incompatibleBlockChainMessage = "Received block chain is incompatible with the current instance.";
    public static final String incompatibleBlockChainLastBlockMessage = "Received block chain's last block was added after current instance's last block.";
    public static final String receivedBlockChainTooSmallMessage = "Received block chain is too small.";
    public static final String receivedBlockChainTooBigMessage = "Received block chain is too big.";
    public static final String receivedTcpMessage = "Received TCP message: %s.";
    public static final String queueMessageReceivedMessage = "Received queue message: %s.";
    public static final String queueMessageParseErrorMessage = "The received queue message: %s cannot be parsed. Ignoring it.";
    public static final String queueMessageInvalidVoteMessage = "The received queue message: %s contains a vote for %s, which is invalid, because it does not represent a valid candidate. Ignoring it";
    public static final String queueMessageVoteAddedMessage = "The received queue message: %s contains a vote for %s. Vote added to block chain.";
    public static final String generatedVoteAddedMessage = "Randomly generated a vote for %s. Vote added to block chain.";
    public static final String blockChainSentMessage = "Sent block chain to peers.";
    public static final String blockChainSendFailMessage = "Unable to send block chain to peers.";
    public static final String tcpServerPortArgumentFailMessage = "The first argument must be the TCP port.";
    public static final String privateKeyPemArgumentFailMessage = "The second argument must be the path to the private key PEM file.";
    public static final String ignoreMessageThatCannotBeDecryptedMessage = "The received message cannot be decrypted. Ignoring it because it could be sent by an attacker.";

}
