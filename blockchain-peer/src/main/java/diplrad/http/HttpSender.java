package diplrad.http;

import com.google.gson.Gson;
import com.google.gson.GsonBuilder;
import diplrad.constants.Constants;
import diplrad.constants.ErrorMessages;
import diplrad.constants.LogMessages;
import diplrad.exceptions.HttpException;
import diplrad.exceptions.ParseException;
import diplrad.helpers.ListSerializationHelper;
import diplrad.models.peer.Peer;
import diplrad.models.peer.PeerRequest;

import java.io.IOException;
import java.net.URI;
import java.net.URISyntaxException;
import java.net.http.HttpClient;
import java.net.http.HttpRequest;
import java.net.http.HttpResponse;
import java.util.List;
import java.util.UUID;

public class HttpSender {

    private final Gson gson;
    private final HttpClient client;

    public HttpSender() {
        this.gson = new GsonBuilder().setPrettyPrinting().create();
        this.client = HttpClient.newBuilder().build();
    }

    public Peer createPeer(PeerRequest peerRequest) throws ParseException, HttpException {
        try {
            String json = gson.toJson(peerRequest);
            if (json == null) {
                throw new ParseException(ErrorMessages.parsePeerRequestErrorMessage);
            }
            byte[] bytes = json.getBytes();

            HttpRequest request = HttpRequest.newBuilder()
                    .uri(new URI(Constants.CENTRAL_PEER_COORDINATOR_BASE_URL + Constants.CENTRAL_PEER_COORDINATOR_PEERS_ENDPOINT))
                    .headers("Content-Type", "application/json")
                    .POST(HttpRequest.BodyPublishers.ofByteArray(bytes))
                    .build();

            HttpResponse<String> response = client.send(request, HttpResponse.BodyHandlers.ofString());

            String responseBody = response.body();
            int responseStatusCode = response.statusCode();
            if (responseStatusCode != 200) {
                throw new HttpException(ErrorMessages.unsuccessfulHttpRequestErrorMessage);
            }

            Peer peer = gson.fromJson(responseBody, Peer.class);
            if (peer == null) {
                throw new ParseException(ErrorMessages.parsePeerErrorMessage);
            }

            return peer;
        } catch (URISyntaxException e) {
            throw new HttpException(ErrorMessages.incorrectUrlErrorMessage);
        } catch (IOException | InterruptedException e) {
            throw new HttpException(String.format(ErrorMessages.sendHttpRequestErrorMessage, Constants.CENTRAL_PEER_COORDINATOR_BASE_URL + Constants.CENTRAL_PEER_COORDINATOR_PEERS_ENDPOINT));
        }
    }

    public void deletePeer(UUID peerId) throws HttpException {
        try {
            HttpRequest request = HttpRequest.newBuilder()
                    .uri(new URI(Constants.CENTRAL_PEER_COORDINATOR_BASE_URL + Constants.CENTRAL_PEER_COORDINATOR_PEERS_ENDPOINT))
                    .headers("Content-Type", "application/json")
                    .DELETE()
                    .build();

            HttpResponse<String> response = client.send(request, HttpResponse.BodyHandlers.ofString());

            int responseStatusCode = response.statusCode();
            if (responseStatusCode != 200) {
                throw new HttpException(ErrorMessages.unsuccessfulHttpRequestErrorMessage);
            }
        } catch (URISyntaxException e) {
            throw new HttpException(ErrorMessages.incorrectUrlErrorMessage);
        } catch (IOException | InterruptedException e) {
            throw new HttpException(String.format(ErrorMessages.sendHttpRequestErrorMessage, Constants.CENTRAL_PEER_COORDINATOR_BASE_URL + Constants.CENTRAL_PEER_COORDINATOR_PEERS_ENDPOINT));
        }
    }

