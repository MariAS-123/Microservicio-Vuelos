using Microservicio.Vuelos.DataManagement.Models;

namespace Microservicio.Vuelos.DataManagement.Interfaces;

public interface IAeropuertoDataService
{
    Task<DataPagedResult<AeropuertoDataModel>> GetPagedAsync(AeropuertoFiltroDataModel filtro);
    Task<AeropuertoDataModel?> GetByIdAsync(int id);
    Task<AeropuertoDataModel?> GetByCodigoIataAsync(string codigoIata);
    Task<AeropuertoDataModel> CreateAsync(AeropuertoDataModel model);
    Task<AeropuertoDataModel?> UpdateAsync(AeropuertoDataModel model);
    Task<bool> DeleteAsync(int id, string modificadoPorUsuario);
}