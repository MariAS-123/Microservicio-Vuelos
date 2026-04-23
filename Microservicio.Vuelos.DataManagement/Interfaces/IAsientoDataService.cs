using Microservicio.Vuelos.DataManagement.Models;

namespace Microservicio.Vuelos.DataManagement.Interfaces;

public interface IAsientoDataService
{
    Task<DataPagedResult<AsientoDataModel>> GetPagedAsync(AsientoFiltroDataModel filtro);
    Task<AsientoDataModel?> GetByIdAsync(int id);
    Task<IReadOnlyList<AsientoDataModel>> GetByVueloAsync(int idVuelo);
    Task<AsientoDataModel> CreateAsync(AsientoDataModel model);
    Task<AsientoDataModel?> UpdateAsync(AsientoDataModel model);
    Task<bool> DeleteAsync(int id, string modificadoPorUsuario);
}