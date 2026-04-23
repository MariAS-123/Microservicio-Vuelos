using Microsoft.EntityFrameworkCore;
using Microservicio.Vuelos.DataAccess.Context;
using Microservicio.Vuelos.DataAccess.Entities;
using Microservicio.Vuelos.DataAccess.Repositories.Interfaces;

namespace Microservicio.Vuelos.DataAccess.Repositories
{
    public class AeropuertoRepository : IAeropuertoRepository
    {
        private readonly SistemaVuelosDBContext _context;

        public AeropuertoRepository(SistemaVuelosDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<AeropuertoEntity>> ObtenerTodosAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Aeropuertos
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }

        public async Task<AeropuertoEntity?> ObtenerPorIdAsync(int idAeropuerto, CancellationToken cancellationToken = default)
        {
            return await _context.Aeropuertos
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.IdAeropuerto == idAeropuerto, cancellationToken);
        }

        public async Task<AeropuertoEntity?> ObtenerPorCodigoIataAsync(string codigoIata, CancellationToken cancellationToken = default)
        {
            return await _context.Aeropuertos
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.CodigoIata == codigoIata, cancellationToken);
        }

        public async Task<IEnumerable<AeropuertoEntity>> ObtenerPorPaisAsync(int idPais, CancellationToken cancellationToken = default)
        {
            return await _context.Aeropuertos
                .AsNoTracking()
                .Where(a => a.IdPais == idPais)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<AeropuertoEntity>> ObtenerPorCiudadAsync(int idCiudad, CancellationToken cancellationToken = default)
        {
            return await _context.Aeropuertos
                .AsNoTracking()
                .Where(a => a.IdCiudad == idCiudad)
                .ToListAsync(cancellationToken);
        }

        public async Task<bool> ExistePorIdAsync(int idAeropuerto, CancellationToken cancellationToken = default)
        {
            return await _context.Aeropuertos
                .AnyAsync(a => a.IdAeropuerto == idAeropuerto, cancellationToken);
        }

        public async Task<bool> ExistePorCodigoIataAsync(string codigoIata, CancellationToken cancellationToken = default)
        {
            return await _context.Aeropuertos
                .AnyAsync(a => a.CodigoIata == codigoIata, cancellationToken);
        }

        public async Task AgregarAsync(AeropuertoEntity entity, CancellationToken cancellationToken = default)
        {
            await _context.Aeropuertos.AddAsync(entity, cancellationToken);
        }

        public void Actualizar(AeropuertoEntity entity)
        {
            _context.Aeropuertos.Update(entity);
        }

        public void Eliminar(AeropuertoEntity entity)
        {
            _context.Aeropuertos.Remove(entity);
        }

        public async Task<AeropuertoEntity?> ObtenerPorIdParaEditarAsync(int idAeropuerto, CancellationToken cancellationToken = default)
        {
            return await _context.Aeropuertos
                .FirstOrDefaultAsync(a => a.IdAeropuerto == idAeropuerto, cancellationToken); // sin AsNoTracking
        }
    }
}