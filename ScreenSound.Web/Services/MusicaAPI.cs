using ScreenSound.Web.Requests;
using ScreenSound.Web.Response;
using System.Net.Http.Json;

namespace ScreenSound.Web.Services;

public class MusicaAPI
{
    private readonly HttpClient _httpClient;

    public MusicaAPI(IHttpClientFactory factory)
    {
        _httpClient = factory.CreateClient("API");
    }

    // Listar todas as músicas
    public async Task<ICollection<MusicaResponse>?> GetMusicasAsync()
    {
        return await _httpClient.GetFromJsonAsync<ICollection<MusicaResponse>>("musicas");
    }

    // Listar música por nome
    public async Task<MusicaResponse?> GetMusicaPorNomeAsync(string nome)
    {
        return await _httpClient.GetFromJsonAsync<MusicaResponse>($"musicas/{nome}");
    }
    
    // Listar músicas por gênero
    public async Task<ICollection<MusicaResponse?>> GetMusicasPorGeneroAsync(int generoId)
    {
        return await  _httpClient.GetFromJsonAsync<ICollection<MusicaResponse?>>($"musicas/por-genero/{generoId}");
    }

    // Adicionar nova música
    public async Task AddMusicaAsync(MusicaRequest musica)
    {
        await _httpClient.PostAsJsonAsync("musicas", musica);
    }

    // Excluir música por ID
    public async Task DeleteMusicaAsync(int id)
    {
        await _httpClient.DeleteAsync($"musicas/{id}");
    }

    // Editar música
    public async Task UpdateMusicaAsync(MusicaRequestEdit artista)
    {
        await _httpClient.PutAsJsonAsync("musicas/", artista);
    }
}
