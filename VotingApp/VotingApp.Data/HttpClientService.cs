using VotingApp.Contracts.Services;

namespace VotingApp.Services;

public class HttpClientService : IHttpClientService
{
    private readonly IHttpClientFactory _httpClientFactory;
    public HttpClientService(IHttpClientFactory httpClientFactory) =>
        _httpClientFactory = httpClientFactory;

    public async Task GetAsync(string url, string token)
    {
        var httpClient = _httpClientFactory.CreateClient();

        httpClient.DefaultRequestHeaders.Add("Authorization", token);

        using var httpResponseMessage = await httpClient.GetAsync(url);

        httpResponseMessage.EnsureSuccessStatusCode();
    }

    public async Task PutAsync(string url, string token)
    {
        var httpClient = _httpClientFactory.CreateClient();

        httpClient.DefaultRequestHeaders.Add("Authorization", token);

        var content = new StringContent(string.Empty);

        using var httpResponseMessage = await httpClient.PutAsync(url, content);

        httpResponseMessage.EnsureSuccessStatusCode();
    }
}
