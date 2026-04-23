using Microservicio.Vuelos.DataAccess.Repositories.Interfaces;
using Microservicio.Vuelos.DataManagement.Interfaces;
using Microservicio.Vuelos.DataManagement.Mappers;
using Microservicio.Vuelos.DataManagement.Models;

namespace Microservicio.Vuelos.DataManagement.Services;

public class UsuarioRolDataService : IUsuarioRolDataService
{
    private readonly IUsuarioRolRepository _repo;
    private readonly IUnitOfWork _uow;

    public UsuarioRolDataService(IUsuarioRolRepository repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    public async Task<DataPagedResult<UsuarioRolDataModel>> GetPagedAsync(UsuarioRolFiltroDataModel filtro)
    {
        filtro.PageNumber = filtro.PageNumber <= 0 ? 1 : filtro.PageNumber;
        filtro.PageSize = filtro.PageSize <= 0 ? 10 : filtro.PageSize;

        var data = await _repo.ObtenerTodosAsync();
        var query = data.AsQueryable();

        if (!filtro.IncluirEliminados)
            query = query.Where(x => !x.EsEliminado);

        if (filtro.IdUsuario.HasValue)
            query = query.Where(x => x.IdUsuario == filtro.IdUsuario.Value);

        if (filtro.IdRol.HasValue)
            query = query.Where(x => x.IdRol == filtro.IdRol.Value);

        query = query.OrderBy(x => x.IdUsuarioRol);

        var total = query.Count();

        var items = query
            .Skip((filtro.PageNumber - 1) * filtro.PageSize)
            .Take(filtro.PageSize)
            .Select(UsuarioRolDataMapper.ToDataModel)
            .ToList();

        return new DataPagedResult<UsuarioRolDataModel>
        {
            Items = items,
            PageNumber = filtro.PageNumber,
            PageSize = filtro.PageSize,
            TotalRecords = total
        };
    }

    public async Task<UsuarioRolDataModel?> GetByIdAsync(int id)
    {
        var entity = await _repo.ObtenerPorIdAsync(id);

        if (entity == null || entity.EsEliminado)
            return null;

        return UsuarioRolDataMapper.ToDataModel(entity);
    }

    public async Task<UsuarioRolDataModel?> GetByUsuarioRolAsync(int idUsuario, int idRol)
    {
        var data = await _repo.ObtenerTodosAsync();

        var entity = data.FirstOrDefault(x =>
            !x.EsEliminado &&
            x.IdUsuario == idUsuario &&
            x.IdRol == idRol);

        return entity == null ? null : UsuarioRolDataMapper.ToDataModel(entity);
    }

    public async Task<UsuarioRolDataModel> CreateAsync(UsuarioRolDataModel model)
    {
        // 🔴 VALIDACIÓN DUPLICADO
        var existente = await GetByUsuarioRolAsync(model.IdUsuario, model.IdRol);

        if (existente != null)
            throw new InvalidOperationException("El usuario ya tiene asignado este rol.");

        var entity = UsuarioRolDataMapper.ToEntity(model);

        entity.EsEliminado = false;
        entity.Activo = true;
        entity.EstadoUsuarioRol = "ACT";
        entity.FechaRegistroUtc = DateTime.UtcNow;

        await _repo.AgregarAsync(entity);
        await _uow.SaveChangesAsync();

        return UsuarioRolDataMapper.ToDataModel(entity);
    }

    public async Task<UsuarioRolDataModel?> UpdateAsync(UsuarioRolDataModel model)
    {
        var entity = await _repo.ObtenerPorIdAsync(model.IdUsuarioRol);

        if (entity == null || entity.EsEliminado)
            return null;

        UsuarioRolDataMapper.UpdateEntity(entity, model);

        await _uow.SaveChangesAsync();

        return UsuarioRolDataMapper.ToDataModel(entity);
    }

    public async Task<bool> DeleteAsync(int id, string modificadoPorUsuario)
    {
        var entity = await _repo.ObtenerPorIdAsync(id);

        if (entity == null || entity.EsEliminado)
            return false;

        entity.EsEliminado = true;
        entity.Activo = false;
        entity.EstadoUsuarioRol = "INA";
        entity.ModificadoPorUsuario = modificadoPorUsuario.Trim();
        entity.FechaModificacionUtc = DateTime.UtcNow;

        await _uow.SaveChangesAsync();

        return true;
    }
}