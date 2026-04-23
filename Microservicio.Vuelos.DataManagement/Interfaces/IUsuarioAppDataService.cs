using Microservicio.Vuelos.DataManagement.Models;

namespace Microservicio.Vuelos.DataManagement.Interfaces;

public interface IUsuarioAppDataService
{
    Task<DataPagedResult<UsuarioAppDataModel>> GetPagedAsync(UsuarioAppFiltroDataModel filtro);
    Task<UsuarioAppDataModel?> GetByIdAsync(int id);
    Task<UsuarioAppDataModel?> GetByUsernameAsync(string username);
    Task<UsuarioAppDataModel> CreateAsync(UsuarioAppDataModel model);
    Task<UsuarioAppDataModel?> UpdateAsync(UsuarioAppDataModel model);
    Task<bool> DeleteAsync(int id, string modificadoPorUsuario);
}