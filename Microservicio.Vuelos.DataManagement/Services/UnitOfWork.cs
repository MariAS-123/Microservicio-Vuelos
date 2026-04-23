using Microservicio.Vuelos.DataAccess.Context;
using Microservicio.Vuelos.DataManagement.Interfaces;

namespace Microservicio.Vuelos.DataManagement.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly SistemaVuelosDBContext _context;

    public UnitOfWork(SistemaVuelosDBContext context)
    {
        _context = context;
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }
}