    public List<Peer> getPeers(Peer ownPeer) throws HttpException, ParseException {
        try {
            HttpRequest request = HttpRequest.newBuilder()
                    .uri(new URI(Constants.CENTRAL_PEER_COORDINATOR_BASE_URL + Constants.CENTRAL_PEER_COORDINATOR_PEERS_ENDPOINT))
                    .GET()
                    .build();

            HttpResponse<String> response = client.send(request, HttpResponse.BodyHandlers.ofString());

            String responseBody = response.body();
            int responseStatusCode = response.statusCode();
            if (responseStatusCode != 200) {
                throw new HttpException(ErrorMessages.unsuccessfulHttpRequestErrorMessage);
            }

            List<Peer> peers = ListSerializationHelper.deserializeList(responseBody, Peer.class);
            if (peers == null) {
                throw new ParseException(ErrorMessages.parsePeerErrorMessage);
            }

            peers = peers.stream().filter(peer -> !peer.getId().equals(ownPeer.getId())).toList();

            return peers;
        } catch (URISyntaxException e) {
            throw new HttpException(ErrorMessages.incorrectUrlErrorMessage);
        } catch (IOException | InterruptedException e) {
            throw new HttpException(String.format(ErrorMessages.sendHttpRequestErrorMessage, Constants.CENTRAL_PEER_COORDINATOR_BASE_URL + Constants.CENTRAL_PEER_COORDINATOR_PEERS_ENDPOINT));
        }
    }

    public void checkToken(String token) throws HttpException {
        try {
            HttpRequest request = HttpRequest.newBuilder()
                    .uri(new URI(Constants.AUTHORIZATION_PROVIDER_BASE_URL + Constants.AUTHORIZATION_PROVIDER_CHECK_TOKEN_REQUESTED_ENDPOINT))
                    .headers("Authorization", token)
                    .GET()
                    .build();

            HttpResponse<String> response = client.send(request, HttpResponse.BodyHandlers.ofString());

            int responseStatusCode = response.statusCode();

            if (responseStatusCode == 200) {
                return;
            } else if (responseStatusCode == 401) {
                throw new HttpException(ErrorMessages.invalidTokenErrorMessage);
            } else {
                throw new HttpException(ErrorMessages.unsuccessfulHttpRequestErrorMessage);
            }
        } catch (URISyntaxException e) {
            throw new HttpException(ErrorMessages.incorrectUrlErrorMessage);
        } catch (IOException | InterruptedException e) {
            throw new HttpException(String.format(ErrorMessages.sendHttpRequestErrorMessage, Constants.AUTHORIZATION_PROVIDER_BASE_URL + Constants.AUTHORIZATION_PROVIDER_CHECK_TOKEN_REQUESTED_ENDPOINT));
        }
    }

    public void setVoteCreated(String token) throws HttpException {
        try {
            HttpRequest request = HttpRequest.newBuilder()
                    .uri(new URI(Constants.AUTHORIZATION_PROVIDER_BASE_URL + Constants.AUTHORIZATION_PROVIDER_SET_VOTE_CREATED_ENDPOINT))
                    .headers("Authorization", token)
                    .PUT(HttpRequest.BodyPublishers.noBody())
                    .build();

            HttpResponse<String> response = client.send(request, HttpResponse.BodyHandlers.ofString());

            int responseStatusCode = response.statusCode();

            if (responseStatusCode == 200) {
                return;
            } else if (responseStatusCode == 401) {
                throw new HttpException(ErrorMessages.invalidTokenErrorMessage);
            } else {
                throw new HttpException(ErrorMessages.unsuccessfulHttpRequestErrorMessage);
            }
        } catch (URISyntaxException e) {
            throw new HttpException(ErrorMessages.incorrectUrlErrorMessage);
        } catch (IOException | InterruptedException e) {
            throw new HttpException(String.format(ErrorMessages.sendHttpRequestErrorMessage, Constants.AUTHORIZATION_PROVIDER_BASE_URL + Constants.AUTHORIZATION_PROVIDER_SET_VOTE_CREATED_ENDPOINT));
        }
    }

}
