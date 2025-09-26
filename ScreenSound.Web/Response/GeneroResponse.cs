namespace ScreenSound.Web.Response;

public record GeneroResponse(int Id, string Nome, string Descricao)
{
    public override string ToString() => Nome;
}