using Microsoft.EntityFrameworkCore;
using Microservicio.Vuelos.DataAccess.Context;
using Microservicio.Vuelos.DataAccess.Entities;
using Microservicio.Vuelos.DataAccess.Repositories.Interfaces;

namespace Microservicio.Vuelos.DataAccess.Repositories
{
    public class RolRepository : IRolRepository
    {
        private readonly SistemaVuelosDBContext _context;

        public RolRepository(SistemaVuelosDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<RolEntity>> ObtenerTodosAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Roles
                .AsNoTracking()
                .Where(r => !r.EsEliminado)
                .OrderBy(r => r.NombreRol)
                .ToListAsync(cancellationToken);
        }

        public async Task<RolEntity?> ObtenerPorIdAsync(int idRol, CancellationToken cancellationToken = default)
        {
            return await _context.Roles
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.IdRol == idRol && !r.EsEliminado, cancellationToken);
        }

        public async Task<RolEntity?> ObtenerPorGuidAsync(Guid rolGuid, CancellationToken cancellationToken = default)
        {
            return await _context.Roles
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.RolGuid == rolGuid && !r.EsEliminado, cancellationToken);
        }

        public async Task<RolEntity?> ObtenerPorNombreAsync(string nombreRol, CancellationToken cancellationToken = default)
        {
            return await _context.Roles
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.NombreRol == nombreRol && !r.EsEliminado, cancellationToken);
        }

        public async Task<bool> ExistePorIdAsync(int idRol, CancellationToken cancellationToken = default)
        {
            return await _context.Roles
                .AnyAsync(r => r.IdRol == idRol && !r.EsEliminado, cancellationToken);
        }

        public async Task<bool> ExistePorGuidAsync(Guid rolGuid, CancellationToken cancellationToken = default)
        {
            return await _context.Roles
                .AnyAsync(r => r.RolGuid == rolGuid && !r.EsEliminado, cancellationToken);
        }

        public async Task<bool> ExistePorNombreAsync(string nombreRol, CancellationToken cancellationToken = default)
        {
            return await _context.Roles
                .AnyAsync(r => r.NombreRol == nombreRol && !r.EsEliminado, cancellationToken);
        }

        public async Task AgregarAsync(RolEntity entity, CancellationToken cancellationToken = default)
        {
            await _context.Roles.AddAsync(entity, cancellationToken);
        }

        public void Actualizar(RolEntity entity)
        {
            _context.Roles.Update(entity);
        }

        public void Eliminar(RolEntity entity)
        {
            entity.EsEliminado = true;
            _context.Roles.Update(entity);
        }
    }
}