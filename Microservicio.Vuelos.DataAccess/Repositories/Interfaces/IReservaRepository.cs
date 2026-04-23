using Microservicio.Vuelos.DataAccess.Entities;

namespace Microservicio.Vuelos.DataAccess.Repositories.Interfaces;

public interface IReservaRepository
{
    Task<IEnumerable<ReservaEntity>> ObtenerTodosAsync(CancellationToken cancellationToken = default);
    Task<ReservaEntity?> ObtenerPorIdAsync(int idReserva, CancellationToken cancellationToken = default);
    Task<ReservaEntity?> ObtenerPorIdParaEditarAsync(int idReserva, CancellationToken cancellationToken = default); // ✅ nuevo
    Task<ReservaEntity?> ObtenerPorGuidAsync(Guid guidReserva, CancellationToken cancellationToken = default);
    Task<ReservaEntity?> ObtenerPorCodigoAsync(string codigoReserva, CancellationToken cancellationToken = default);
    Task<IEnumerable<ReservaEntity>> ObtenerPorClienteAsync(int idCliente, CancellationToken cancellationToken = default);
    Task<IEnumerable<ReservaEntity>> ObtenerPorPasajeroAsync(int idPasajero, CancellationToken cancellationToken = default);
    Task<IEnumerable<ReservaEntity>> ObtenerPorVueloAsync(int idVuelo, CancellationToken cancellationToken = default);
    Task<ReservaEntity?> ObtenerPorAsientoAsync(int idAsiento, CancellationToken cancellationToken = default);
    Task<ReservaEntity?> ObtenerPorVueloYAsientoAsync(int idVuelo, int idAsiento, CancellationToken cancellationToken = default);
    Task<ReservaEntity?> ObtenerPorVueloYPasajeroAsync(int idVuelo, int idPasajero, CancellationToken cancellationToken = default);
    Task<bool> ExistePorIdAsync(int idReserva, CancellationToken cancellationToken = default);
    Task<bool> ExistePorGuidAsync(Guid guidReserva, CancellationToken cancellationToken = default);
    Task<bool> ExistePorCodigoAsync(string codigoReserva, CancellationToken cancellationToken = default);
    Task<bool> ExistePorVueloYAsientoAsync(int idVuelo, int idAsiento, CancellationToken cancellationToken = default);
    Task<bool> ExistePorVueloYPasajeroAsync(int idVuelo, int idPasajero, CancellationToken cancellationToken = default);
    Task AgregarAsync(ReservaEntity entity, CancellationToken cancellationToken = default);
    void Actualizar(ReservaEntity entity);
    void Eliminar(ReservaEntity entity);
}