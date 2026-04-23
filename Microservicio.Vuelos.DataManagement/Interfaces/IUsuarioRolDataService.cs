using Microservicio.Vuelos.DataManagement.Models;

namespace Microservicio.Vuelos.DataManagement.Interfaces;

public interface IUsuarioRolDataService
{
    Task<DataPagedResult<UsuarioRolDataModel>> GetPagedAsync(UsuarioRolFiltroDataModel filtro);

    Task<UsuarioRolDataModel?> GetByIdAsync(int id);

    Task<UsuarioRolDataModel?> GetByUsuarioRolAsync(int idUsuario, int idRol);

    Task<UsuarioRolDataModel> CreateAsync(UsuarioRolDataModel model);

    Task<UsuarioRolDataModel?> UpdateAsync(UsuarioRolDataModel model);

    Task<bool> DeleteAsync(int id, string modificadoPorUsuario);
}