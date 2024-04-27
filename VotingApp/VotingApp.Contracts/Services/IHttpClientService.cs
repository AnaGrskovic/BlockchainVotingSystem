namespace VotingApp.Contracts.Services;

public interface IHttpClientService
{
    Task GetAsync(string url, string token);
}