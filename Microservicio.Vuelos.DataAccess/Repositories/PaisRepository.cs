using Microsoft.EntityFrameworkCore;
using Microservicio.Vuelos.DataAccess.Context;
using Microservicio.Vuelos.DataAccess.Entities;
using Microservicio.Vuelos.DataAccess.Repositories.Interfaces;

namespace Microservicio.Vuelos.DataAccess.Repositories;

public class PaisRepository : IPaisRepository
{
    private readonly SistemaVuelosDBContext _context;

    public PaisRepository(SistemaVuelosDBContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<PaisEntity>> ObtenerTodosAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Paises
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<PaisEntity?> ObtenerPorIdAsync(int idPais, CancellationToken cancellationToken = default)
    {
        return await _context.Paises
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.IdPais == idPais, cancellationToken);
    }

    // ✅ Nuevo método — sin AsNoTracking para que EF rastree los cambios
    public async Task<PaisEntity?> ObtenerPorIdParaEditarAsync(int idPais, CancellationToken cancellationToken = default)
    {
        return await _context.Paises
            .FirstOrDefaultAsync(p => p.IdPais == idPais, cancellationToken);
    }

    public async Task<PaisEntity?> ObtenerPorCodigoIso2Async(string codigoIso2, CancellationToken cancellationToken = default)
    {
        return await _context.Paises
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.CodigoIso2 == codigoIso2, cancellationToken);
    }

    public async Task<PaisEntity?> ObtenerPorNombreAsync(string nombre, CancellationToken cancellationToken = default)
    {
        return await _context.Paises
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Nombre == nombre, cancellationToken);
    }

    public async Task<bool> ExistePorIdAsync(int idPais, CancellationToken cancellationToken = default)
    {
        return await _context.Paises
            .AnyAsync(p => p.IdPais == idPais, cancellationToken);
    }

    public async Task<bool> ExistePorCodigoIso2Async(string codigoIso2, CancellationToken cancellationToken = default)
    {
        return await _context.Paises
            .AnyAsync(p => p.CodigoIso2 == codigoIso2, cancellationToken);
    }

    public async Task AgregarAsync(PaisEntity entity, CancellationToken cancellationToken = default)
    {
        await _context.Paises.AddAsync(entity, cancellationToken);
    }

    public void Actualizar(PaisEntity entity)
    {
        _context.Paises.Update(entity);
    }

    public void Eliminar(PaisEntity entity)
    {
        _context.Paises.Remove(entity);
    }
}