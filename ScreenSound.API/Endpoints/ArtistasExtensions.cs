using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using ScreenSound.API.Requests;
using ScreenSound.API.Response;
using ScreenSound.Banco;
using ScreenSound.Modelos;
using ScreenSound.Shared.Dados.Modelos;

namespace ScreenSound.API.Endpoints;

public static class ArtistasExtensions
{
    public static void AddEndPointsArtistas(this WebApplication app)
    {
        var groupBuilderArtistas = app
            .MapGroup("artistas")
            .RequireAuthorization()
            .WithTags("Artistas");

        #region Endpoint Artistas

        groupBuilderArtistas.MapGet("", ([FromServices] DAL<Artista> dal) =>
        {
            var listaDeArtistas = dal.Listar();
            var listadeArtistasOrdenado = listaDeArtistas.OrderBy(a => a.Nome);
            var listaDeArtistaResponse = EntityListToResponseList(listadeArtistasOrdenado);
            
            return Results.Ok(listaDeArtistaResponse);
        });

        groupBuilderArtistas.MapGet("{nome}", ([FromServices] DAL<Artista> dal, string nome) =>
        {
            var artista = dal.RecuperarPor(a => a.Nome.ToUpper().Equals(nome.ToUpper()));
            if (artista is null)
            {
                return Results.NotFound();
            }

            return Results.Ok(EntityToResponse(artista));
        });

        groupBuilderArtistas.MapPost("",
            async ([FromServices] IHostEnvironment env, [FromServices] DAL<Artista> dal,
                [FromBody] ArtistaRequest artistaRequest) =>
            {
                var nome = artistaRequest.Nome.Trim();
                var imagemArtista = DateTime.Now.ToString("dd-MM-yyyy") + "." + nome + ".jpeg";
                var path = Path.Combine(env.ContentRootPath, "wwwroot", "FotosPerfil", imagemArtista);

                using MemoryStream ms = new(Convert.FromBase64String(artistaRequest.FotoPerfil!));
                await using FileStream fs = new(path, FileMode.Create);

                await ms.CopyToAsync(fs);

                var artista = new Artista(artistaRequest.Nome, artistaRequest.Bio)
                {
                    FotoPerfil = $"/FotosPerfil/{imagemArtista}"
                };

                dal.Adicionar(artista);
                return Results.Ok();
            });

        groupBuilderArtistas.MapDelete("{id}", ([FromServices] DAL<Artista> dal, int id) =>
        {
            var artista = dal.RecuperarPor(a => a.Id == id);
            if (artista is null)
            {
                return Results.NotFound();
            }

            dal.Deletar(artista);
            return Results.NoContent();
        });

        groupBuilderArtistas.MapPut("", ([FromServices] DAL<Artista> dal, [FromBody] ArtistaRequestEdit artistaRequestEdit) =>
        {
            var artistaAAtualizar = dal.RecuperarPor(a => a.Id == artistaRequestEdit.Id);
            if (artistaAAtualizar is null)
            {
                return Results.NotFound();
            }

            artistaAAtualizar.Nome = artistaRequestEdit.Nome;
            artistaAAtualizar.Bio = artistaRequestEdit.Bio;
            dal.Atualizar(artistaAAtualizar);
            return Results.Ok();
        });
        
        // Avaliar um artista
        groupBuilderArtistas.MapPost("avaliacao", (
            HttpContext context,
            [FromBody] AvaliacaoArtistaRequest avaliacaoArtistaRequest,
            [FromServices] DAL<Artista> dalArtista,
            [FromServices] DAL<PessoaComAcesso> dalPessoaComAcesso
            ) =>
        {
            var artista = dalArtista.RecuperarPor(a => a.Id == avaliacaoArtistaRequest.ArtistaId);
            if (artista is null)
            {
                return Results.NotFound();
            }
            
            var email = context
                            .User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value 
                        ?? throw new InvalidOperationException("Pessoa não está cadastrada.");

            var pessoa = dalPessoaComAcesso
                .RecuperarPor(a => a.Email.Equals(email))
                ?? throw new InvalidOperationException("Pessoa não cadastrada.");

            var avaliacao = artista.Avaliacoes.FirstOrDefault(a => a.ArtistaId == artista.Id && a.PessoaId == pessoa.Id);

            if (avaliacao is null)
            {
                artista.AdicionarNota(pessoa.Id, avaliacaoArtistaRequest.Nota);
            }
            else
            {
                avaliacao.Nota = avaliacaoArtistaRequest.Nota;
            }

            dalArtista.Atualizar(artista);

            return Results.Created();
        });

        #endregion
    }

    private static ICollection<ArtistaResponse> EntityListToResponseList(IEnumerable<Artista> listaDeArtistas)
    {
        return listaDeArtistas.Select(a => EntityToResponse(a)).ToList();
    }

    private static ArtistaResponse EntityToResponse(Artista artista)

    {
        return new ArtistaResponse(artista.Id, artista.Nome, artista.Bio, artista.FotoPerfil);
    }
}