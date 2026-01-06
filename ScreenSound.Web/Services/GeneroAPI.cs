using ScreenSound.Web.Requests;
using ScreenSound.Web.Response;
using System.Net.Http.Json;

namespace ScreenSound.Web.Services;

public class GeneroAPI
{
    private readonly HttpClient _httpClient;

    public GeneroAPI(IHttpClientFactory factory)
    {
        _httpClient = factory.CreateClient("API");
    }

    public async Task<List<GeneroResponse>?> GetGenerosAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<GeneroResponse>>("generos");
    }

    public async Task<GeneroResponse?> GetGeneroPorNomeAsync(string nome)
    {
        return await _httpClient.GetFromJsonAsync<GeneroResponse>($"generos/{nome}");
    }

    public async Task AddGeneroAsync(GeneroRequest genero)
    {
        await _httpClient.PostAsJsonAsync("generos", genero);
    }

    public async Task UpdateGeneroAsync(GeneroRequestEdit genero)
    {
        await _httpClient.PutAsJsonAsync("generos", genero);
    }

    public async Task DeleteGeneroAsync(int id)
    {
        await _httpClient.DeleteAsync($"generos/{id}");
    }
}