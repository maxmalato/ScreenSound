using Microsoft.AspNetCore.Components.Authorization;
using ScreenSound.Web.Response;
using System.Net.Http.Json;
using System.Security.Claims;

namespace ScreenSound.Web.Services;

public class AuthAPI(IHttpClientFactory factory) : AuthenticationStateProvider
{
    private readonly HttpClient _httpClient = factory.CreateClient("API");

    // Método que pega o estado de autenticação
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var pessoa = new ClaimsPrincipal(); // Estado anônimo por padrão (não autenticado)
        var response = await _httpClient.GetAsync("auth/manage/info");

        if (response.IsSuccessStatusCode)
        {
            var info = await response.Content.ReadFromJsonAsync<InfoPessoaResponse>();
            Claim[] dados =
            [
                new Claim(ClaimTypes.Name, info.Email),
                new Claim(ClaimTypes.Email, info.Email),
            ];

            var identity = new ClaimsIdentity(dados, "Cookies");

            pessoa = new ClaimsPrincipal(identity);
        }

        return new AuthenticationState(pessoa);
    }

    // Criar um método para realizar a autenticação
    public async Task<AuthResponse> LoginAsync(string email, string senha)
    {
        var cookiesUrl = "auth/login?useCookies=true&useSessionCookies=true";
        //var cookiesUrl = "auth/login?useCookies=true";

        var response = await _httpClient.PostAsJsonAsync(cookiesUrl, new
        {
            email,
            password = senha,
        });

        if (response.IsSuccessStatusCode)
        {
            // Propagar o estado de autenticação
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
            return new AuthResponse { Sucesso = true };
        }

        return new AuthResponse { Sucesso = false, Erros = ["E-mail e/ou Senha inválido"] };
    }
}