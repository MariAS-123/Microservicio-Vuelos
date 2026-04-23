using Microservicio.Vuelos.DataManagement.Models;

namespace Microservicio.Vuelos.DataManagement.Interfaces;

public interface IEscalaDataService
{
    Task<DataPagedResult<EscalaDataModel>> GetPagedAsync(EscalaFiltroDataModel filtro);
    Task<EscalaDataModel?> GetByIdAsync(int id);
    Task<EscalaDataModel> CreateAsync(EscalaDataModel model);
    Task<EscalaDataModel?> UpdateAsync(EscalaDataModel model);
    Task<bool> DeleteAsync(int id, string modificadoPorUsuario);
}