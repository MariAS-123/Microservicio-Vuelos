using Microservicio.Vuelos.DataAccess.Repositories.Interfaces;
using Microservicio.Vuelos.DataManagement.Interfaces;
using Microservicio.Vuelos.DataManagement.Mappers;
using Microservicio.Vuelos.DataManagement.Models;

namespace Microservicio.Vuelos.DataManagement.Services;

public class FacturaDataService : IFacturaDataService
{
    private readonly IFacturaRepository _repo;
    private readonly IUnitOfWork _uow;

    public FacturaDataService(IFacturaRepository repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    public async Task<DataPagedResult<FacturaDataModel>> GetPagedAsync(FacturaFiltroDataModel filtro)
    {
        filtro.PageNumber = filtro.PageNumber <= 0 ? 1 : filtro.PageNumber;
        filtro.PageSize = filtro.PageSize <= 0 ? 10 : filtro.PageSize;

        var data = await _repo.ObtenerTodosAsync();
        var query = data.AsQueryable();

        if (!filtro.IncluirEliminados)
            query = query.Where(x => !x.EsEliminado);

        if (!string.IsNullOrWhiteSpace(filtro.NumeroFactura))
        {
            var numero = filtro.NumeroFactura.Trim().ToUpperInvariant();
            query = query.Where(x => x.NumeroFactura.Contains(numero));
        }

        if (filtro.IdCliente.HasValue)
            query = query.Where(x => x.IdCliente == filtro.IdCliente.Value);

        if (filtro.IdReserva.HasValue)
            query = query.Where(x => x.IdReserva == filtro.IdReserva.Value);

        if (!string.IsNullOrWhiteSpace(filtro.Estado))
        {
            var estado = filtro.Estado.Trim().ToUpperInvariant();
            query = query.Where(x => x.Estado == estado);
        }

        query = query
            .OrderByDescending(x => x.FechaEmision)
            .ThenByDescending(x => x.IdFactura);

        var total = query.Count();

        var items = query
            .Skip((filtro.PageNumber - 1) * filtro.PageSize)
            .Take(filtro.PageSize)
            .Select(FacturaDataMapper.ToDataModel)
            .ToList();

        return new DataPagedResult<FacturaDataModel>
        {
            Items = items,
            PageNumber = filtro.PageNumber,
            PageSize = filtro.PageSize,
            TotalRecords = total
        };
    }

    public async Task<FacturaDataModel?> GetByIdAsync(int id)
    {
        var entity = await _repo.ObtenerPorIdAsync(id);

        if (entity == null || entity.EsEliminado)
            return null;

        return FacturaDataMapper.ToDataModel(entity);
    }

    public async Task<FacturaDataModel> CreateAsync(FacturaDataModel model)
    {
        var entity = FacturaDataMapper.ToEntity(model);

        entity.EsEliminado = false;
        entity.Estado = "ABI";
        entity.GuidFactura = entity.GuidFactura == Guid.Empty ? Guid.NewGuid() : entity.GuidFactura;
        entity.FechaRegistroUtc = DateTime.UtcNow;
        entity.FechaEmision = DateTime.UtcNow;

        await _repo.AgregarAsync(entity);
        await _uow.SaveChangesAsync();

        return FacturaDataMapper.ToDataModel(entity);
    }

    public async Task<FacturaDataModel?> UpdateAsync(FacturaDataModel model)
    {
        var entity = await _repo.ObtenerPorIdParaEditarAsync(model.IdFactura); // ✅ con tracking

        if (entity == null || entity.EsEliminado)
            return null;

        FacturaDataMapper.UpdateEntity(entity, model);
        entity.FechaModificacionUtc = DateTime.UtcNow;

        await _uow.SaveChangesAsync();

        return FacturaDataMapper.ToDataModel(entity);
    }

    public async Task<bool> DeleteAsync(int id, string modificadoPorUsuario)
    {
        var entity = await _repo.ObtenerPorIdParaEditarAsync(id); // ✅ con tracking

        if (entity == null || entity.EsEliminado)
            return false;

        entity.EsEliminado = true;
        entity.Estado = "INA";
        entity.FechaInhabilitacionUtc = DateTime.UtcNow;
        entity.MotivoInhabilitacion = "Eliminación lógica";
        entity.ModificadoPorUsuario = modificadoPorUsuario.Trim();
        entity.FechaModificacionUtc = DateTime.UtcNow;

        await _uow.SaveChangesAsync();

        return true;
    }
}