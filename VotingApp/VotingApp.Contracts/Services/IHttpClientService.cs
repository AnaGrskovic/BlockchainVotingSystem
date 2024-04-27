namespace VotingApp.Contracts.Services;

public interface IHttpClientService
{
    Task<T?> GetAsync<T>(string url);
    Task PostAsync(string url, object data);
    Task PutAsync(string url, object data);
    Task DeleteAsync(string url);
}