using Microsoft.EntityFrameworkCore;
using Microservicio.Vuelos.DataAccess.Context;
using Microservicio.Vuelos.DataAccess.Entities;
using Microservicio.Vuelos.DataAccess.Repositories.Interfaces;

namespace Microservicio.Vuelos.DataAccess.Repositories;

public class PasajeroRepository : IPasajeroRepository
{
    private readonly SistemaVuelosDBContext _context;

    public PasajeroRepository(SistemaVuelosDBContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<PasajeroEntity>> ObtenerTodosAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Pasajeros
            .AsNoTracking()
            .Where(p => !p.EsEliminado)
            .OrderBy(p => p.ApellidoPasajero)
            .ThenBy(p => p.NombrePasajero)
            .ToListAsync(cancellationToken);
    }

    public async Task<PasajeroEntity?> ObtenerPorIdAsync(int idPasajero, CancellationToken cancellationToken = default)
    {
        return await _context.Pasajeros
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.IdPasajero == idPasajero && !p.EsEliminado, cancellationToken);
    }

    // ✅ Nuevo — sin AsNoTracking para que EF rastree los cambios
    public async Task<PasajeroEntity?> ObtenerPorIdParaEditarAsync(int idPasajero, CancellationToken cancellationToken = default)
    {
        return await _context.Pasajeros
            .FirstOrDefaultAsync(p => p.IdPasajero == idPasajero && !p.EsEliminado, cancellationToken);
    }

    public async Task<IEnumerable<PasajeroEntity>> ObtenerPorClienteAsync(int idCliente, CancellationToken cancellationToken = default)
    {
        return await _context.Pasajeros
            .AsNoTracking()
            .Where(p => p.IdCliente == idCliente && !p.EsEliminado)
            .OrderBy(p => p.ApellidoPasajero)
            .ThenBy(p => p.NombrePasajero)
            .ToListAsync(cancellationToken);
    }

    public async Task<PasajeroEntity?> ObtenerPorNumeroDocumentoAsync(string numeroDocumento, CancellationToken cancellationToken = default)
    {
        return await _context.Pasajeros
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.NumeroDocumentoPasajero == numeroDocumento && !p.EsEliminado, cancellationToken);
    }

    public async Task<PasajeroEntity?> ObtenerPorTipoYNumeroDocumentoAsync(string tipoDocumento, string numeroDocumento, CancellationToken cancellationToken = default)
    {
        return await _context.Pasajeros
            .AsNoTracking()
            .FirstOrDefaultAsync(p =>
                p.TipoDocumentoPasajero == tipoDocumento &&
                p.NumeroDocumentoPasajero == numeroDocumento &&
                !p.EsEliminado, cancellationToken);
    }

    public async Task<bool> ExistePorIdAsync(int idPasajero, CancellationToken cancellationToken = default)
    {
        return await _context.Pasajeros
            .AnyAsync(p => p.IdPasajero == idPasajero && !p.EsEliminado, cancellationToken);
    }

    public async Task<bool> ExistePorTipoYNumeroDocumentoAsync(string tipoDocumento, string numeroDocumento, CancellationToken cancellationToken = default)
    {
        return await _context.Pasajeros
            .AnyAsync(p =>
                p.TipoDocumentoPasajero == tipoDocumento &&
                p.NumeroDocumentoPasajero == numeroDocumento &&
                !p.EsEliminado, cancellationToken);
    }

    public async Task AgregarAsync(PasajeroEntity entity, CancellationToken cancellationToken = default)
    {
        await _context.Pasajeros.AddAsync(entity, cancellationToken);
    }

    public void Actualizar(PasajeroEntity entity)
    {
        _context.Pasajeros.Update(entity);
    }

    public void Eliminar(PasajeroEntity entity)
    {
        entity.EsEliminado = true;
        _context.Pasajeros.Update(entity);
    }
}