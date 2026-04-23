using Microservicio.Vuelos.DataAccess.Entities;

namespace Microservicio.Vuelos.DataAccess.Repositories.Interfaces;

public interface IEquipajeRepository
{
    Task<IEnumerable<EquipajeEntity>> ObtenerTodosAsync(CancellationToken cancellationToken = default);
    Task<EquipajeEntity?> ObtenerPorIdAsync(int idEquipaje, CancellationToken cancellationToken = default);
    Task<EquipajeEntity?> ObtenerPorIdParaEditarAsync(int idEquipaje, CancellationToken cancellationToken = default); // ✅ nuevo
    Task<IEnumerable<EquipajeEntity>> ObtenerPorBoletoAsync(int idBoleto, CancellationToken cancellationToken = default);
    Task<EquipajeEntity?> ObtenerPorNumeroEtiquetaAsync(string numeroEtiqueta, CancellationToken cancellationToken = default);
    Task<IEnumerable<EquipajeEntity>> ObtenerPorTipoAsync(string tipo, CancellationToken cancellationToken = default);
    Task<bool> ExistePorIdAsync(int idEquipaje, CancellationToken cancellationToken = default);
    Task<bool> ExistePorNumeroEtiquetaAsync(string numeroEtiqueta, CancellationToken cancellationToken = default);
    Task AgregarAsync(EquipajeEntity entity, CancellationToken cancellationToken = default);
    void Actualizar(EquipajeEntity entity);
    void Eliminar(EquipajeEntity entity);
}