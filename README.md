# BlockchainVotingSystem
This is a system for secure online voting based on the blockchain technology. It provides a secure way of voting in any kind of elections, which prevents falsification of votes and minimizes the amount of human work required to conduct elections.

## How it works
The system consists of 5 components:
1. **Blockchain peer** - a node in the blockchain network that stores the blockchain and validates new blocks
2. **Central peer coordinator** - a server that manages new peers entering and exiting the network
3. **Authorization provider** - a server responsible for authenticating and authorizing the users
4. **Voting server** - a server that manages the voting process
5. **Client** - a web application that allows users to vote

## Local setup
To run the system locally, you must first install the following dependencies:
- .NET 8 or higher
- Java 17 or higher
- Maven 3.9.4 or higher
- Angular 17.3.9
- Microsoft SQL server 2022 
- optionally some IDEs like Visual Studio 2022, Visual Studio Code, IntelliJ IDEA, etc.

[Create an Azure storage account](https://learn.microsoft.com/en-us/azure/storage/common/storage-account-create?tabs=azure-portal), log in locally and [create an Azure storage queue](https://learn.microsoft.com/en-us/azure/storage/queues/storage-quickstart-queues-portal).

After installing the dependencies, you should edit settings of the following components:
1. **Blockchain peer** - `diplrad.constants.Constants.java`
2. **Central peer coordinator** - `CentralPeerCoordinator.API.appsettings.json`
3. **Authorization provider** - `DummyAuthorizationProvider.API.appsettings.json`
4. **Voting server** - `VotingApp.API.appsettings.json`

Make sure that the settings of the components are matching:
* blockchain timestamps need to be in sync across all components 
  * BlockChainCalculationStartTime must be before BlockChainCalculationEndTime, which must be before BlockChainStabilizationEndTime
  * BlockChainCalculationEndTime on the Blockchain peers should be a few minutes (e.g. 5) after BlockChainCalculationEndTime on the Voting server, but still before BlockChainStabilizationEndTime
* list of candidates must be the same in the Voting server and the Client
* list of peer public keys in the Voting server needs to match the public keys of the blockchain peers
* Azure storage settings and endpoint settings must match across all components

Then, you must run the migrations in the following three components using EntityFramework Core:
1. **Central peer coordinator**
2. **Authorization provider**
3. **Voting server**

[Add certificates of **Central peer coordinator**, **Authorization provider** and **Voting server** to the trusted root certification authorities on the client side.](https://help.highbond.com/helpdocs/analytics-exchange/5/Content/ax_administration/t_importing_certificates_into_the_java_cacerts_file.html)

Finally, you can run the components in the following order:
1. **Authorization provider**
2. **Voting server**
3. **Central peer coordinator**
4. **Blockchain peer**
5. **Client**

**Authorization provider**, **Voting server**, **Central peer coordinator** and **Client** can be run regularly, using the IDE or the command line. 
For **Blockchain peer**, you must provide public and private RSA key as well as the port. Some sample RSA keys are provided in the `blockchain-peer/src/main/resources` folder.
Also, for the application to work as expected, you should run multiple instances of Blockchain peer, at least 3. 
One instance of the Blockchain peer should be run as the master peer (MasterMain.java), and that one must be run before the other ones, while the other instances should be run as regular peers (PeerMain.java).
Here are given examples of how to run three Blockchain peers using the command line:
```
mvn package
mvn compile
mvn exec:java -Dexec.mainClass=diplrad.MasterMain -Dexec.args="5555 ./src/main/resources/keys/private1.txt ./src/main/resources/keys/public1.txt"
mvn exec:java -Dexec.mainClass=diplrad.PeerMain -Dexec.args="5556 ./src/main/resources/keys/private2.txt ./src/main/resources/keys/public2.txt"
mvn exec:java -Dexec.mainClass=diplrad.PeerMain -Dexec.args="5557 ./src/main/resources/keys/private3.txt ./src/main/resources/keys/public3.txt"
```

## Flow
Applications should be started before the scheduled beginning of the voting process (can be setup in the settings, as mentioned above).
Before the beginning of the voting process, voting will be enabled and on the client side, the user will be able to see the message indicating so.
Once the voting process begins, users can login using their personal identification number and vote for the candidate they want. They can also see the approximate number of votes so far.
After the voting process ends, the voting will be disabled and the user will not be able to vote anymore. Then follows a short stabilization period, during which the peers send their blockchains to the server and the counting process begins. After the stabilization period ends, the user will be able to see the results of the voting process.


