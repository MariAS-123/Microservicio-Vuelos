using Microsoft.EntityFrameworkCore;
using Microservicio.Vuelos.DataAccess.Context;
using Microservicio.Vuelos.DataAccess.Entities;
using Microservicio.Vuelos.DataAccess.Repositories.Interfaces;

namespace Microservicio.Vuelos.DataAccess.Repositories
{
    public class UsuarioRolRepository : IUsuarioRolRepository
    {
        private readonly SistemaVuelosDBContext _context;

        public UsuarioRolRepository(SistemaVuelosDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<UsuarioRolEntity>> ObtenerTodosAsync(CancellationToken cancellationToken = default)
        {
            return await _context.UsuariosRoles
                .AsNoTracking()
                .Where(ur => !ur.EsEliminado)
                .OrderBy(ur => ur.IdUsuario)
                .ThenBy(ur => ur.IdRol)
                .ToListAsync(cancellationToken);
        }

        public async Task<UsuarioRolEntity?> ObtenerPorIdAsync(int idUsuarioRol, CancellationToken cancellationToken = default)
        {
            return await _context.UsuariosRoles
                .AsNoTracking()
                .FirstOrDefaultAsync(ur => ur.IdUsuarioRol == idUsuarioRol && !ur.EsEliminado, cancellationToken);
        }

        public async Task<IEnumerable<UsuarioRolEntity>> ObtenerPorUsuarioAsync(int idUsuario, CancellationToken cancellationToken = default)
        {
            return await _context.UsuariosRoles
                .AsNoTracking()
                .Where(ur => ur.IdUsuario == idUsuario && !ur.EsEliminado)
                .OrderBy(ur => ur.IdRol)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<UsuarioRolEntity>> ObtenerPorRolAsync(int idRol, CancellationToken cancellationToken = default)
        {
            return await _context.UsuariosRoles
                .AsNoTracking()
                .Where(ur => ur.IdRol == idRol && !ur.EsEliminado)
                .OrderBy(ur => ur.IdUsuario)
                .ToListAsync(cancellationToken);
        }

        public async Task<UsuarioRolEntity?> ObtenerPorUsuarioYRolAsync(int idUsuario, int idRol, CancellationToken cancellationToken = default)
        {
            return await _context.UsuariosRoles
                .AsNoTracking()
                .FirstOrDefaultAsync(ur => ur.IdUsuario == idUsuario && ur.IdRol == idRol && !ur.EsEliminado, cancellationToken);
        }

        public async Task<bool> ExistePorIdAsync(int idUsuarioRol, CancellationToken cancellationToken = default)
        {
            return await _context.UsuariosRoles
                .AnyAsync(ur => ur.IdUsuarioRol == idUsuarioRol && !ur.EsEliminado, cancellationToken);
        }

        public async Task<bool> ExistePorUsuarioYRolAsync(int idUsuario, int idRol, CancellationToken cancellationToken = default)
        {
            return await _context.UsuariosRoles
                .AnyAsync(ur => ur.IdUsuario == idUsuario && ur.IdRol == idRol && !ur.EsEliminado, cancellationToken);
        }

        public async Task AgregarAsync(UsuarioRolEntity entity, CancellationToken cancellationToken = default)
        {
            await _context.UsuariosRoles.AddAsync(entity, cancellationToken);
        }

        public void Actualizar(UsuarioRolEntity entity)
        {
            _context.UsuariosRoles.Update(entity);
        }

        public void Eliminar(UsuarioRolEntity entity)
        {
            entity.EsEliminado = true;
            _context.UsuariosRoles.Update(entity);
        }
    }
}