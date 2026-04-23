using Microservicio.Vuelos.DataAccess.Repositories.Interfaces;
using Microservicio.Vuelos.DataManagement.Interfaces;
using Microservicio.Vuelos.DataManagement.Mappers;
using Microservicio.Vuelos.DataManagement.Models;

namespace Microservicio.Vuelos.DataManagement.Services;

public class UsuarioAppDataService : IUsuarioAppDataService
{
    private readonly IUsuarioAppRepository _repo;
    private readonly IUnitOfWork _uow;

    public UsuarioAppDataService(IUsuarioAppRepository repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    public async Task<DataPagedResult<UsuarioAppDataModel>> GetPagedAsync(UsuarioAppFiltroDataModel filtro)
    {
        filtro.PageNumber = filtro.PageNumber <= 0 ? 1 : filtro.PageNumber;
        filtro.PageSize = filtro.PageSize <= 0 ? 10 : filtro.PageSize;

        var data = await _repo.ObtenerTodosAsync();
        var query = data.AsQueryable();

        if (!filtro.IncluirEliminados)
            query = query.Where(x => !x.EsEliminado);

        if (!string.IsNullOrWhiteSpace(filtro.Username))
        {
            var username = filtro.Username.Trim().ToUpperInvariant();
            query = query.Where(x => x.Username.Contains(username));
        }

        if (!string.IsNullOrWhiteSpace(filtro.Correo))
        {
            var correo = filtro.Correo.Trim().ToUpperInvariant();
            query = query.Where(x => x.Correo.Contains(correo));
        }

        if (filtro.Activo.HasValue)
            query = query.Where(x => x.Activo == filtro.Activo.Value);

        query = query.OrderBy(x => x.Username);

        var total = query.Count();

        var items = query
            .Skip((filtro.PageNumber - 1) * filtro.PageSize)
            .Take(filtro.PageSize)
            .Select(UsuarioAppDataMapper.ToDataModel)
            .ToList();

        return new DataPagedResult<UsuarioAppDataModel>
        {
            Items = items,
            PageNumber = filtro.PageNumber,
            PageSize = filtro.PageSize,
            TotalRecords = total
        };
    }

    public async Task<UsuarioAppDataModel?> GetByIdAsync(int id)
    {
        var entity = await _repo.ObtenerPorIdAsync(id);

        if (entity == null || entity.EsEliminado)
            return null;

        return UsuarioAppDataMapper.ToDataModel(entity);
    }

    public async Task<UsuarioAppDataModel?> GetByUsernameAsync(string username)
    {
        var entity = await _repo.ObtenerPorUsernameAsync(username);

        if (entity == null || entity.EsEliminado)
            return null;

        return UsuarioAppDataMapper.ToDataModel(entity);
    }

    public async Task<UsuarioAppDataModel> CreateAsync(UsuarioAppDataModel model)
    {
        var entity = UsuarioAppDataMapper.ToEntity(model);

        entity.EsEliminado = false;
        entity.Activo = true;
        entity.EstadoUsuario = "ACT";
        entity.UsuarioGuid = Guid.NewGuid();
        entity.FechaRegistroUtc = DateTime.UtcNow;

        await _repo.AgregarAsync(entity);
        await _uow.SaveChangesAsync();

        return UsuarioAppDataMapper.ToDataModel(entity);
    }

    public async Task<UsuarioAppDataModel?> UpdateAsync(UsuarioAppDataModel model)
    {
        var entity = await _repo.ObtenerPorIdAsync(model.IdUsuario);

        if (entity == null || entity.EsEliminado)
            return null;

        UsuarioAppDataMapper.UpdateEntity(entity, model);

        await _uow.SaveChangesAsync();

        return UsuarioAppDataMapper.ToDataModel(entity);
    }

    public async Task<bool> DeleteAsync(int id, string modificadoPorUsuario)
    {
        var entity = await _repo.ObtenerPorIdAsync(id);

        if (entity == null || entity.EsEliminado)
            return false;

        entity.EsEliminado = true;
        entity.Activo = false;
        entity.EstadoUsuario = "INA";
        entity.ModificadoPorUsuario = modificadoPorUsuario.Trim();
        entity.FechaModificacionUtc = DateTime.UtcNow;

        await _uow.SaveChangesAsync();

        return true;
    }
}