package diplrad.constants;

import java.time.LocalDate;
import java.time.LocalDateTime;
import java.time.LocalTime;

public class Constants {
    public static final int DIFFICULTY = 2;
    public static final String GENESIS_BLOCK_PREVIOUS_HASH = "0";
    public static final String GENESIS_BLOCK_DATA = "The is the Genesis Block.";
    public static final int INITIAL_BLOCK_NONCE = 0;
    public static final LocalDateTime VOTING_START_DATE_TIME = LocalDateTime.of(LocalDate.of(2024, 7, 1), LocalTime.of(7, 0));
    public static final LocalDateTime VOTING_END_DATE_TIME = LocalDateTime.of(LocalDate.of(2024, 7, 1), LocalTime.of(19, 0));
    public static final LocalDateTime STABILIZE_END_DATE_TIME = LocalDateTime.of(LocalDate.of(2024, 7, 1), LocalTime.of(19, 55));
    public static final String CANDIDATES_FILE_PATH = "./src/main/resources/candidates.txt";
    public static final String VOTERS_FILE_PATH = "./src/main/resources/voters.txt";
    public static final String TCP_CONNECT = "CONNECT";
    public static final String TCP_DISCONNECT = "DISCONNECT";
    public static final String HASH_ALGORITHM = "SHA-256";
    public static final String CRYPTOGRAPHY_ALGORITHM = "AES";
    public static final String CRYPTOGRAPHY_ALGORITHM_DETAILS = "AES/CBC/PKCS5Padding";
    public static final String PROPERTIES_FILE_NAME = "application.properties";
    public static final String AES_PASSWORD_PROPERTY = "cryptography.aes.password";
    public static final String AES_INITIALIZATION_VECTOR_PROPERTY = "cryptography.aes.initialization-vector";
    public static final String AZURE_STORAGE_ACCOUNT = "votingblockchainstorage";
    public static final String AZURE_STORAGE_ENDPOINT = "https://" + AZURE_STORAGE_ACCOUNT + ".queue.core.windows.net/";
    public static final String AZURE_STORAGE_QUEUE = "vote-queue";
    public static final String CENTRAL_PEER_COORDINATOR_BASE_URL = "https://localhost:7063/";
    public static final String CENTRAL_PEER_COORDINATOR_PEERS_ENDPOINT = "api/peers/";
    public static final String AUTHORIZATION_PROVIDER_BASE_URL = "https://localhost:44378/";
    public static final String AUTHORIZATION_PROVIDER_CHECK_TOKEN_REQUESTED_ENDPOINT = "api/authorization/check-token-voted";
    public static final String VOTING_API_BASE_URL = "https://localhost:44328/";
    public static final String VOTING_API_CREATE_BLOCKCHAIN_ENDPOINT = "api/block-chains";
}
