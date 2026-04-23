using Microservicio.Vuelos.DataAccess.Entities;

namespace Microservicio.Vuelos.DataAccess.Repositories.Interfaces;

public interface IBoletoRepository
{
    Task<IEnumerable<BoletoEntity>> ObtenerTodosAsync(CancellationToken cancellationToken = default);
    Task<BoletoEntity?> ObtenerPorIdAsync(int idBoleto, CancellationToken cancellationToken = default);
    Task<BoletoEntity?> ObtenerPorIdParaEditarAsync(int idBoleto, CancellationToken cancellationToken = default); // ✅ nuevo
    Task<BoletoEntity?> ObtenerPorCodigoAsync(string codigoBoleto, CancellationToken cancellationToken = default);
    Task<IEnumerable<BoletoEntity>> ObtenerPorReservaAsync(int idReserva, CancellationToken cancellationToken = default);
    Task<IEnumerable<BoletoEntity>> ObtenerPorVueloAsync(int idVuelo, CancellationToken cancellationToken = default);
    Task<IEnumerable<BoletoEntity>> ObtenerPorAsientoAsync(int idAsiento, CancellationToken cancellationToken = default);
    Task<IEnumerable<BoletoEntity>> ObtenerPorFacturaAsync(int idFactura, CancellationToken cancellationToken = default);
    Task<bool> ExistePorIdAsync(int idBoleto, CancellationToken cancellationToken = default);
    Task<bool> ExistePorCodigoAsync(string codigoBoleto, CancellationToken cancellationToken = default);
    Task AgregarAsync(BoletoEntity entity, CancellationToken cancellationToken = default);
    void Actualizar(BoletoEntity entity);
    void Eliminar(BoletoEntity entity);
}