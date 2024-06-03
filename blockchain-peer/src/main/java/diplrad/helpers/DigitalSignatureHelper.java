package diplrad.helpers;

import com.google.gson.FieldNamingPolicy;
import com.google.gson.Gson;
import com.google.gson.GsonBuilder;
import diplrad.constants.ErrorMessages;
import diplrad.exceptions.CryptographyException;
import diplrad.exceptions.IpException;
import diplrad.models.blockchain.BlockChain;
import org.bouncycastle.util.io.pem.PemObject;
import org.bouncycastle.util.io.pem.PemReader;

import java.io.StringReader;
import java.security.KeyFactory;
import java.security.PrivateKey;
import java.security.Signature;
import java.security.spec.PKCS8EncodedKeySpec;
import java.util.Base64;

public class DigitalSignatureHelper {

    public static String signBlockChain(BlockChain blockChain, String privateKeyPem) throws CryptographyException {
        try {
            Gson pascalCaseGson = new GsonBuilder().setFieldNamingPolicy(FieldNamingPolicy.UPPER_CAMEL_CASE).create();
            var serializedBlockChain = pascalCaseGson.toJson(blockChain);
            PrivateKey privateKey = getPrivateKeyFromPem(privateKeyPem);
            return signMessage(privateKey, serializedBlockChain);
        } catch (Exception e) {
            throw new CryptographyException(ErrorMessages.unsuccessfulDigitalSignatureErrorMessage);
        }
    }

    private static PrivateKey getPrivateKeyFromPem(String pem) throws Exception {
        PemReader pemReader = new PemReader(new StringReader(pem));
        PemObject pemObject = pemReader.readPemObject();
        byte[] content = pemObject.getContent();
        PKCS8EncodedKeySpec keySpec = new PKCS8EncodedKeySpec(content);
        KeyFactory keyFactory = KeyFactory.getInstance("RSA");
        return keyFactory.generatePrivate(keySpec);
    }

    private static String signMessage(PrivateKey privateKey, String message) throws Exception {
        Signature sign = Signature.getInstance("SHA256withRSA");
        sign.initSign(privateKey);
        sign.update(message.getBytes());
        byte[] signature = sign.sign();
        return Base64.getEncoder().encodeToString(signature);
    }

}