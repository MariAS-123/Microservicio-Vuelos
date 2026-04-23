using Microservicio.Vuelos.DataAccess.Entities;

namespace Microservicio.Vuelos.DataAccess.Repositories.Interfaces;

public interface IFacturaRepository
{
    Task<IEnumerable<FacturaEntity>> ObtenerTodosAsync(CancellationToken cancellationToken = default);
    Task<FacturaEntity?> ObtenerPorIdAsync(int idFactura, CancellationToken cancellationToken = default);
    Task<FacturaEntity?> ObtenerPorIdParaEditarAsync(int idFactura, CancellationToken cancellationToken = default); // ✅ nuevo
    Task<FacturaEntity?> ObtenerPorGuidAsync(Guid guidFactura, CancellationToken cancellationToken = default);
    Task<FacturaEntity?> ObtenerPorNumeroAsync(string numeroFactura, CancellationToken cancellationToken = default);
    Task<IEnumerable<FacturaEntity>> ObtenerPorClienteAsync(int idCliente, CancellationToken cancellationToken = default);
    Task<IEnumerable<FacturaEntity>> ObtenerPorReservaAsync(int idReserva, CancellationToken cancellationToken = default);
    Task<bool> ExistePorIdAsync(int idFactura, CancellationToken cancellationToken = default);
    Task<bool> ExistePorGuidAsync(Guid guidFactura, CancellationToken cancellationToken = default);
    Task<bool> ExistePorNumeroAsync(string numeroFactura, CancellationToken cancellationToken = default);
    Task AgregarAsync(FacturaEntity entity, CancellationToken cancellationToken = default);
    void Actualizar(FacturaEntity entity);
    void Eliminar(FacturaEntity entity);
}