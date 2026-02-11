using ScreenSound.Modelos;
using System.Linq.Expressions;

namespace ScreenSound.Banco;

public class DAL<T> where T : class
{
    private readonly ScreenSoundContext context;

    public DAL(ScreenSoundContext context)
    {
        this.context = context;
    }

    public IEnumerable<T> Listar()
    {
        return context.Set<T>().ToList();
    }

    public IEnumerable<T> Listar(Expression<Func<T, bool>> condicao)
    {
        return context.Set<T>().Where(condicao).ToList();
    }

    public void Adicionar(T objeto)
    {
        context.Set<T>().Add(objeto);
        context.SaveChanges();
    }

    public void Atualizar(T objeto)
    {
        context.Set<T>().Update(objeto);
        context.SaveChanges();
    }

    public void Deletar(T objeto)
    {
        if(objeto is Artista artista)
        {
            var temMusicas = context.Musicas.Any(m => m.ArtistaId == artista.Id);

            if(temMusicas)
            {
                throw new InvalidOperationException("Não é possível deletar um artista que possui músicas associadas.");
            }
        }
        context.Set<T>().Remove(objeto);
        context.SaveChanges();
    }

    public T? RecuperarPor(Func<T, bool> condicao)
    {
        return context.Set<T>().FirstOrDefault(condicao);
    }
}