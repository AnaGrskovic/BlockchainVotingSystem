package diplrad.helpers;

import diplrad.constants.LogMessages;
import diplrad.tcp.TcpServer;

public class ArgsProcessHelper {

    public static void initializeTcpServer(String[] args) {
        try {
            String tcpPortString = args[0];
            TcpServer.tcpServerPort = Integer.parseInt(tcpPortString);
        } catch (Exception e) {
            System.out.println(LogMessages.tcpServerPortArgumentFailMessage);
            System.exit(1);
        }
    }

    public static String initializePrivateKeyPem(String[] args) {
        try {
            String privateKeyPemPath = args[1];
            return FileReader.readFile(privateKeyPemPath);
        } catch (Exception e) {
            System.out.println(LogMessages.privateKeyPemArgumentFailMessage);
            System.exit(1);
        }
        return null;
    }

}
