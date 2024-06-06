package diplrad.tcp.blockchain;

import com.google.gson.Gson;
import diplrad.constants.Constants;
import diplrad.constants.ErrorMessages;
import diplrad.constants.LogMessages;
import diplrad.constants.ResponseMessages;
import diplrad.cryptography.CryptographyHelper;
import diplrad.exceptions.CryptographyException;
import diplrad.exceptions.TcpException;
import diplrad.models.blockchain.Block;
import diplrad.models.blockchain.VotingBlockChain;
import diplrad.models.blockchain.VotingBlockChainSingleton;
import diplrad.models.peer.Peer;
import diplrad.models.peer.PeersSingleton;
import diplrad.queue.AzureMessageQueueClient;
import diplrad.tcp.ITcpMessageObserver;

public class BlockChainTcpMessageObserver implements ITcpMessageObserver {

    private final Gson gson;

    public BlockChainTcpMessageObserver(Gson gson) {
        this.gson = gson;
    }

    @Override
    public String messageReceived(String encryptedMessage) throws TcpException {

        String message = null;
        try {
            message = CryptographyHelper.decrypt(encryptedMessage);
        } catch (CryptographyException e) {
            System.out.println(LogMessages.ignoreMessageThatCannotBeDecryptedMessage);
            return ResponseMessages.ignoreMessage;
        }

        System.out.printf((LogMessages.receivedTcpMessage) + "%n", message);

        String[] messageParts = message.split(" ");
        if (messageParts.length == 2) {
            Peer peer = gson.fromJson(messageParts[1], Peer.class);
            if (peer != null && peer.getId() != null) {
                if (messageParts[0].equals(Constants.TCP_CONNECT)) {
                    return connectMessageReceived(peer, gson);
                } else if (messageParts[0].equals(Constants.TCP_DISCONNECT)) {
                    return disconnectMessageReceived(peer);
                }
            }
        }

        VotingBlockChain blockchain = gson.fromJson(message, VotingBlockChain.class);
        if (blockchain != null && blockchain.getBlock(0) != null) {
            return blockChainMessageReceived(blockchain);
        }

        throw new TcpException(ErrorMessages.invalidTcpMessageReceivedErrorMessage);

    }

    public String connectMessageReceived(Peer peer, Gson gson) {
        PeersSingleton.addPeer(peer);
        return gson.toJson(VotingBlockChainSingleton.getInstance());
    }

    public String disconnectMessageReceived(Peer peer) {
        PeersSingleton.removePeer(peer);
        return ResponseMessages.okMessage;
    }

    public String blockChainMessageReceived(VotingBlockChain incomingBlockchain) {

        // we are overriding our current instance with the received one if it is valid
        // and contains exactly one block more than our current instance
        // or if it is the same length as our current instance, but the last block was added before the last block of our current instance

        if (!incomingBlockchain.validate()) {
            System.out.println(LogMessages.invalidBlockChainReceivedMessage);
            return ResponseMessages.invalidBlockChainReceivedMessage;
        }

        synchronized (VotingBlockChainSingleton.lock) {

            int currentBlockChainSize;
            VotingBlockChain currentBlockChain = VotingBlockChainSingleton.getInstance();
            if (currentBlockChain == null) {
                currentBlockChainSize = 0;
            } else {
                currentBlockChainSize = currentBlockChain.size();
            }
            int incomingBlockChainSize = incomingBlockchain.size();

            if (currentBlockChainSize == 0 || currentBlockChainSize == 1) {
                VotingBlockChainSingleton.setInstance(incomingBlockchain);
                System.out.println(LogMessages.overrideBlockChainMessage);
                return ResponseMessages.okMessage;
            }

            if (incomingBlockChainSize == currentBlockChainSize + 1) {
                if (!incomingBlockchain.validateAgainstCurrent(currentBlockChain, currentBlockChainSize)) {
                    System.out.println(LogMessages.incompatibleBlockChainMessage);
                    return ResponseMessages.incompatibleBlockChainMessage;
                }
                VotingBlockChainSingleton.setInstance(incomingBlockchain);
                System.out.println(LogMessages.overrideBlockChainMessage);
                return ResponseMessages.okMessage;
            } else if (incomingBlockChainSize == currentBlockChainSize) {
                if (!incomingBlockchain.validateAgainstCurrent(currentBlockChain, currentBlockChainSize - 1)){
                    System.out.println(LogMessages.incompatibleBlockChainMessage);
                    return ResponseMessages.incompatibleBlockChainMessage;
                }
                if (incomingBlockchain.getLastBlockTimeStamp() > currentBlockChain.getLastBlockTimeStamp()) {
                    System.out.println(LogMessages.incompatibleBlockChainLastBlockMessage);
                    return ResponseMessages.incompatibleBlockChainLastBlockMessage;
                }
                var currentLastBlock = currentBlockChain.getLastBlock();
                VotingBlockChainSingleton.setInstance(incomingBlockchain);
                recreateLastBlock(currentLastBlock);
                System.out.println(LogMessages.overrideBlockChainDiscardLastBlockMessage);
                return ResponseMessages.okMessage;
            } else if (incomingBlockChainSize < currentBlockChainSize) {
                System.out.println(LogMessages.receivedBlockChainTooSmallMessage);
                return ResponseMessages.receivedBlockChainTooSmallMessage;
            } else {
                System.out.println(LogMessages.receivedBlockChainTooBigMessage);
                return ResponseMessages.receivedBlockChainTooBigMessage;
            }

        }

    }

    public void recreateLastBlock(Block currentLastBlock){
        AzureMessageQueueClient azureMessageQueueClient = new AzureMessageQueueClient(gson);
        azureMessageQueueClient.handleQueueMessage(currentLastBlock.getData());
    }

}
