package diplrad;

import com.google.gson.Gson;
import com.google.gson.GsonBuilder;
import diplrad.constants.Constants;
import diplrad.constants.LogMessages;
import diplrad.cryptography.CryptographyHelper;
import diplrad.exceptions.*;
import diplrad.helpers.DigitalSignatureHelper;
import diplrad.http.PeerHttpHelper;
import diplrad.http.HttpSender;
import diplrad.models.blockchain.VotingBlockChainSingleton;
import diplrad.queue.AzureMessageQueueClient;
import diplrad.tcp.TcpServer;
import org.bouncycastle.jce.provider.BouncyCastleProvider;

import java.security.*;
import java.time.LocalDateTime;

import static diplrad.helpers.ExceptionHandler.handleFatalException;
import static diplrad.helpers.FileReader.readCandidatesFromFile;
import static diplrad.models.peer.PeersSingleton.ownPeer;

public class MasterMain {

    private static Gson gson = new GsonBuilder().create();

    public static void main(String[] args) {

        String privateKeyPem = "-----BEGIN RSA PRIVATE KEY-----\n" +
                "MIICWwIBAAKBgG6FsXeYvMY7Izn/xeyjt7kQdYMeKV6lsMCXRzP3j5pfS5Jcz4Sx\n" +
                "B4+dlloloDaMsxo0QBnuAPRwFWsJ4o8f9WwHngexSlkObrkws+O/LZiMBzYqG3pn\n" +
                "PRFs9iSJXLf8BxLBcFSmT4yjgwkYtYeYMMuMKK+aEZyYjFoI+DZLhdIbAgMBAAEC\n" +
                "gYAHzEcJOS2YjvOdU/6TA7oixJaF+crRcr9V11aexAjNy4t5eDLsGdF+wI+rLJxx\n" +
                "PNwmLSmYqsJGfOIF+1yQ3KBkNuExHKDsdndR924BCwbyV+MpHDkZU8rD0rk5S1F7\n" +
                "OrRSxrwWnaeQ2s1z6HzmJpMUYLGKWyh0O61uKsHbSw/7gQJBAMQbUpZopzsuITyL\n" +
                "kVBWzPHX7Xp4k4OcI8C0+YCZ9o4xF9RQHxKVPgWOf7ngLFCsv/b8RPIAdIVm2EdF\n" +
                "ln8hfz8CQQCQRupf3MMh4Vg+kUPCjIhYVG7PZz7Bvb0XmfhKmaVLUFyjqor2J1g6\n" +
                "1n2GpnvctQtfsnLPxeVo+nFb0/M8RBIlAkAOWR1qyc9qgg6GeoOwSBmInE7Qxh+s\n" +
                "4nCvOc6DfUBP2QGwVAhh+K9oAqwPsnorkOgerwhwWF4uIH80f7/qH05LAkAaFchA\n" +
                "RsC8+moi/c6beR8ZoUJbm1YcXzq17q+WhUr+X+wv5yCyupBYKvmNA8K3N8Bzr+bU\n" +
                "K/p7TXR7XsnGZqRFAkEAtMDSjm186iwF59JELE8uDV7+5Q6Pd35eTcsmV07M8mww\n" +
                "Xx41d4f2U+vgIEulKs11ibKENlvoZ+QlmHOI2bnnoQ==\n" +
                "-----END RSA PRIVATE KEY-----";
        String publicKeyPem = "-----BEGIN PUBLIC KEY-----\n" +
                "MIGeMA0GCSqGSIb3DQEBAQUAA4GMADCBiAKBgG6FsXeYvMY7Izn/xeyjt7kQdYMe\n" +
                "KV6lsMCXRzP3j5pfS5Jcz4SxB4+dlloloDaMsxo0QBnuAPRwFWsJ4o8f9WwHngex\n" +
                "SlkObrkws+O/LZiMBzYqG3pnPRFs9iSJXLf8BxLBcFSmT4yjgwkYtYeYMMuMKK+a\n" +
                "EZyYjFoI+DZLhdIbAgMBAAE=\n" +
                "-----END PUBLIC KEY-----";

        try {
            String tcpPortString = args[0];
            TcpServer.tcpServerPort = Integer.parseInt(tcpPortString);
        } catch (Exception e) {
            System.out.println(LogMessages.tcpServerPortArgumentFailMessage);
            System.exit(1);
        }

        try {

            Security.addProvider(new BouncyCastleProvider());

            VotingBlockChainSingleton.createInstance(readCandidatesFromFile());
            System.out.printf((LogMessages.createdBlockChainMessage) + "%n", gson.toJson(VotingBlockChainSingleton.getInstance()));

            HttpSender httpSender = new HttpSender();
            ownPeer = PeerHttpHelper.createOwnPeer(httpSender);
            PeerHttpHelper.getPeersInitial(httpSender, ownPeer);
            System.out.println(LogMessages.registeredOwnPeer);

            CryptographyHelper.loadCryptographyProperties();

            TcpServer.TcpServerThread tcpServerThread = new TcpServer.TcpServerThread();
            tcpServerThread.start();
            System.out.printf((LogMessages.startedTcpServer) + "%n", TcpServer.tcpServerPort);

            AzureMessageQueueClient azureMessageQueueClient = new AzureMessageQueueClient(gson);
            while (LocalDateTime.now().isBefore(Constants.VOTING_END_DATE_TIME)) {
                azureMessageQueueClient.receiveAndHandleQueueMessage();
            }
            
            var finalBlockChain = VotingBlockChainSingleton.getInstance();
            var signedFinalBlockChain = DigitalSignatureHelper.signBlockChain(finalBlockChain, privateKeyPem, gson);
            httpSender.createBlockChain(finalBlockChain, signedFinalBlockChain, publicKeyPem);

        } catch (InvalidFileException | ReadFromFileException | IpException | ParseException | HttpException | CryptographyException e) {
            handleFatalException(e);
        }

    }

}
