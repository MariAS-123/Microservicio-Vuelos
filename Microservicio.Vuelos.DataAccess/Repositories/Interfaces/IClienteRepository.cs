using Microservicio.Vuelos.DataAccess.Entities;

namespace Microservicio.Vuelos.DataAccess.Repositories.Interfaces;

public interface IClienteRepository
{
    Task<IEnumerable<ClienteEntity>> ObtenerTodosAsync(CancellationToken cancellationToken = default);
    Task<ClienteEntity?> ObtenerPorIdAsync(int idCliente, CancellationToken cancellationToken = default);
    Task<ClienteEntity?> ObtenerPorIdParaEditarAsync(int idCliente, CancellationToken cancellationToken = default); // ✅ nuevo
    Task<ClienteEntity?> ObtenerPorGuidAsync(Guid clienteGuid, CancellationToken cancellationToken = default);
    Task<ClienteEntity?> ObtenerPorNumeroIdentificacionAsync(string numeroIdentificacion, CancellationToken cancellationToken = default);
    Task<ClienteEntity?> ObtenerPorCorreoAsync(string correo, CancellationToken cancellationToken = default);
    Task<IEnumerable<ClienteEntity>> ObtenerPorCiudadResidenciaAsync(int idCiudadResidencia, CancellationToken cancellationToken = default);
    Task<IEnumerable<ClienteEntity>> ObtenerPorPaisNacionalidadAsync(int idPaisNacionalidad, CancellationToken cancellationToken = default);
    Task<bool> ExistePorIdAsync(int idCliente, CancellationToken cancellationToken = default);
    Task<bool> ExistePorNumeroIdentificacionAsync(string numeroIdentificacion, CancellationToken cancellationToken = default);
    Task<bool> ExistePorCorreoAsync(string correo, CancellationToken cancellationToken = default);
    Task AgregarAsync(ClienteEntity entity, CancellationToken cancellationToken = default);
    void Actualizar(ClienteEntity entity);
    void Eliminar(ClienteEntity entity);
}