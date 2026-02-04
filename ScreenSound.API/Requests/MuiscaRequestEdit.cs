namespace ScreenSound.API.Requests;

public record MusicaRequestEdit(int Id, string Nome, int ArtistaId, int AnoLancamento, ICollection<int>? GenerosId = null)
    : MusicaRequest(Nome, ArtistaId, AnoLancamento, GenerosId);