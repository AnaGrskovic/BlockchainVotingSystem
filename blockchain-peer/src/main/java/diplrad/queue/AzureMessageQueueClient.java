package diplrad.queue;

import com.azure.identity.DefaultAzureCredentialBuilder;
import com.azure.storage.queue.QueueClient;
import com.azure.storage.queue.QueueClientBuilder;
import com.azure.storage.queue.models.QueueMessageItem;
import com.google.gson.Gson;
import com.google.gson.JsonSyntaxException;
import diplrad.constants.Constants;
import diplrad.constants.LogMessages;
import diplrad.exceptions.HttpException;
import diplrad.exceptions.TcpException;
import diplrad.http.HttpSender;
import diplrad.models.vote.VoteMessage;
import diplrad.tcp.blockchain.BlockChainTcpClientHelper;
import diplrad.models.blockchain.Block;
import diplrad.models.blockchain.VotingBlockChain;
import diplrad.models.blockchain.VotingBlockChainSingleton;

import java.util.List;

import static diplrad.helpers.ExceptionHandler.handleFatalException;

public class AzureMessageQueueClient {

    private QueueClient queueClient;
    private Gson gson;

    public AzureMessageQueueClient(Gson gson) {
        this.queueClient = new QueueClientBuilder()
                .endpoint(Constants.AZURE_STORAGE_ENDPOINT)
                .queueName(Constants.AZURE_STORAGE_QUEUE)
                .credential(new DefaultAzureCredentialBuilder().build())
                .buildClient();
        this.gson = gson;
    }

    public void receiveAndHandleQueueMessage() {
        queueClient.receiveMessages(1).forEach(queueMessageItem -> {
            handleQueueMessageItem(queueMessageItem);
            queueClient.deleteMessage(queueMessageItem.getMessageId(), queueMessageItem.getPopReceipt());
        });
    }

    public void handleQueueMessageItem(QueueMessageItem queueMessageItem) {
        String message = queueMessageItem.getBody().toString();
        System.out.printf((LogMessages.queueMessageReceivedMessage) + "%n", message);
        VoteMessage voteMessage;
        try {
            voteMessage = gson.fromJson(message, VoteMessage.class);
        } catch (JsonSyntaxException e) {
            System.out.printf((LogMessages.queueMessageParseErrorMessage) + "%n", message);
            return;
        }
        if (voteMessage == null || voteMessage.getVote() == null || voteMessage.getToken() == null) {
            System.out.printf((LogMessages.queueMessageParseErrorMessage) + "%n", message);
            return;
        }

        try {
            HttpSender httpSender = new HttpSender();
            httpSender.checkToken(voteMessage.getToken());
        } catch (HttpException e) {
            System.out.println(e.getMessage());
            return;
        }

        handleQueueMessage(voteMessage.getVote());
    }

    public void handleQueueMessage(String vote) {

        if (!isVoteValid(vote)) {
            System.out.printf((LogMessages.queueMessageInvalidVoteMessage) + "%n", vote);
            return;
        }

        synchronized (VotingBlockChainSingleton.lock) {
            VotingBlockChain blockChain = VotingBlockChainSingleton.getInstance();
            Block block = new Block(vote, blockChain.getLastBlockHash());
            blockChain.mineBlock(block);
            System.out.printf((LogMessages.queueMessageVoteAddedMessage) + "%n", vote);
        }

        try {
            BlockChainTcpClientHelper.createTcpClientsAndSendBlockChains(gson);
            System.out.println(LogMessages.blockChainSentMessage);
        } catch (TcpException e) {
            System.out.println(LogMessages.blockChainSendFailMessage);
            handleFatalException(e);
        }

    }

    private boolean isVoteValid(String vote) {
        VotingBlockChain blockChain = VotingBlockChainSingleton.getInstance();
        List<String> candidates = blockChain.getCandidates();
        return candidates.contains(vote);
    }

}
