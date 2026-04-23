using Microsoft.EntityFrameworkCore;
using Microservicio.Vuelos.DataAccess.Context;
using Microservicio.Vuelos.DataAccess.Entities;
using Microservicio.Vuelos.DataAccess.Repositories.Interfaces;

namespace Microservicio.Vuelos.DataAccess.Repositories;

public class CiudadRepository : ICiudadRepository
{
    private readonly SistemaVuelosDBContext _context;

    public CiudadRepository(SistemaVuelosDBContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<CiudadEntity>> ObtenerTodosAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Ciudades
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<CiudadEntity?> ObtenerPorIdAsync(int idCiudad, CancellationToken cancellationToken = default)
    {
        return await _context.Ciudades
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.IdCiudad == idCiudad, cancellationToken);
    }

    // ✅ Nuevo — sin AsNoTracking para que EF rastree los cambios
    public async Task<CiudadEntity?> ObtenerPorIdParaEditarAsync(int idCiudad, CancellationToken cancellationToken = default)
    {
        return await _context.Ciudades
            .FirstOrDefaultAsync(c => c.IdCiudad == idCiudad, cancellationToken);
    }

    public async Task<IEnumerable<CiudadEntity>> ObtenerPorPaisAsync(int idPais, CancellationToken cancellationToken = default)
    {
        return await _context.Ciudades
            .AsNoTracking()
            .Where(c => c.IdPais == idPais)
            .ToListAsync(cancellationToken);
    }

    public async Task<CiudadEntity?> ObtenerPorPaisYNombreAsync(int idPais, string nombre, CancellationToken cancellationToken = default)
    {
        return await _context.Ciudades
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.IdPais == idPais && c.Nombre == nombre, cancellationToken);
    }

    public async Task<bool> ExistePorIdAsync(int idCiudad, CancellationToken cancellationToken = default)
    {
        return await _context.Ciudades
            .AnyAsync(c => c.IdCiudad == idCiudad, cancellationToken);
    }

    public async Task<bool> ExistePorPaisYNombreAsync(int idPais, string nombre, CancellationToken cancellationToken = default)
    {
        return await _context.Ciudades
            .AnyAsync(c => c.IdPais == idPais && c.Nombre == nombre, cancellationToken);
    }

    public async Task AgregarAsync(CiudadEntity entity, CancellationToken cancellationToken = default)
    {
        await _context.Ciudades.AddAsync(entity, cancellationToken);
    }

    public void Actualizar(CiudadEntity entity)
    {
        _context.Ciudades.Update(entity);
    }

    public void Eliminar(CiudadEntity entity)
    {
        _context.Ciudades.Remove(entity);
    }
}