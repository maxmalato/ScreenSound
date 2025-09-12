using ScreenSound.Web.Requests;
using ScreenSound.Web.Response;
using System.Net.Http.Json;

namespace ScreenSound.Web.Services;

public class ArtistaAPI
{
    private readonly HttpClient _httpClient;

    public ArtistaAPI(IHttpClientFactory factory)
    {
        _httpClient = factory.CreateClient("API");
    }

    // Consultar todos os artistas
    public async Task<ICollection<ArtistaResponse>?> GetArtistasAsync()
    {
        return await _httpClient.GetFromJsonAsync<ICollection<ArtistaResponse>>("artistas");
    }

    // Consultar artista por ID
    public async Task<ArtistaResponse?> GetArtistaByIdAsync(int id)
    {
        return await _httpClient.GetFromJsonAsync<ArtistaResponse>($"artistas/{id}");
    }
}
