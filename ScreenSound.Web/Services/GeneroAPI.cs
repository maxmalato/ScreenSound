using ScreenSound.Web.Response;
using System.Net.Http.Json;

namespace ScreenSound.Web.Services;

public class GeneroAPI(IHttpClientFactory factory)
{
    private readonly HttpClient _httpClient = factory.CreateClient("API");

    // Listar os gêneros
    public async Task<List<GeneroResponse>?> GetGeneroAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<GeneroResponse>>("generos");
    }

    // Listar gênero por nome
    public async Task<List<GeneroResponse>?> GetGeneroAsync(string nome)
    {
        return await _httpClient.GetFromJsonAsync<List<GeneroResponse>>($"generos/{nome}");
    }
}