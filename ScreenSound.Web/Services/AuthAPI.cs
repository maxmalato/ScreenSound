using ScreenSound.Web.Response;
using System.Net.Http.Json;

namespace ScreenSound.Web.Services;

public class AuthAPI(IHttpClientFactory factory)
{
    private readonly HttpClient _httpClient = factory.CreateClient("API");

    // Criar um método para realizar a autenticação
    public async Task<AuthResponse> LoginAsync(string email, string senha)
    {
        var cookiesUrl = "auth/login?useCookies=true&useSessionCookies=true";

        var response = await _httpClient.PostAsJsonAsync(cookiesUrl, new
        {
            email,
            password = senha,
        });

        if (response.IsSuccessStatusCode)
        {
            return new AuthResponse { Sucesso = true };
        }

        return new AuthResponse { Sucesso = false, Erros = ["E-mail e/ou Senha inválido"] };
    }
}