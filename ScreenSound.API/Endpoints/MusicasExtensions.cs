using Microsoft.AspNetCore.Mvc;
using ScreenSound.API.Requests;
using ScreenSound.API.Response;
using ScreenSound.Banco;
using ScreenSound.Modelos;
using ScreenSound.Shared.Modelos.Modelos;

namespace ScreenSound.API.Endpoints;

public static class MusicasExtensions
{
    public static void AddEndPointsMusicas(this WebApplication app)
    {
        var groupBuilderMusicas = app
            .MapGroup("musicas")
            .RequireAuthorization()
            .WithTags("Músicas");

        #region Endpoint Músicas

        groupBuilderMusicas.MapGet("", ([FromServices] DAL<Musica> dal) =>
        {
            var listaDeMusicas = dal.Listar();
            if (listaDeMusicas is null)
            {
                return Results.NotFound();
            }
            var listaDeMusicaResponse = EntityListToResponseList(listaDeMusicas);
            return Results.Ok(listaDeMusicaResponse);
        });

        groupBuilderMusicas.MapGet("{nome}", ([FromServices] DAL<Musica> dal, string nome) =>
        {
            var musica = dal.RecuperarPor(a => a.Nome.ToUpper().Equals(nome.ToUpper()));
            if (musica is null)
            {
                return Results.NotFound();
            }
            return Results.Ok(EntityToResponse(musica));
        });

        groupBuilderMusicas.MapPost("", ([FromServices] DAL<Musica> dal, [FromServices] DAL<Genero> dalGenero, [FromBody] MusicaRequest musicaRequest) =>
        {
            var nomeMusica = musicaRequest.Nome.Trim();
            var musica = new Musica(nomeMusica)
            {
                ArtistaId = musicaRequest.ArtistaId,
                AnoLancamento = musicaRequest.AnoLancamento,
                Generos = musicaRequest.Generos is not null ? GeneroRequestConverter(musicaRequest.Generos, dalGenero) :
                new List<Genero>()
            };
            dal.Adicionar(musica);
            return Results.Ok();
        });

        groupBuilderMusicas.MapDelete("{id}", ([FromServices] DAL<Musica> dal, int id) =>
        {
            var musica = dal.RecuperarPor(a => a.Id == id);
            if (musica is null)
            {
                return Results.NotFound();
            }
            dal.Deletar(musica);
            return Results.NoContent();
        });

        groupBuilderMusicas.MapPut("", ([FromServices] DAL<Musica> dal, [FromBody] MusicaRequestEdit musicaRequestEdit) =>
        {
            var musicaAAtualizar = dal.RecuperarPor(a => a.Id == musicaRequestEdit.Id);
            if (musicaAAtualizar is null)
            {
                return Results.NotFound();
            }
            musicaAAtualizar.Nome = musicaRequestEdit.Nome;
            musicaAAtualizar.AnoLancamento = musicaRequestEdit.AnoLancamento;
            musicaAAtualizar.ArtistaId = musicaRequestEdit.ArtistaId;

            dal.Atualizar(musicaAAtualizar);
            return Results.Ok();
        });
        
        // Listar músicas por gênero
        groupBuilderMusicas.MapGet("por-genero/{generoId}", ([FromServices] DAL<Musica> dal, int generoId) =>
        {
            var listaMusicasPorGenero = dal.Listar(m => m.Generos.Any(g => g.Id == generoId));
            var listaMusicasPorGeneroResponse = EntityListToResponseList(listaMusicasPorGenero);
            
            return Results.Ok(listaMusicasPorGeneroResponse);
        });

        #endregion Endpoint Músicas
    }

    private static ICollection<Genero> GeneroRequestConverter(ICollection<GeneroRequest> generos, DAL<Genero> dalGenero)
    {
        var listaDeGeneros = new List<Genero>();
        foreach (var item in generos)
        {
            var entity = RequestToEntity(item);
            var genero = dalGenero.RecuperarPor(g => g.Nome.ToUpper().Equals(item.Nome.ToUpper()));
            if (genero is not null)
            {
                listaDeGeneros.Add(genero);
            }
            else
            {
                listaDeGeneros.Add(entity);
            }
        }

        return listaDeGeneros;
    }

    private static Genero RequestToEntity(GeneroRequest genero)
    {
        return new Genero() { Nome = genero.Nome, Descricao = genero.Descricao };
    }

    private static ICollection<MusicaResponse> EntityListToResponseList(IEnumerable<Musica> musicaList)
    {
        return musicaList.Select(a => EntityToResponse(a)).ToList();
    }

    private static MusicaResponse EntityToResponse(Musica musica)
    {
        if (musica.Artista is null)
        {
            return new MusicaResponse(musica.Id, musica.Nome, 0, "Artista não cadastrado", 0);
        }

        return new MusicaResponse(musica.Id, musica.Nome, musica.Artista.Id, musica.Artista.Nome, musica.AnoLancamento);
    }
}