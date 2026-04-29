using Microsoft.EntityFrameworkCore;
using Microservicio.Vuelos.DataAccess.Context;
using Microservicio.Vuelos.DataAccess.Entities;
using Microservicio.Vuelos.DataAccess.Repositories.Interfaces;

namespace Microservicio.Vuelos.DataAccess.Repositories;

public class ReservaRepository : IReservaRepository
{
    private readonly SistemaVuelosDBContext _context;

    public ReservaRepository(SistemaVuelosDBContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ReservaEntity>> ObtenerTodosAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Reservas
            .Include(r => r.Detalles)
            .AsNoTracking()
            .Where(r => !r.EsEliminado)
            .OrderByDescending(r => r.FechaReservaUtc)
            .ToListAsync(cancellationToken);
    }

    public async Task<ReservaEntity?> ObtenerPorIdAsync(int idReserva, CancellationToken cancellationToken = default)
    {
        return await _context.Reservas
            .Include(r => r.Detalles)
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.IdReserva == idReserva && !r.EsEliminado, cancellationToken);
    }

    // ✅ Nuevo — sin AsNoTracking para que EF rastree los cambios
    public async Task<ReservaEntity?> ObtenerPorIdParaEditarAsync(int idReserva, CancellationToken cancellationToken = default)
    {
        return await _context.Reservas
            .Include(r => r.Detalles)
            .FirstOrDefaultAsync(r => r.IdReserva == idReserva && !r.EsEliminado, cancellationToken);
    }

    public async Task<ReservaEntity?> ObtenerPorGuidAsync(Guid guidReserva, CancellationToken cancellationToken = default)
    {
        return await _context.Reservas
            .Include(r => r.Detalles)
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.GuidReserva == guidReserva && !r.EsEliminado, cancellationToken);
    }

    public async Task<ReservaEntity?> ObtenerPorCodigoAsync(string codigoReserva, CancellationToken cancellationToken = default)
    {
        return await _context.Reservas
            .Include(r => r.Detalles)
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.CodigoReserva == codigoReserva && !r.EsEliminado, cancellationToken);
    }

    public async Task<IEnumerable<ReservaEntity>> ObtenerPorClienteAsync(int idCliente, CancellationToken cancellationToken = default)
    {
        return await _context.Reservas
            .Include(r => r.Detalles)
            .AsNoTracking()
            .Where(r => r.IdCliente == idCliente && !r.EsEliminado)
            .OrderByDescending(r => r.FechaReservaUtc)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<ReservaEntity>> ObtenerPorPasajeroAsync(int idPasajero, CancellationToken cancellationToken = default)
    {
        return await _context.Reservas
            .Include(r => r.Detalles)
            .AsNoTracking()
            .Where(r => !r.EsEliminado && r.Detalles.Any(d => !d.EsEliminado && d.IdPasajero == idPasajero))
            .OrderByDescending(r => r.FechaReservaUtc)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<ReservaEntity>> ObtenerPorVueloAsync(int idVuelo, CancellationToken cancellationToken = default)
    {
        return await _context.Reservas
            .Include(r => r.Detalles)
            .AsNoTracking()
            .Where(r => r.IdVuelo == idVuelo && !r.EsEliminado)
            .OrderByDescending(r => r.FechaReservaUtc)
            .ToListAsync(cancellationToken);
    }

    public async Task<ReservaEntity?> ObtenerPorAsientoAsync(int idAsiento, CancellationToken cancellationToken = default)
    {
        return await _context.Reservas
            .Include(r => r.Detalles)
            .AsNoTracking()
            .FirstOrDefaultAsync(r => !r.EsEliminado && r.Detalles.Any(d => !d.EsEliminado && d.IdAsiento == idAsiento), cancellationToken);
    }

    public async Task<ReservaEntity?> ObtenerPorVueloYAsientoAsync(int idVuelo, int idAsiento, CancellationToken cancellationToken = default)
    {
        return await _context.Reservas
            .Include(r => r.Detalles)
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.IdVuelo == idVuelo && !r.EsEliminado && r.Detalles.Any(d => !d.EsEliminado && d.IdAsiento == idAsiento), cancellationToken);
    }

    public async Task<ReservaEntity?> ObtenerPorVueloYPasajeroAsync(int idVuelo, int idPasajero, CancellationToken cancellationToken = default)
    {
        return await _context.Reservas
            .Include(r => r.Detalles)
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.IdVuelo == idVuelo && !r.EsEliminado && r.Detalles.Any(d => !d.EsEliminado && d.IdPasajero == idPasajero), cancellationToken);
    }

    public async Task<bool> ExistePorIdAsync(int idReserva, CancellationToken cancellationToken = default)
    {
        return await _context.Reservas
            .AnyAsync(r => r.IdReserva == idReserva && !r.EsEliminado, cancellationToken);
    }

    public async Task<bool> ExistePorGuidAsync(Guid guidReserva, CancellationToken cancellationToken = default)
    {
        return await _context.Reservas
            .AnyAsync(r => r.GuidReserva == guidReserva && !r.EsEliminado, cancellationToken);
    }

    public async Task<bool> ExistePorCodigoAsync(string codigoReserva, CancellationToken cancellationToken = default)
    {
        return await _context.Reservas
            .AnyAsync(r => r.CodigoReserva == codigoReserva && !r.EsEliminado, cancellationToken);
    }

    public async Task<bool> ExistePorVueloYAsientoAsync(int idVuelo, int idAsiento, CancellationToken cancellationToken = default)
    {
        return await _context.Reservas
            .AnyAsync(r => r.IdVuelo == idVuelo && !r.EsEliminado && r.Detalles.Any(d => !d.EsEliminado && d.IdAsiento == idAsiento), cancellationToken);
    }

    public async Task<bool> ExistePorVueloYPasajeroAsync(int idVuelo, int idPasajero, CancellationToken cancellationToken = default)
    {
        return await _context.Reservas
            .AnyAsync(r => r.IdVuelo == idVuelo && !r.EsEliminado && r.Detalles.Any(d => !d.EsEliminado && d.IdPasajero == idPasajero), cancellationToken);
    }

    public async Task AgregarAsync(ReservaEntity entity, CancellationToken cancellationToken = default)
    {
        await _context.Reservas.AddAsync(entity, cancellationToken);
    }

    public void Actualizar(ReservaEntity entity)
    {
        _context.Reservas.Update(entity);
    }

    public void Eliminar(ReservaEntity entity)
    {
        entity.EsEliminado = true;
        _context.Reservas.Update(entity);
    }
}
