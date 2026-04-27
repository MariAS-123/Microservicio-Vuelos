using Microsoft.EntityFrameworkCore;
using Microservicio.Vuelos.DataAccess.Context;
using Microservicio.Vuelos.DataAccess.Entities;
using Microservicio.Vuelos.DataAccess.Repositories.Interfaces;

namespace Microservicio.Vuelos.DataAccess.Repositories
{
    public class UsuarioAppRepository : IUsuarioAppRepository
    {
        private readonly SistemaVuelosDBContext _context;

        public UsuarioAppRepository(SistemaVuelosDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<UsuarioAppEntity>> ObtenerTodosAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Usuarios
                .AsNoTracking()
                .Where(u => !u.EsEliminado)
                .OrderBy(u => u.Username)
                .ToListAsync(cancellationToken);
        }

        public async Task<UsuarioAppEntity?> ObtenerPorIdAsync(int idUsuario, CancellationToken cancellationToken = default)
        {
            return await _context.Usuarios
                .AsNoTracking()
                .Include(u => u.UsuariosRoles)
                    .ThenInclude(ur => ur.Rol)
                .FirstOrDefaultAsync(u => u.IdUsuario == idUsuario && !u.EsEliminado, cancellationToken);
        }

        public async Task<UsuarioAppEntity?> ObtenerPorGuidAsync(Guid usuarioGuid, CancellationToken cancellationToken = default)
        {
            return await _context.Usuarios
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.UsuarioGuid == usuarioGuid && !u.EsEliminado, cancellationToken);
        }

        public async Task<UsuarioAppEntity?> ObtenerPorUsernameAsync(string username, CancellationToken cancellationToken = default)
        {
            var exactUsername = username.Trim();

            return await _context.Usuarios
                .AsNoTracking()
                .Include(u => u.UsuariosRoles)
                    .ThenInclude(ur => ur.Rol)
                .FirstOrDefaultAsync(
                    u => EF.Functions.Collate(u.Username, "Latin1_General_CS_AS") == exactUsername &&
                         !u.EsEliminado,
                    cancellationToken);
        }

        public async Task<UsuarioAppEntity?> ObtenerPorCorreoAsync(string correo, CancellationToken cancellationToken = default)
        {
            return await _context.Usuarios
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Correo == correo && !u.EsEliminado, cancellationToken);
        }

        public async Task<IEnumerable<UsuarioAppEntity>> ObtenerPorClienteAsync(int idCliente, CancellationToken cancellationToken = default)
        {
            return await _context.Usuarios
                .AsNoTracking()
                .Where(u => u.IdCliente == idCliente && !u.EsEliminado)
                .OrderBy(u => u.Username)
                .ToListAsync(cancellationToken);
        }

        public async Task<bool> ExistePorIdAsync(int idUsuario, CancellationToken cancellationToken = default)
        {
            return await _context.Usuarios
                .AnyAsync(u => u.IdUsuario == idUsuario && !u.EsEliminado, cancellationToken);
        }

        public async Task<bool> ExistePorGuidAsync(Guid usuarioGuid, CancellationToken cancellationToken = default)
        {
            return await _context.Usuarios
                .AnyAsync(u => u.UsuarioGuid == usuarioGuid && !u.EsEliminado, cancellationToken);
        }

        public async Task<bool> ExistePorUsernameAsync(string username, CancellationToken cancellationToken = default)
        {
            return await _context.Usuarios
                .AnyAsync(u => u.Username == username && !u.EsEliminado, cancellationToken);
        }

        public async Task<bool> ExistePorCorreoAsync(string correo, CancellationToken cancellationToken = default)
        {
            return await _context.Usuarios
                .AnyAsync(u => u.Correo == correo && !u.EsEliminado, cancellationToken);
        }

        public async Task AgregarAsync(UsuarioAppEntity entity, CancellationToken cancellationToken = default)
        {
            await _context.Usuarios.AddAsync(entity, cancellationToken);
        }

        public void Actualizar(UsuarioAppEntity entity)
        {
            _context.Usuarios.Update(entity);
        }

        public void Eliminar(UsuarioAppEntity entity)
        {
            entity.EsEliminado = true;
            _context.Usuarios.Update(entity);
        }
    }
}