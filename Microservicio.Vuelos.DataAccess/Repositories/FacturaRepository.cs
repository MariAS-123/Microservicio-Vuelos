using Microsoft.EntityFrameworkCore;
using Microservicio.Vuelos.DataAccess.Context;
using Microservicio.Vuelos.DataAccess.Entities;
using Microservicio.Vuelos.DataAccess.Repositories.Interfaces;

namespace Microservicio.Vuelos.DataAccess.Repositories;

public class FacturaRepository : IFacturaRepository
{
    private readonly SistemaVuelosDBContext _context;

    public FacturaRepository(SistemaVuelosDBContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<FacturaEntity>> ObtenerTodosAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Facturas
            .AsNoTracking()
            .Where(f => !f.EsEliminado)
            .OrderByDescending(f => f.FechaEmision)
            .ToListAsync(cancellationToken);
    }

    public async Task<FacturaEntity?> ObtenerPorIdAsync(int idFactura, CancellationToken cancellationToken = default)
    {
        return await _context.Facturas
            .AsNoTracking()
            .FirstOrDefaultAsync(f => f.IdFactura == idFactura && !f.EsEliminado, cancellationToken);
    }

    // ✅ Nuevo — sin AsNoTracking para que EF rastree los cambios
    public async Task<FacturaEntity?> ObtenerPorIdParaEditarAsync(int idFactura, CancellationToken cancellationToken = default)
    {
        return await _context.Facturas
            .FirstOrDefaultAsync(f => f.IdFactura == idFactura && !f.EsEliminado, cancellationToken);
    }

    public async Task<FacturaEntity?> ObtenerPorGuidAsync(Guid guidFactura, CancellationToken cancellationToken = default)
    {
        return await _context.Facturas
            .AsNoTracking()
            .FirstOrDefaultAsync(f => f.GuidFactura == guidFactura && !f.EsEliminado, cancellationToken);
    }

    public async Task<FacturaEntity?> ObtenerPorNumeroAsync(string numeroFactura, CancellationToken cancellationToken = default)
    {
        return await _context.Facturas
            .AsNoTracking()
            .FirstOrDefaultAsync(f => f.NumeroFactura == numeroFactura && !f.EsEliminado, cancellationToken);
    }

    public async Task<IEnumerable<FacturaEntity>> ObtenerPorClienteAsync(int idCliente, CancellationToken cancellationToken = default)
    {
        return await _context.Facturas
            .AsNoTracking()
            .Where(f => f.IdCliente == idCliente && !f.EsEliminado)
            .OrderByDescending(f => f.FechaEmision)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<FacturaEntity>> ObtenerPorReservaAsync(int idReserva, CancellationToken cancellationToken = default)
    {
        return await _context.Facturas
            .AsNoTracking()
            .Where(f => f.IdReserva == idReserva && !f.EsEliminado)
            .OrderByDescending(f => f.FechaEmision)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> ExistePorIdAsync(int idFactura, CancellationToken cancellationToken = default)
    {
        return await _context.Facturas
            .AnyAsync(f => f.IdFactura == idFactura && !f.EsEliminado, cancellationToken);
    }

    public async Task<bool> ExistePorGuidAsync(Guid guidFactura, CancellationToken cancellationToken = default)
    {
        return await _context.Facturas
            .AnyAsync(f => f.GuidFactura == guidFactura && !f.EsEliminado, cancellationToken);
    }

    public async Task<bool> ExistePorNumeroAsync(string numeroFactura, CancellationToken cancellationToken = default)
    {
        return await _context.Facturas
            .AnyAsync(f => f.NumeroFactura == numeroFactura && !f.EsEliminado, cancellationToken);
    }

    public async Task AgregarAsync(FacturaEntity entity, CancellationToken cancellationToken = default)
    {
        await _context.Facturas.AddAsync(entity, cancellationToken);
    }

    public void Actualizar(FacturaEntity entity)
    {
        _context.Facturas.Update(entity);
    }

    public void Eliminar(FacturaEntity entity)
    {
        entity.EsEliminado = true;
        _context.Facturas.Update(entity);
    }
}