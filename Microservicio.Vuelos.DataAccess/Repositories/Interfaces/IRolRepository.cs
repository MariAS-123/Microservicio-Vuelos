using Microservicio.Vuelos.DataAccess.Entities;

namespace Microservicio.Vuelos.DataAccess.Repositories.Interfaces
{
    public interface IRolRepository
    {
        Task<IEnumerable<RolEntity>> ObtenerTodosAsync(CancellationToken cancellationToken = default);
        Task<RolEntity?> ObtenerPorIdAsync(int idRol, CancellationToken cancellationToken = default);
        Task<RolEntity?> ObtenerPorGuidAsync(Guid rolGuid, CancellationToken cancellationToken = default);
        Task<RolEntity?> ObtenerPorNombreAsync(string nombreRol, CancellationToken cancellationToken = default);
        Task<bool> ExistePorIdAsync(int idRol, CancellationToken cancellationToken = default);
        Task<bool> ExistePorGuidAsync(Guid rolGuid, CancellationToken cancellationToken = default);
        Task<bool> ExistePorNombreAsync(string nombreRol, CancellationToken cancellationToken = default);
        Task AgregarAsync(RolEntity entity, CancellationToken cancellationToken = default);
        void Actualizar(RolEntity entity);
        void Eliminar(RolEntity entity);
    }
}