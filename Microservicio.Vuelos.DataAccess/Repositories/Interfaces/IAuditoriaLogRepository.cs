using Microservicio.Vuelos.DataAccess.Entities;

namespace Microservicio.Vuelos.DataAccess.Repositories.Interfaces
{
    public interface IAuditoriaLogRepository
    {
        Task<IEnumerable<AuditoriaLogEntity>> ObtenerTodosAsync(CancellationToken cancellationToken = default);
        Task<AuditoriaLogEntity?> ObtenerPorIdAsync(long idAuditoria, CancellationToken cancellationToken = default);
        Task<AuditoriaLogEntity?> ObtenerPorGuidAsync(Guid auditoriaGuid, CancellationToken cancellationToken = default);
        Task<IEnumerable<AuditoriaLogEntity>> ObtenerPorTablaAfectadaAsync(string tablaAfectada, CancellationToken cancellationToken = default);
        Task<IEnumerable<AuditoriaLogEntity>> ObtenerPorOperacionAsync(string operacion, CancellationToken cancellationToken = default);
        Task<IEnumerable<AuditoriaLogEntity>> ObtenerPorUsuarioEjecutorAsync(string usuarioEjecutor, CancellationToken cancellationToken = default);
        Task<bool> ExistePorIdAsync(long idAuditoria, CancellationToken cancellationToken = default);
        Task<bool> ExistePorGuidAsync(Guid auditoriaGuid, CancellationToken cancellationToken = default);
        Task AgregarAsync(AuditoriaLogEntity entity, CancellationToken cancellationToken = default);
        void Actualizar(AuditoriaLogEntity entity);
        void Eliminar(AuditoriaLogEntity entity);
    }
}