using Microservicio.Vuelos.DataAccess.Entities;

namespace Microservicio.Vuelos.DataAccess.Repositories.Interfaces;

public interface ICiudadRepository
{
    Task<IEnumerable<CiudadEntity>> ObtenerTodosAsync(CancellationToken cancellationToken = default);

    Task<CiudadEntity?> ObtenerPorIdAsync(int idCiudad, CancellationToken cancellationToken = default);

    Task<CiudadEntity?> ObtenerPorIdParaEditarAsync(int idCiudad, CancellationToken cancellationToken = default); // ✅ nuevo

    Task<IEnumerable<CiudadEntity>> ObtenerPorPaisAsync(int idPais, CancellationToken cancellationToken = default);

    Task<CiudadEntity?> ObtenerPorPaisYNombreAsync(int idPais, string nombre, CancellationToken cancellationToken = default);

    Task<bool> ExistePorIdAsync(int idCiudad, CancellationToken cancellationToken = default);

    Task<bool> ExistePorPaisYNombreAsync(int idPais, string nombre, CancellationToken cancellationToken = default);

    Task AgregarAsync(CiudadEntity entity, CancellationToken cancellationToken = default);

    void Actualizar(CiudadEntity entity);

    void Eliminar(CiudadEntity entity);
}