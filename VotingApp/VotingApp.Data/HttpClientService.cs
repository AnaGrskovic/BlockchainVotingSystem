using static System.Net.Mime.MediaTypeNames;
using System.Text.Json;
using System.Text;
using VotingApp.Contracts.Services;

namespace VotingApp.Services;

public class HttpClientService : IHttpClientService
{
    private readonly IHttpClientFactory _httpClientFactory;
    public HttpClientService(IHttpClientFactory httpClientFactory) =>
        _httpClientFactory = httpClientFactory;

    public async Task<T?> GetAsync<T>(string url, string token)
    {
        var httpClient = _httpClientFactory.CreateClient();

        httpClient.DefaultRequestHeaders.Add("Authorization", token);

        using var httpResponseMessage =
            await httpClient.GetAsync(url);

        httpResponseMessage.EnsureSuccessStatusCode();

        using var contentStream = await httpResponseMessage.Content.ReadAsStreamAsync();

        return await JsonSerializer.DeserializeAsync<T>(contentStream);
    }

    public async Task<T?> GetAsync<T>(string url)
    {
        var httpClient = _httpClientFactory.CreateClient();
        using var httpResponseMessage =
            await httpClient.GetAsync(url);

        httpResponseMessage.EnsureSuccessStatusCode();

        using var contentStream = await httpResponseMessage.Content.ReadAsStreamAsync();

        return await JsonSerializer.DeserializeAsync<T>(contentStream);
    }

    public async Task PostAsync(string url, object data)
    {
        var dataJson = GetDataJson(data);

        var httpClient = _httpClientFactory.CreateClient();
        using var httpResponseMessage =
            await httpClient.PostAsync(url, dataJson);

        httpResponseMessage.EnsureSuccessStatusCode();
    }

    public async Task PutAsync(string url, object data)
    {
        var dataJson = GetDataJson(data);

        var httpClient = _httpClientFactory.CreateClient();
        using var httpResponseMessage =
            await httpClient.PutAsync(url, dataJson);

        httpResponseMessage.EnsureSuccessStatusCode();
    }

    public async Task DeleteAsync(string url)
    {
        var httpClient = _httpClientFactory.CreateClient();
        using var httpResponseMessage =
            await httpClient.DeleteAsync(url);

        httpResponseMessage.EnsureSuccessStatusCode();
    }

    private static StringContent GetDataJson(object data)
    {
        return new StringContent(
                    JsonSerializer.Serialize(data),
                    Encoding.UTF8,
                    Application.Json);
    }
}
