using Microservicio.Vuelos.DataManagement.Models;

namespace Microservicio.Vuelos.DataManagement.Interfaces;

public interface IPasajeroDataService
{
    Task<DataPagedResult<PasajeroDataModel>> GetPagedAsync(PasajeroFiltroDataModel filtro);

    Task<PasajeroDataModel?> GetByIdAsync(int id);

    Task<PasajeroDataModel> CreateAsync(PasajeroDataModel model);

    Task<PasajeroDataModel?> UpdateAsync(PasajeroDataModel model);

    Task<bool> DeleteAsync(int id, string modificadoPorUsuario);
}