using Microsoft.EntityFrameworkCore;
using Microservicio.Vuelos.DataAccess.Context;
using Microservicio.Vuelos.DataAccess.Entities;
using Microservicio.Vuelos.DataAccess.Repositories.Interfaces;

namespace Microservicio.Vuelos.DataAccess.Repositories;

public class EquipajeRepository : IEquipajeRepository
{
    private readonly SistemaVuelosDBContext _context;

    public EquipajeRepository(SistemaVuelosDBContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<EquipajeEntity>> ObtenerTodosAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Equipajes
            .AsNoTracking()
            .Where(e => !e.EsEliminado)
            .OrderByDescending(e => e.IdEquipaje)
            .ToListAsync(cancellationToken);
    }

    public async Task<EquipajeEntity?> ObtenerPorIdAsync(int idEquipaje, CancellationToken cancellationToken = default)
    {
        return await _context.Equipajes
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.IdEquipaje == idEquipaje && !e.EsEliminado, cancellationToken);
    }

    // ✅ Nuevo — sin AsNoTracking para que EF rastree los cambios
    public async Task<EquipajeEntity?> ObtenerPorIdParaEditarAsync(int idEquipaje, CancellationToken cancellationToken = default)
    {
        return await _context.Equipajes
            .FirstOrDefaultAsync(e => e.IdEquipaje == idEquipaje && !e.EsEliminado, cancellationToken);
    }

    public async Task<IEnumerable<EquipajeEntity>> ObtenerPorBoletoAsync(int idBoleto, CancellationToken cancellationToken = default)
    {
        return await _context.Equipajes
            .AsNoTracking()
            .Where(e => e.IdBoleto == idBoleto && !e.EsEliminado)
            .OrderByDescending(e => e.IdEquipaje)
            .ToListAsync(cancellationToken);
    }

    public async Task<EquipajeEntity?> ObtenerPorNumeroEtiquetaAsync(string numeroEtiqueta, CancellationToken cancellationToken = default)
    {
        return await _context.Equipajes
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.NumeroEtiqueta == numeroEtiqueta && !e.EsEliminado, cancellationToken);
    }

    public async Task<IEnumerable<EquipajeEntity>> ObtenerPorTipoAsync(string tipo, CancellationToken cancellationToken = default)
    {
        return await _context.Equipajes
            .AsNoTracking()
            .Where(e => e.Tipo == tipo && !e.EsEliminado)
            .OrderByDescending(e => e.IdEquipaje)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> ExistePorIdAsync(int idEquipaje, CancellationToken cancellationToken = default)
    {
        return await _context.Equipajes
            .AnyAsync(e => e.IdEquipaje == idEquipaje && !e.EsEliminado, cancellationToken);
    }

    public async Task<bool> ExistePorNumeroEtiquetaAsync(string numeroEtiqueta, CancellationToken cancellationToken = default)
    {
        return await _context.Equipajes
            .AnyAsync(e => e.NumeroEtiqueta == numeroEtiqueta && !e.EsEliminado, cancellationToken);
    }

    public async Task AgregarAsync(EquipajeEntity entity, CancellationToken cancellationToken = default)
    {
        await _context.Equipajes.AddAsync(entity, cancellationToken);
    }

    public void Actualizar(EquipajeEntity entity)
    {
        _context.Equipajes.Update(entity);
    }

    public void Eliminar(EquipajeEntity entity)
    {
        entity.EsEliminado = true;
        _context.Equipajes.Update(entity);
    }
}