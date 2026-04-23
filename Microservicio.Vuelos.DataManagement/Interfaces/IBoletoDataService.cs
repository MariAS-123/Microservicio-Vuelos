using Microservicio.Vuelos.DataManagement.Models;

namespace Microservicio.Vuelos.DataManagement.Interfaces;

public interface IBoletoDataService
{
    Task<DataPagedResult<BoletoDataModel>> GetPagedAsync(BoletoFiltroDataModel filtro);

    Task<BoletoDataModel?> GetByIdAsync(int id);

    Task<BoletoDataModel> CreateAsync(BoletoDataModel model);

    Task<BoletoDataModel?> UpdateAsync(BoletoDataModel model);

    Task<bool> DeleteAsync(int id, string modificadoPorUsuario);
}