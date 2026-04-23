using Microservicio.Vuelos.DataAccess.Repositories.Interfaces;
using Microservicio.Vuelos.DataManagement.Interfaces;
using Microservicio.Vuelos.DataManagement.Mappers;
using Microservicio.Vuelos.DataManagement.Models;

namespace Microservicio.Vuelos.DataManagement.Services;

public class RolDataService : IRolDataService
{
    private readonly IRolRepository _repo;
    private readonly IUnitOfWork _uow;

    public RolDataService(IRolRepository repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    public async Task<DataPagedResult<RolDataModel>> GetPagedAsync(RolFiltroDataModel filtro)
    {
        filtro.PageNumber = filtro.PageNumber <= 0 ? 1 : filtro.PageNumber;
        filtro.PageSize = filtro.PageSize <= 0 ? 10 : filtro.PageSize;

        var data = await _repo.ObtenerTodosAsync();
        var query = data.AsQueryable();

        if (!filtro.IncluirEliminados)
            query = query.Where(x => !x.EsEliminado);

        if (!string.IsNullOrWhiteSpace(filtro.NombreRol))
        {
            var nombreRol = filtro.NombreRol.Trim().ToUpperInvariant();
            query = query.Where(x => x.NombreRol.Contains(nombreRol));
        }

        if (!string.IsNullOrWhiteSpace(filtro.EstadoRol))
        {
            var estadoRol = filtro.EstadoRol.Trim().ToUpperInvariant();
            query = query.Where(x => x.EstadoRol == estadoRol);
        }

        query = query.OrderBy(x => x.NombreRol).ThenBy(x => x.IdRol);

        var total = query.Count();

        var items = query
            .Skip((filtro.PageNumber - 1) * filtro.PageSize)
            .Take(filtro.PageSize)
            .Select(RolDataMapper.ToDataModel)
            .ToList();

        return new DataPagedResult<RolDataModel>
        {
            Items = items,
            PageNumber = filtro.PageNumber,
            PageSize = filtro.PageSize,
            TotalRecords = total
        };
    }

    public async Task<RolDataModel?> GetByIdAsync(int id)
    {
        var entity = await _repo.ObtenerPorIdAsync(id);

        if (entity == null || entity.EsEliminado)
            return null;

        return RolDataMapper.ToDataModel(entity);
    }

    public async Task<RolDataModel?> GetByNombreAsync(string nombre)
    {
        if (string.IsNullOrWhiteSpace(nombre))
            return null;

        var nombreNormalizado = nombre.Trim().ToUpperInvariant();
        var data = await _repo.ObtenerTodosAsync();

        var entity = data.FirstOrDefault(x =>
            !x.EsEliminado &&
            x.NombreRol.ToUpper() == nombreNormalizado);

        return entity == null ? null : RolDataMapper.ToDataModel(entity);
    }

    public async Task<RolDataModel> CreateAsync(RolDataModel model)
    {
        var entity = RolDataMapper.ToEntity(model);

        entity.EsEliminado = false;
        entity.Activo = true;
        entity.EstadoRol = "ACT";
        entity.FechaRegistroUtc = DateTime.UtcNow;

        if (entity.RolGuid == Guid.Empty)
            entity.RolGuid = Guid.NewGuid();

        await _repo.AgregarAsync(entity);
        await _uow.SaveChangesAsync();

        return RolDataMapper.ToDataModel(entity);
    }

    public async Task<RolDataModel?> UpdateAsync(RolDataModel model)
    {
        var entity = await _repo.ObtenerPorIdAsync(model.IdRol);

        if (entity == null || entity.EsEliminado)
            return null;

        RolDataMapper.UpdateEntity(entity, model);

        await _uow.SaveChangesAsync();

        return RolDataMapper.ToDataModel(entity);
    }

    public async Task<bool> DeleteAsync(int id, string modificadoPorUsuario)
    {
        var entity = await _repo.ObtenerPorIdAsync(id);

        if (entity == null || entity.EsEliminado)
            return false;

        entity.EsEliminado = true;
        entity.Activo = false;
        entity.EstadoRol = "INA";
        entity.ModificadoPorUsuario = modificadoPorUsuario.Trim();
        entity.FechaModificacionUtc = DateTime.UtcNow;

        await _uow.SaveChangesAsync();

        return true;
    }
}