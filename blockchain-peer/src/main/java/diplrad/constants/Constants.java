package diplrad.constants;

public class Constants {
    public static final int DIFFICULTY = 2;
    public static final String GENESIS_BLOCK_PREVIOUS_HASH = "0";
    public static final String GENESIS_BLOCK_DATA = "The is the Genesis Block.";
    public static final int INITIAL_BLOCK_NONCE = 0;
    public static final String CANDIDATES_FILE_PATH = "./src/main/resources/candidates.txt";
    public static final String VOTERS_FILE_PATH = "./src/main/resources/voters.txt";
    public static final String TCP_CONNECT = "CONNECT";
    public static final String TCP_DISCONNECT = "DISCONNECT";
    public static final String ENCRYPTION_ALGORITHM = "AES/CBC/PKCS5Padding";
    public static final String AZURE_STORAGE_ACCOUNT = "votingblockchainstorage";
    public static final String AZURE_STORAGE_ENDPOINT = "https://" + AZURE_STORAGE_ACCOUNT + ".queue.core.windows.net/";
    public static final String AZURE_STORAGE_QUEUE = "vote-queue";
    public static final String CENTRAL_PEER_COORDINATOR_BASE_URL = "https://localhost:7063/";
    public static final String CENTRAL_PEER_COORDINATOR_PEERS_ENDPOINT = "api/peers/";
    public static final String AUTHORIZATION_PROVIDER_BASE_URL = "https://localhost:44378/";
    public static final String AUTHORIZATION_PROVIDER_CHECK_TOKEN_ENDPOINT = "api/authorization/check-token";
}
