using Microservicio.Vuelos.DataAccess.Repositories.Interfaces;
using Microservicio.Vuelos.DataManagement.Interfaces;
using Microservicio.Vuelos.DataManagement.Mappers;
using Microservicio.Vuelos.DataManagement.Models;

namespace Microservicio.Vuelos.DataManagement.Services;

public class BoletoDataService : IBoletoDataService
{
    private readonly IBoletoRepository _repo;
    private readonly IUnitOfWork _uow;

    public BoletoDataService(IBoletoRepository repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    public async Task<DataPagedResult<BoletoDataModel>> GetPagedAsync(BoletoFiltroDataModel filtro)
    {
        filtro.PageNumber = filtro.PageNumber <= 0 ? 1 : filtro.PageNumber;
        filtro.PageSize = filtro.PageSize <= 0 ? 10 : filtro.PageSize;

        var data = await _repo.ObtenerTodosAsync();
        var query = data.AsQueryable();

        if (!filtro.IncluirEliminados)
            query = query.Where(x => !x.EsEliminado);

        if (!string.IsNullOrWhiteSpace(filtro.CodigoBoleto))
        {
            var codigo = filtro.CodigoBoleto.Trim().ToUpperInvariant();
            query = query.Where(x => x.CodigoBoleto.Contains(codigo));
        }

        if (filtro.IdReserva.HasValue)
            query = query.Where(x => x.IdReserva == filtro.IdReserva.Value);

        if (filtro.IdVuelo.HasValue)
            query = query.Where(x => x.IdVuelo == filtro.IdVuelo.Value);

        if (!string.IsNullOrWhiteSpace(filtro.EstadoBoleto))
        {
            var estadoBoleto = filtro.EstadoBoleto.Trim().ToUpperInvariant();
            query = query.Where(x => x.EstadoBoleto == estadoBoleto);
        }

        query = query
            .OrderByDescending(x => x.FechaEmision)
            .ThenByDescending(x => x.IdBoleto);

        var total = query.Count();

        var items = query
            .Skip((filtro.PageNumber - 1) * filtro.PageSize)
            .Take(filtro.PageSize)
            .Select(BoletoDataMapper.ToDataModel)
            .ToList();

        return new DataPagedResult<BoletoDataModel>
        {
            Items = items,
            PageNumber = filtro.PageNumber,
            PageSize = filtro.PageSize,
            TotalRecords = total
        };
    }

    public async Task<BoletoDataModel?> GetByIdAsync(int id)
    {
        var entity = await _repo.ObtenerPorIdAsync(id);

        if (entity == null || entity.EsEliminado)
            return null;

        return BoletoDataMapper.ToDataModel(entity);
    }

    public async Task<BoletoDataModel> CreateAsync(BoletoDataModel model)
    {
        var entity = BoletoDataMapper.ToEntity(model);

        entity.EsEliminado = false;
        entity.Estado = "ACTIVO";
        entity.EstadoBoleto = string.IsNullOrWhiteSpace(entity.EstadoBoleto)
            ? "ACTIVO"
            : entity.EstadoBoleto.Trim().ToUpperInvariant();
        entity.FechaEmision = DateTime.UtcNow;
        entity.FechaRegistroUtc = DateTime.UtcNow;

        await _repo.AgregarAsync(entity);
        await _uow.SaveChangesAsync();

        return BoletoDataMapper.ToDataModel(entity);
    }

    public async Task<BoletoDataModel?> UpdateAsync(BoletoDataModel model)
    {
        var entity = await _repo.ObtenerPorIdParaEditarAsync(model.IdBoleto); // ✅ con tracking

        if (entity == null || entity.EsEliminado)
            return null;

        BoletoDataMapper.UpdateEntity(entity, model);
        entity.FechaModificacionUtc = DateTime.UtcNow;

        await _uow.SaveChangesAsync();

        return BoletoDataMapper.ToDataModel(entity);
    }

    public async Task<bool> DeleteAsync(int id, string modificadoPorUsuario)
    {
        var entity = await _repo.ObtenerPorIdParaEditarAsync(id); // ✅ con tracking

        if (entity == null || entity.EsEliminado)
            return false;

        entity.EsEliminado = true;
        entity.Estado = "INACTIVO";
        entity.ModificadoPorUsuario = modificadoPorUsuario.Trim();
        entity.FechaModificacionUtc = DateTime.UtcNow;

        await _uow.SaveChangesAsync();

        return true;
    }
}