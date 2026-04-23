using Microservicio.Vuelos.DataAccess.Entities;

namespace Microservicio.Vuelos.DataAccess.Repositories.Interfaces;

public interface IPasajeroRepository
{
    Task<IEnumerable<PasajeroEntity>> ObtenerTodosAsync(CancellationToken cancellationToken = default);
    Task<PasajeroEntity?> ObtenerPorIdAsync(int idPasajero, CancellationToken cancellationToken = default);
    Task<PasajeroEntity?> ObtenerPorIdParaEditarAsync(int idPasajero, CancellationToken cancellationToken = default); // ✅ nuevo
    Task<IEnumerable<PasajeroEntity>> ObtenerPorClienteAsync(int idCliente, CancellationToken cancellationToken = default);
    Task<PasajeroEntity?> ObtenerPorNumeroDocumentoAsync(string numeroDocumento, CancellationToken cancellationToken = default);
    Task<PasajeroEntity?> ObtenerPorTipoYNumeroDocumentoAsync(string tipoDocumento, string numeroDocumento, CancellationToken cancellationToken = default);
    Task<bool> ExistePorIdAsync(int idPasajero, CancellationToken cancellationToken = default);
    Task<bool> ExistePorTipoYNumeroDocumentoAsync(string tipoDocumento, string numeroDocumento, CancellationToken cancellationToken = default);
    Task AgregarAsync(PasajeroEntity entity, CancellationToken cancellationToken = default);
    void Actualizar(PasajeroEntity entity);
    void Eliminar(PasajeroEntity entity);
}