package diplrad.models.vote;

import com.google.gson.annotations.Expose;

public class VoteMessage {

    @Expose
    private String token;
    @Expose
    private String vote;

    public String getToken() {
        return token;
    }
    public String getVote() {
        return vote;
    }

}
