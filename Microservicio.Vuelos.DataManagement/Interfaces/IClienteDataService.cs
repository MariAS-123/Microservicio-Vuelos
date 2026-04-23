using Microservicio.Vuelos.DataManagement.Models;

namespace Microservicio.Vuelos.DataManagement.Interfaces;

public interface IClienteDataService
{
    Task<DataPagedResult<ClienteDataModel>> GetPagedAsync(ClienteFiltroDataModel filtro);
    Task<ClienteDataModel?> GetByIdAsync(int id);
    Task<ClienteDataModel> CreateAsync(ClienteDataModel model);
    Task<ClienteDataModel?> UpdateAsync(ClienteDataModel model);
    Task<bool> DeleteAsync(int id, string modificadoPorUsuario);
}