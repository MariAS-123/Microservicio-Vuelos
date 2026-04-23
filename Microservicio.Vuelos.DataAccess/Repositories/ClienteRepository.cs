using Microsoft.EntityFrameworkCore;
using Microservicio.Vuelos.DataAccess.Context;
using Microservicio.Vuelos.DataAccess.Entities;
using Microservicio.Vuelos.DataAccess.Repositories.Interfaces;

namespace Microservicio.Vuelos.DataAccess.Repositories;

public class ClienteRepository : IClienteRepository
{
    private readonly SistemaVuelosDBContext _context;

    public ClienteRepository(SistemaVuelosDBContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ClienteEntity>> ObtenerTodosAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Clientes
            .AsNoTracking()
            .Where(c => !c.EsEliminado)
            .OrderBy(c => c.IdCliente)
            .ToListAsync(cancellationToken);
    }

    public async Task<ClienteEntity?> ObtenerPorIdAsync(int idCliente, CancellationToken cancellationToken = default)
    {
        return await _context.Clientes
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.IdCliente == idCliente && !c.EsEliminado, cancellationToken);
    }

    // ✅ Nuevo — sin AsNoTracking para que EF rastree los cambios
    public async Task<ClienteEntity?> ObtenerPorIdParaEditarAsync(int idCliente, CancellationToken cancellationToken = default)
    {
        return await _context.Clientes
            .FirstOrDefaultAsync(c => c.IdCliente == idCliente && !c.EsEliminado, cancellationToken);
    }

    public async Task<ClienteEntity?> ObtenerPorGuidAsync(Guid clienteGuid, CancellationToken cancellationToken = default)
    {
        return await _context.Clientes
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.ClienteGuid == clienteGuid && !c.EsEliminado, cancellationToken);
    }

    public async Task<ClienteEntity?> ObtenerPorNumeroIdentificacionAsync(string numeroIdentificacion, CancellationToken cancellationToken = default)
    {
        return await _context.Clientes
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.NumeroIdentificacion == numeroIdentificacion && !c.EsEliminado, cancellationToken);
    }

    public async Task<ClienteEntity?> ObtenerPorCorreoAsync(string correo, CancellationToken cancellationToken = default)
    {
        return await _context.Clientes
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Correo == correo && !c.EsEliminado, cancellationToken);
    }

    public async Task<IEnumerable<ClienteEntity>> ObtenerPorCiudadResidenciaAsync(int idCiudadResidencia, CancellationToken cancellationToken = default)
    {
        return await _context.Clientes
            .AsNoTracking()
            .Where(c => c.IdCiudadResidencia == idCiudadResidencia && !c.EsEliminado)
            .OrderBy(c => c.IdCliente)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<ClienteEntity>> ObtenerPorPaisNacionalidadAsync(int idPaisNacionalidad, CancellationToken cancellationToken = default)
    {
        return await _context.Clientes
            .AsNoTracking()
            .Where(c => c.IdPaisNacionalidad == idPaisNacionalidad && !c.EsEliminado)
            .OrderBy(c => c.IdCliente)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> ExistePorIdAsync(int idCliente, CancellationToken cancellationToken = default)
    {
        return await _context.Clientes
            .AnyAsync(c => c.IdCliente == idCliente && !c.EsEliminado, cancellationToken);
    }

    public async Task<bool> ExistePorNumeroIdentificacionAsync(string numeroIdentificacion, CancellationToken cancellationToken = default)
    {
        return await _context.Clientes
            .AnyAsync(c => c.NumeroIdentificacion == numeroIdentificacion && !c.EsEliminado, cancellationToken);
    }

    public async Task<bool> ExistePorCorreoAsync(string correo, CancellationToken cancellationToken = default)
    {
        return await _context.Clientes
            .AnyAsync(c => c.Correo == correo && !c.EsEliminado, cancellationToken);
    }

    public async Task AgregarAsync(ClienteEntity entity, CancellationToken cancellationToken = default)
    {
        await _context.Clientes.AddAsync(entity, cancellationToken);
    }

    public void Actualizar(ClienteEntity entity)
    {
        _context.Clientes.Update(entity);
    }

    public void Eliminar(ClienteEntity entity)
    {
        entity.EsEliminado = true;
        _context.Clientes.Update(entity);
    }
}