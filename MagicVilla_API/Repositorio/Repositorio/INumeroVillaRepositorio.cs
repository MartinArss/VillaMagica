using MagicVilla_API.Modelos;

namespace MagicVilla_API.Repositorio.Repositorio
{
    public interface INumeroVillaRepositorio :  IRepositorio<NumeroVilla>
    {
        Task<NumeroVilla> Actualizar(NumeroVilla entidad);
    }
}
