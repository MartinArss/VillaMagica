using System.Linq.Expressions;

namespace MagicVilla_API.Repositorio.Repositorio
{
    // Con T decimos que aqui oodemos recibir cualquier tipo de entidad
    public interface IRepositorio<T> where T : class
    {
        Task Crear(T entidad);
        Task<List<T>> ObtenerTodos(Expression<Func<T, bool>>? filtro = null);
        Task<T?> Obtener(Expression<Func<T, bool>> filtro = null, bool tracked = true);
        Task Remover(T entidad);
        Task GuardarCambios();
    }
}
