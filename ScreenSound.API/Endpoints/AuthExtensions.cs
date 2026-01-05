using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;
using ScreenSound.Shared.Dados.Modelos;

namespace ScreenSound.API.Endpoints;

public static class AuthExtensions
{
    public static void AddEnpointAuth(this WebApplication app)
    {
        // Método que cria automaticamente as rotas de autenticação
        app.MapGroup("auth")
            .MapIdentityApi<PessoaComAcesso>()
            .WithTags("_Autorização");
    }
}