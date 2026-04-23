using Microsoft.EntityFrameworkCore;
using Microservicio.Vuelos.DataAccess.Context;
using Microservicio.Vuelos.DataAccess.Entities;
using Microservicio.Vuelos.DataAccess.Repositories.Interfaces;

namespace Microservicio.Vuelos.DataAccess.Repositories;

public class BoletoRepository : IBoletoRepository
{
    private readonly SistemaVuelosDBContext _context;

    public BoletoRepository(SistemaVuelosDBContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<BoletoEntity>> ObtenerTodosAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Boletos
            .AsNoTracking()
            .Where(b => !b.EsEliminado)
            .OrderByDescending(b => b.FechaEmision)
            .ToListAsync(cancellationToken);
    }

    public async Task<BoletoEntity?> ObtenerPorIdAsync(int idBoleto, CancellationToken cancellationToken = default)
    {
        return await _context.Boletos
            .AsNoTracking()
            .FirstOrDefaultAsync(b => b.IdBoleto == idBoleto && !b.EsEliminado, cancellationToken);
    }

    // ✅ Nuevo — sin AsNoTracking para que EF rastree los cambios
    public async Task<BoletoEntity?> ObtenerPorIdParaEditarAsync(int idBoleto, CancellationToken cancellationToken = default)
    {
        return await _context.Boletos
            .FirstOrDefaultAsync(b => b.IdBoleto == idBoleto && !b.EsEliminado, cancellationToken);
    }

    public async Task<BoletoEntity?> ObtenerPorCodigoAsync(string codigoBoleto, CancellationToken cancellationToken = default)
    {
        return await _context.Boletos
            .AsNoTracking()
            .FirstOrDefaultAsync(b => b.CodigoBoleto == codigoBoleto && !b.EsEliminado, cancellationToken);
    }

    public async Task<IEnumerable<BoletoEntity>> ObtenerPorReservaAsync(int idReserva, CancellationToken cancellationToken = default)
    {
        return await _context.Boletos
            .AsNoTracking()
            .Where(b => b.IdReserva == idReserva && !b.EsEliminado)
            .OrderByDescending(b => b.FechaEmision)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<BoletoEntity>> ObtenerPorVueloAsync(int idVuelo, CancellationToken cancellationToken = default)
    {
        return await _context.Boletos
            .AsNoTracking()
            .Where(b => b.IdVuelo == idVuelo && !b.EsEliminado)
            .OrderByDescending(b => b.FechaEmision)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<BoletoEntity>> ObtenerPorAsientoAsync(int idAsiento, CancellationToken cancellationToken = default)
    {
        return await _context.Boletos
            .AsNoTracking()
            .Where(b => b.IdAsiento == idAsiento && !b.EsEliminado)
            .OrderByDescending(b => b.FechaEmision)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<BoletoEntity>> ObtenerPorFacturaAsync(int idFactura, CancellationToken cancellationToken = default)
    {
        return await _context.Boletos
            .AsNoTracking()
            .Where(b => b.IdFactura == idFactura && !b.EsEliminado)
            .OrderByDescending(b => b.FechaEmision)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> ExistePorIdAsync(int idBoleto, CancellationToken cancellationToken = default)
    {
        return await _context.Boletos
            .AnyAsync(b => b.IdBoleto == idBoleto && !b.EsEliminado, cancellationToken);
    }

    public async Task<bool> ExistePorCodigoAsync(string codigoBoleto, CancellationToken cancellationToken = default)
    {
        return await _context.Boletos
            .AnyAsync(b => b.CodigoBoleto == codigoBoleto && !b.EsEliminado, cancellationToken);
    }

    public async Task AgregarAsync(BoletoEntity entity, CancellationToken cancellationToken = default)
    {
        await _context.Boletos.AddAsync(entity, cancellationToken);
    }

    public void Actualizar(BoletoEntity entity)
    {
        _context.Boletos.Update(entity);
    }

    public void Eliminar(BoletoEntity entity)
    {
        entity.EsEliminado = true;
        _context.Boletos.Update(entity);
    }
}