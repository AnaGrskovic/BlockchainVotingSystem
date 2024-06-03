package diplrad.cryptography;

import diplrad.constants.Constants;

import java.security.MessageDigest;
import java.security.NoSuchAlgorithmException;

import static java.nio.charset.StandardCharsets.UTF_8;

public class HashHelper {

    public static String hashWithSha256(String input)
    {
        try {
            MessageDigest digest = MessageDigest.getInstance(Constants.HASH_ALGORITHM);
            byte[] bytes = digest.digest(input.getBytes(UTF_8));
            StringBuilder buffer = new StringBuilder();
            for (byte b : bytes) {
                buffer.append(String.format("%02x", b));
            }
            return buffer.toString();
        }
        catch (NoSuchAlgorithmException e) {
            throw new RuntimeException(e);
        }
    }
    
}
