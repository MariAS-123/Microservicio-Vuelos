using Microservicio.Vuelos.DataAccess.Entities;


namespace Microservicio.Vuelos.DataAccess.Repositories.Interfaces
{
    public interface IAeropuertoRepository
    {
        Task<IEnumerable<AeropuertoEntity>> ObtenerTodosAsync(CancellationToken cancellationToken = default);
        Task<AeropuertoEntity?> ObtenerPorIdAsync(int idAeropuerto, CancellationToken cancellationToken = default);
        Task<AeropuertoEntity?> ObtenerPorCodigoIataAsync(string codigoIata, CancellationToken cancellationToken = default);
        Task<IEnumerable<AeropuertoEntity>> ObtenerPorPaisAsync(int idPais, CancellationToken cancellationToken = default);
        Task<IEnumerable<AeropuertoEntity>> ObtenerPorCiudadAsync(int idCiudad, CancellationToken cancellationToken = default);
        Task<bool> ExistePorIdAsync(int idAeropuerto, CancellationToken cancellationToken = default);
        Task<bool> ExistePorCodigoIataAsync(string codigoIata, CancellationToken cancellationToken = default);
        Task AgregarAsync(AeropuertoEntity entity, CancellationToken cancellationToken = default);
        void Actualizar(AeropuertoEntity entity);
        void Eliminar(AeropuertoEntity entity);

        Task<AeropuertoEntity?> ObtenerPorIdParaEditarAsync(int idAeropuerto, CancellationToken cancellationToken = default);
    }
}