using Microservicio.Vuelos.DataAccess.Entities;

namespace Microservicio.Vuelos.DataAccess.Repositories.Interfaces
{
    public interface IUsuarioRolRepository
    {
        Task<IEnumerable<UsuarioRolEntity>> ObtenerTodosAsync(CancellationToken cancellationToken = default);
        Task<UsuarioRolEntity?> ObtenerPorIdAsync(int idUsuarioRol, CancellationToken cancellationToken = default);
        Task<IEnumerable<UsuarioRolEntity>> ObtenerPorUsuarioAsync(int idUsuario, CancellationToken cancellationToken = default);
        Task<IEnumerable<UsuarioRolEntity>> ObtenerPorRolAsync(int idRol, CancellationToken cancellationToken = default);
        Task<UsuarioRolEntity?> ObtenerPorUsuarioYRolAsync(int idUsuario, int idRol, CancellationToken cancellationToken = default);
        Task<bool> ExistePorIdAsync(int idUsuarioRol, CancellationToken cancellationToken = default);
        Task<bool> ExistePorUsuarioYRolAsync(int idUsuario, int idRol, CancellationToken cancellationToken = default);
        Task AgregarAsync(UsuarioRolEntity entity, CancellationToken cancellationToken = default);
        void Actualizar(UsuarioRolEntity entity);
        void Eliminar(UsuarioRolEntity entity);
    }
}