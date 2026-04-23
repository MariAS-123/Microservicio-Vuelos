using Microsoft.EntityFrameworkCore;
using Microservicio.Vuelos.DataAccess.Context;
using Microservicio.Vuelos.DataAccess.Entities;
using Microservicio.Vuelos.DataAccess.Repositories.Interfaces;

namespace Microservicio.Vuelos.DataAccess.Repositories
{
    public class AuditoriaLogRepository : IAuditoriaLogRepository
    {
        private readonly SistemaVuelosDBContext _context;

        public AuditoriaLogRepository(SistemaVuelosDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<AuditoriaLogEntity>> ObtenerTodosAsync(CancellationToken cancellationToken = default)
        {
            return await _context.AuditoriaLogs
                .AsNoTracking()
                .Where(a => a.Activo)
                .OrderByDescending(a => a.FechaEventoUtc)
                .ToListAsync(cancellationToken);
        }

        public async Task<AuditoriaLogEntity?> ObtenerPorIdAsync(long idAuditoria, CancellationToken cancellationToken = default)
        {
            return await _context.AuditoriaLogs
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.IdAuditoria == idAuditoria && a.Activo, cancellationToken);
        }

        public async Task<AuditoriaLogEntity?> ObtenerPorGuidAsync(Guid auditoriaGuid, CancellationToken cancellationToken = default)
        {
            return await _context.AuditoriaLogs
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.AuditoriaGuid == auditoriaGuid && a.Activo, cancellationToken);
        }

        public async Task<IEnumerable<AuditoriaLogEntity>> ObtenerPorTablaAfectadaAsync(string tablaAfectada, CancellationToken cancellationToken = default)
        {
            return await _context.AuditoriaLogs
                .AsNoTracking()
                .Where(a => a.TablaAfectada == tablaAfectada && a.Activo)
                .OrderByDescending(a => a.FechaEventoUtc)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<AuditoriaLogEntity>> ObtenerPorOperacionAsync(string operacion, CancellationToken cancellationToken = default)
        {
            return await _context.AuditoriaLogs
                .AsNoTracking()
                .Where(a => a.Operacion == operacion && a.Activo)
                .OrderByDescending(a => a.FechaEventoUtc)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<AuditoriaLogEntity>> ObtenerPorUsuarioEjecutorAsync(string usuarioEjecutor, CancellationToken cancellationToken = default)
        {
            return await _context.AuditoriaLogs
                .AsNoTracking()
                .Where(a => a.UsuarioEjecutor == usuarioEjecutor && a.Activo)
                .OrderByDescending(a => a.FechaEventoUtc)
                .ToListAsync(cancellationToken);
        }

        public async Task<bool> ExistePorIdAsync(long idAuditoria, CancellationToken cancellationToken = default)
        {
            return await _context.AuditoriaLogs
                .AnyAsync(a => a.IdAuditoria == idAuditoria && a.Activo, cancellationToken);
        }

        public async Task<bool> ExistePorGuidAsync(Guid auditoriaGuid, CancellationToken cancellationToken = default)
        {
            return await _context.AuditoriaLogs
                .AnyAsync(a => a.AuditoriaGuid == auditoriaGuid && a.Activo, cancellationToken);
        }

        public async Task AgregarAsync(AuditoriaLogEntity entity, CancellationToken cancellationToken = default)
        {
            await _context.AuditoriaLogs.AddAsync(entity, cancellationToken);
        }

        public void Actualizar(AuditoriaLogEntity entity)
        {
            _context.AuditoriaLogs.Update(entity);
        }

        public void Eliminar(AuditoriaLogEntity entity)
        {
            entity.Activo = false;
            _context.AuditoriaLogs.Update(entity);
        }
    }
}