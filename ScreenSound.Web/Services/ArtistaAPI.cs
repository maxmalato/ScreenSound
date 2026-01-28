using System.Net.Http.Json;
using ScreenSound.Web.Requests;
using ScreenSound.Web.Response;

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

    // Adicionar novo artista
    public async Task AddArtistaAsync(ArtistaRequest artista)
    {
        await _httpClient.PostAsJsonAsync("artistas", artista);
    }

    // Excluir artista por nome
    public async Task DeleteArtistaAsync(int id)
    {
        await _httpClient.DeleteAsync($"artistas/{id}");
    }

    // Puxar os dados do artista por nome
    public async Task<ArtistaResponse?> GetArtistaPorNomeAsync(string nome)
    {
        return await _httpClient.GetFromJsonAsync<ArtistaResponse>($"artistas/{nome}");
    }

    // Editar artista
    public async Task UpdateArtistaAsync(ArtistaRequestEdit artista)
    {
        await _httpClient.PutAsJsonAsync("artistas/", artista);
    }
    
    // Avaliar um artista
    public async Task AvaliarArtistaAsync(int artistaId, int nota)
    {
        await _httpClient.PostAsJsonAsync("artistas/avaliacao", new { artistaId, nota});
    }
    
    public async Task<AvaliacaoArtistaResponse?> GetAvaliacaoDaPessoaLogadaAsync(int artistaId)
    {
        return await _httpClient
            .GetFromJsonAsync<AvaliacaoArtistaResponse?>($"artistas/{artistaId}/avaliacao");
    }
}