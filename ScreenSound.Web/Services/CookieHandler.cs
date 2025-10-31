using Microsoft.AspNetCore.Components.WebAssembly.Http;

namespace ScreenSound.Web.Services;

public class CookieHandler : DelegatingHandler
{
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        // Inclui cookies e outras credenciais do navegador na requisição HTTP
        request.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);

        return base.SendAsync(request, cancellationToken);
    }
}