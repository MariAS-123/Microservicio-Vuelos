using Microsoft.EntityFrameworkCore;
using Microservicio.Vuelos.DataAccess.Context;
using Microservicio.Vuelos.DataAccess.Entities;
using Microservicio.Vuelos.DataAccess.Repositories.Interfaces;

namespace Microservicio.Vuelos.DataAccess.Repositories;

public class EscalaRepository : IEscalaRepository
{
    private readonly SistemaVuelosDBContext _context;

    public EscalaRepository(SistemaVuelosDBContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<EscalaEntity>> ObtenerTodosAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Escalas
            .AsNoTracking()
            .Where(e => !e.Eliminado)
            .OrderBy(e => e.IdVuelo)
            .ThenBy(e => e.Orden)
            .ToListAsync(cancellationToken);
    }

    public async Task<EscalaEntity?> ObtenerPorIdAsync(int idEscala, CancellationToken cancellationToken = default)
    {
        return await _context.Escalas
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.IdEscala == idEscala && !e.Eliminado, cancellationToken);
    }

    // ✅ Nuevo — sin AsNoTracking para que EF rastree los cambios
    public async Task<EscalaEntity?> ObtenerPorIdParaEditarAsync(int idEscala, CancellationToken cancellationToken = default)
    {
        return await _context.Escalas
            .FirstOrDefaultAsync(e => e.IdEscala == idEscala && !e.Eliminado, cancellationToken);
    }

    public async Task<IEnumerable<EscalaEntity>> ObtenerPorVueloAsync(int idVuelo, CancellationToken cancellationToken = default)
    {
        return await _context.Escalas
            .AsNoTracking()
            .Where(e => e.IdVuelo == idVuelo && !e.Eliminado)
            .OrderBy(e => e.Orden)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<EscalaEntity>> ObtenerPorAeropuertoAsync(int idAeropuerto, CancellationToken cancellationToken = default)
    {
        return await _context.Escalas
            .AsNoTracking()
            .Where(e => e.IdAeropuerto == idAeropuerto && !e.Eliminado)
            .OrderBy(e => e.IdVuelo)
            .ThenBy(e => e.Orden)
            .ToListAsync(cancellationToken);
    }

    public async Task<EscalaEntity?> ObtenerPorVueloYOrdenAsync(int idVuelo, int orden, CancellationToken cancellationToken = default)
    {
        return await _context.Escalas
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.IdVuelo == idVuelo && e.Orden == orden && !e.Eliminado, cancellationToken);
    }

    public async Task<bool> ExistePorIdAsync(int idEscala, CancellationToken cancellationToken = default)
    {
        return await _context.Escalas
            .AnyAsync(e => e.IdEscala == idEscala && !e.Eliminado, cancellationToken);
    }

    public async Task<bool> ExistePorVueloYOrdenAsync(int idVuelo, int orden, CancellationToken cancellationToken = default)
    {
        return await _context.Escalas
            .AnyAsync(e => e.IdVuelo == idVuelo && e.Orden == orden && !e.Eliminado, cancellationToken);
    }

    public async Task AgregarAsync(EscalaEntity entity, CancellationToken cancellationToken = default)
    {
        await _context.Escalas.AddAsync(entity, cancellationToken);
    }

    public void Actualizar(EscalaEntity entity)
    {
        _context.Escalas.Update(entity);
    }

    public void Eliminar(EscalaEntity entity)
    {
        entity.Eliminado = true;
        _context.Escalas.Update(entity);
    }
}