using Microservicio.Vuelos.DataAccess.Entities;

namespace Microservicio.Vuelos.DataAccess.Repositories.Interfaces
{
    public interface IUsuarioAppRepository
    {
        Task<IEnumerable<UsuarioAppEntity>> ObtenerTodosAsync(CancellationToken cancellationToken = default);
        Task<UsuarioAppEntity?> ObtenerPorIdAsync(int idUsuario, CancellationToken cancellationToken = default);
        Task<UsuarioAppEntity?> ObtenerPorGuidAsync(Guid usuarioGuid, CancellationToken cancellationToken = default);
        Task<UsuarioAppEntity?> ObtenerPorUsernameAsync(string username, CancellationToken cancellationToken = default);
        Task<UsuarioAppEntity?> ObtenerPorCorreoAsync(string correo, CancellationToken cancellationToken = default);
        Task<IEnumerable<UsuarioAppEntity>> ObtenerPorClienteAsync(int idCliente, CancellationToken cancellationToken = default);
        Task<bool> ExistePorIdAsync(int idUsuario, CancellationToken cancellationToken = default);
        Task<bool> ExistePorGuidAsync(Guid usuarioGuid, CancellationToken cancellationToken = default);
        Task<bool> ExistePorUsernameAsync(string username, CancellationToken cancellationToken = default);
        Task<bool> ExistePorCorreoAsync(string correo, CancellationToken cancellationToken = default);
        Task AgregarAsync(UsuarioAppEntity entity, CancellationToken cancellationToken = default);
        void Actualizar(UsuarioAppEntity entity);
        void Eliminar(UsuarioAppEntity entity);
    }
}