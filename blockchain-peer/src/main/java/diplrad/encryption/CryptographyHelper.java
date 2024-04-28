package diplrad.encryption;

import diplrad.constants.Constants;
import diplrad.constants.ErrorMessages;
import diplrad.exceptions.CryptographyException;

import javax.crypto.BadPaddingException;
import javax.crypto.Cipher;
import javax.crypto.IllegalBlockSizeException;
import javax.crypto.NoSuchPaddingException;
import javax.crypto.spec.IvParameterSpec;
import javax.crypto.spec.SecretKeySpec;
import java.io.FileInputStream;
import java.io.IOException;
import java.security.InvalidAlgorithmParameterException;
import java.security.InvalidKeyException;
import java.security.NoSuchAlgorithmException;
import java.security.spec.AlgorithmParameterSpec;
import java.util.Base64;
import java.util.Properties;

public class CryptographyHelper {

    private static String password;
    private static String initializationVector;

    public static void loadCryptographyProperties() throws CryptographyException {
        try {
            String rootPath = Thread.currentThread().getContextClassLoader().getResource("").getPath();
            String appConfigPath = rootPath + "application.properties";
            Properties appProps = new Properties();
            appProps.load(new FileInputStream(appConfigPath));
            password = appProps.getProperty("cryptography.aes.password");
            initializationVector = appProps.getProperty("cryptography.aes.initialization-vector");
        } catch (IOException e) {
            throw new CryptographyException(ErrorMessages.cryptographyPropertiesNotLoadedErrorMessage);
        }
    }

    public static String encrypt(String input) throws CryptographyException {
        checkCryptographyProperties();
        try {
            return encrypt(password, initializationVector, input);
        } catch (NoSuchPaddingException | NoSuchAlgorithmException | InvalidAlgorithmParameterException | InvalidKeyException | BadPaddingException | IllegalBlockSizeException e) {
            throw new CryptographyException(ErrorMessages.unsuccessfulEncryptionErrorMessage);
        }
    }

    public static String decrypt(String input) throws CryptographyException {
        checkCryptographyProperties();
        try {
            return decrypt(password, initializationVector, input);
        } catch (NoSuchPaddingException | NoSuchAlgorithmException | InvalidAlgorithmParameterException | InvalidKeyException | BadPaddingException | IllegalBlockSizeException e) {
            throw new CryptographyException(ErrorMessages.unsuccessfulDecryptionErrorMessage);
        }
    }

    private static void checkCryptographyProperties() throws CryptographyException {
        if (password == null || initializationVector == null) {
            throw new CryptographyException(ErrorMessages.cryptographyPropertiesNotLoadedErrorMessage);
        }
    }

    private static String encrypt(String password, String initializationVector, String input) throws NoSuchPaddingException, NoSuchAlgorithmException, InvalidAlgorithmParameterException, InvalidKeyException, BadPaddingException, IllegalBlockSizeException {
        SecretKeySpec secretKeySpec = new SecretKeySpec(HexByteConversionHelper.hexToByte(password), "AES");
        AlgorithmParameterSpec algorithmParameterSpec = new IvParameterSpec(HexByteConversionHelper.hexToByte(initializationVector));
        Cipher cipher = Cipher.getInstance(Constants.ENCRYPTION_ALGORITHM);
        cipher.init(Cipher.ENCRYPT_MODE, secretKeySpec, algorithmParameterSpec);
        byte[] cipherText = cipher.doFinal(input.getBytes());
        return Base64.getEncoder().encodeToString(cipherText);
    }

    private static String decrypt(String password, String initializationVector, String cipherText) throws NoSuchPaddingException, NoSuchAlgorithmException, InvalidAlgorithmParameterException, InvalidKeyException, BadPaddingException, IllegalBlockSizeException {
        SecretKeySpec secretKeySpec = new SecretKeySpec(HexByteConversionHelper.hexToByte(password), "AES");
        AlgorithmParameterSpec algorithmParameterSpec = new IvParameterSpec(HexByteConversionHelper.hexToByte(initializationVector));
        Cipher cipher = Cipher.getInstance(Constants.ENCRYPTION_ALGORITHM);
        cipher.init(Cipher.DECRYPT_MODE, secretKeySpec, algorithmParameterSpec);
        byte[] plainText = cipher.doFinal(Base64.getDecoder().decode(cipherText));
        return new String(plainText);
    }

}
