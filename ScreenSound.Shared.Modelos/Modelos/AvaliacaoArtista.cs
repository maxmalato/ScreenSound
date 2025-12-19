using ScreenSound.Modelos;

namespace ScreenSound.Shared.Modelos.Modelos;

public class AvaliacaoArtista
{
    public int ArtistaId { get; set; }
    public virtual Artista? Artista { get; set; } // Quando for necessário, carregar o artista associado
    public int PessoaId { get; set; }
    public int Nota { get; set; }
}
