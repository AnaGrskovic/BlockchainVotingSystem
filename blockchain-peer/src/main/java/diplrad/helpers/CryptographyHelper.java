package diplrad.helpers;

import javax.crypto.BadPaddingException;
import javax.crypto.Cipher;
import javax.crypto.IllegalBlockSizeException;
import javax.crypto.NoSuchPaddingException;
import javax.crypto.spec.IvParameterSpec;
import javax.crypto.spec.SecretKeySpec;
import java.io.*;
import java.nio.file.Files;
import java.nio.file.Path;
import java.security.InvalidAlgorithmParameterException;
import java.security.InvalidKeyException;
import java.security.MessageDigest;
import java.security.NoSuchAlgorithmException;
import java.security.spec.AlgorithmParameterSpec;
import java.util.Arrays;

import static java.nio.charset.StandardCharsets.UTF_8;

public class CryptographyHelper {

    public static String hashWithSha256(String input)
    {
        try {
            MessageDigest digest = MessageDigest.getInstance("SHA-256");
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

    public static String encryptAes(String password, String initializationVector, String input) throws IOException, NoSuchPaddingException, NoSuchAlgorithmException, InvalidAlgorithmParameterException, InvalidKeyException, IllegalBlockSizeException, BadPaddingException {

        InputStream inputStream = new ByteArrayInputStream(input.getBytes());
        OutputStream outputStream = new ByteArrayOutputStream();

        Cipher cipher = Cipher.getInstance("AES/CBC/PKCS5Padding");
        SecretKeySpec secretKeySpec = new SecretKeySpec(HexByteConversionHelper.hexToByte(password), "AES");
        AlgorithmParameterSpec algorithmParameterSpec = new IvParameterSpec(HexByteConversionHelper.hexToByte(initializationVector));
        cipher.init(Cipher.ENCRYPT_MODE, secretKeySpec, algorithmParameterSpec);
        byte[] buffer = new byte[4096];
        int readIntoBuffer = inputStream.read(buffer);
        while (readIntoBuffer > 0) {
            if(readIntoBuffer < 4096) buffer = Arrays.copyOf(buffer, readIntoBuffer);
            outputStream.write(cipher.update(buffer));
            buffer = new byte[4096];
            readIntoBuffer = inputStream.read(buffer);
        }
        outputStream.write(cipher.doFinal());

        return outputStream.toString();

    }

    public static String decryptAes(String password, String initializationVector, String input) throws IOException, NoSuchPaddingException, NoSuchAlgorithmException, InvalidAlgorithmParameterException, InvalidKeyException, IllegalBlockSizeException, BadPaddingException {

        InputStream inputStream = new ByteArrayInputStream(input.getBytes());
        OutputStream outputStream = new ByteArrayOutputStream();

        Cipher cipher = Cipher.getInstance("AES/CBC/PKCS5Padding");
        SecretKeySpec secretKeySpec = new SecretKeySpec(HexByteConversionHelper.hexToByte(password), "AES");
        AlgorithmParameterSpec algorithmParameterSpec = new IvParameterSpec(HexByteConversionHelper.hexToByte(initializationVector));
        cipher.init(Cipher.DECRYPT_MODE, secretKeySpec, algorithmParameterSpec);

        byte[] buffer = new byte[4096];
        int readIntoBuffer = inputStream.read(buffer);
        while (readIntoBuffer > 0) {
            if(readIntoBuffer < 4096) buffer = Arrays.copyOf(buffer, readIntoBuffer);
            outputStream.write(cipher.update(buffer));
            buffer = new byte[4096];
            readIntoBuffer = inputStream.read(buffer);
        }
        outputStream.write(cipher.doFinal());

        return outputStream.toString();

    }

}
