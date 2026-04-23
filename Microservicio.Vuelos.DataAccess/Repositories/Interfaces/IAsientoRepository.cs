using Microservicio.Vuelos.DataAccess.Entities;

namespace Microservicio.Vuelos.DataAccess.Repositories.Interfaces;

public interface IAsientoRepository
{
    Task<IEnumerable<AsientoEntity>> ObtenerTodosAsync(CancellationToken cancellationToken = default);
    Task<AsientoEntity?> ObtenerPorIdAsync(int idAsiento, CancellationToken cancellationToken = default);
    Task<AsientoEntity?> ObtenerPorIdParaEditarAsync(int idAsiento, CancellationToken cancellationToken = default); // ✅ nuevo
    Task<IEnumerable<AsientoEntity>> ObtenerPorVueloAsync(int idVuelo, CancellationToken cancellationToken = default);
    Task<IEnumerable<AsientoEntity>> ObtenerDisponiblesPorVueloAsync(int idVuelo, CancellationToken cancellationToken = default);
    Task<AsientoEntity?> ObtenerPorVueloYNumeroAsync(int idVuelo, string numeroAsiento, CancellationToken cancellationToken = default);
    Task<bool> ExistePorIdAsync(int idAsiento, CancellationToken cancellationToken = default);
    Task<bool> ExistePorVueloYNumeroAsync(int idVuelo, string numeroAsiento, CancellationToken cancellationToken = default);
    Task AgregarAsync(AsientoEntity entity, CancellationToken cancellationToken = default);
    void Actualizar(AsientoEntity entity);
    void Eliminar(AsientoEntity entity);
}