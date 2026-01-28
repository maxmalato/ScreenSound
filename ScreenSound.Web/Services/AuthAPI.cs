using System.Net.Http.Json;
using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using ScreenSound.Web.Response;

namespace ScreenSound.Web.Services;

public class AuthAPI(IHttpClientFactory factory) : AuthenticationStateProvider
{
    private bool autenticado = false;

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

            autenticado = true;
        }

        return new AuthenticationState(pessoa);
    }

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
            var claims = new[] { new Claim(ClaimTypes.Name, email), new Claim(ClaimTypes.Email, email) };
            var identity = new ClaimsIdentity(claims, "Cookies");
            var user = new ClaimsPrincipal(identity);

            var authState = Task.FromResult(new AuthenticationState(user));

            NotifyAuthenticationStateChanged(authState);

            return new AuthResponse { Sucesso = true };
        }

        return new AuthResponse { Sucesso = false, Erros = ["E-mail e/ou Senha inválido"] };
    }

    public async Task<AuthResponse> RegisterAsync(string email, string senha)
    {
        var response = await _httpClient.PostAsJsonAsync("auth/register", new
        {
            email,
            password = senha,
        });

        if (response.IsSuccessStatusCode)
        {
            return new AuthResponse { Sucesso = true };
        }

        var errosValidation = await response.Content.ReadFromJsonAsync<ValidationProblemResponse>();

        // Mapear os erros de validação
        var listaErros = new List<string>();

        if (errosValidation?.Errors != null)
        {
            foreach (var erro in errosValidation.Errors)
            {
                listaErros.AddRange(erro.Value);
            }
        }
        else
        {
            listaErros.Add("Não foi possível realizar o cadastro.");
        }

        return new AuthResponse { Sucesso = false, Erros = listaErros.ToArray() };
    }

    private class ValidationProblemResponse
    {
        public Dictionary<string, string[]> Errors { get; set; } = new();
    }

    public void NotifyAuthState()
    {
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }

    // Criar um método para realizar o logout
    public async Task LogoutAsync()
    {
        var response = await _httpClient.PostAsync("auth/logout", null);
        if (response.IsSuccessStatusCode)
        {
            // Propagar o estado de autenticação
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }
    }

    public async Task<bool> VerificarAutenticado()
    {
        await GetAuthenticationStateAsync();

        return autenticado;
    }
}