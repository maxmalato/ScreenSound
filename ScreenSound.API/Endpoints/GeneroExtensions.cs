using Microsoft.AspNetCore.Mvc;
using ScreenSound.API.Requests;
using ScreenSound.API.Response;
using ScreenSound.Banco;
using ScreenSound.Shared.Modelos.Modelos;

namespace ScreenSound.API.Endpoints;

public static class GeneroExtensions
{
    public static void AddEndPointGeneros(this WebApplication app)
    {
        var groupBuilderGenero = app
            .MapGroup("generos")
            .RequireAuthorization()
            .WithTags("Gêneros");

        // Listar todos os gêneros
        groupBuilderGenero.MapGet("", ([FromServices] DAL<Genero> dal) =>
        {
            var listaDeGeneros = dal.Listar();
            var listaDeArtistasResponse =  EntityListToResponseList(listaDeGeneros);

            return Results.Ok(listaDeArtistasResponse);
        });

        // Listar gênero por nome
        groupBuilderGenero.MapGet("{nome}", ([FromServices] DAL<Genero> dal, string nome) =>
        {
            var genero = dal.RecuperarPor(a => a.Nome.ToUpper().Equals(nome.ToUpper()));
            if (genero is not null)
            {
                var response = EntityToResponse(genero!);
                return Results.Ok(response);
            }
            return Results.NotFound("Gênero não encontrado.");
        });

        // Adicionar novo gênero
        groupBuilderGenero.MapPost("", ([FromServices] DAL<Genero> dal, [FromBody] GeneroRequest generoReq) =>
        {
            dal.Adicionar(RequestToEntity(generoReq));
        });

        // Atualizar gênero por id
        groupBuilderGenero.MapPut("", ([FromServices] DAL<Genero> dal, [FromBody] GeneroRequestEdit generoRequestEdit) =>
        {
            var generoAAtualizar = dal.RecuperarPor(a => a.Id == generoRequestEdit.Id);
            
            if (generoAAtualizar is null)
            {
                return Results.NotFound("Gênero para atualização não encontrado.");
            }

            generoAAtualizar.Nome = generoRequestEdit.Nome;
            generoAAtualizar.Descricao = generoRequestEdit.Descricao;

            dal.Atualizar(generoAAtualizar);
            
            return Results.Ok();
        });

        // Excluir gênero por id
        groupBuilderGenero.MapDelete("{id}", ([FromServices] DAL<Genero> dal, int id) =>
        {
            var genero = dal.RecuperarPor(a => a.Id == id);
            if (genero is null)
            {
                return Results.NotFound("Gênero para exclusão não encontrado.");
            }
            dal.Deletar(genero);
            return Results.NoContent();
        });
    }

    private static Genero RequestToEntity(GeneroRequest generoRequest)
    {
        return new Genero() { Nome = generoRequest.Nome, Descricao = generoRequest.Descricao };
    }

    private static ICollection<GeneroResponse> EntityListToResponseList(IEnumerable<Genero> generos)
    {
        return generos.Select(a => EntityToResponse(a)).ToList();
    }

    private static GeneroResponse EntityToResponse(Genero genero)
    {
        return new GeneroResponse(genero.Id, genero.Nome!, genero.Descricao!);
    }
}