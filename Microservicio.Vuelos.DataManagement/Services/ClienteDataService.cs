using Microservicio.Vuelos.DataAccess.Repositories.Interfaces;
using Microservicio.Vuelos.DataManagement.Interfaces;
using Microservicio.Vuelos.DataManagement.Mappers;
using Microservicio.Vuelos.DataManagement.Models;

namespace Microservicio.Vuelos.DataManagement.Services;

public class ClienteDataService : IClienteDataService
{
    private readonly IClienteRepository _repo;
    private readonly IUnitOfWork _uow;

    public ClienteDataService(IClienteRepository repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    public async Task<DataPagedResult<ClienteDataModel>> GetPagedAsync(ClienteFiltroDataModel filtro)
    {
        filtro.PageNumber = filtro.PageNumber <= 0 ? 1 : filtro.PageNumber;
        filtro.PageSize = filtro.PageSize <= 0 ? 10 : filtro.PageSize;

        var data = await _repo.ObtenerTodosAsync();
        var query = data.AsQueryable();

        if (!filtro.IncluirEliminados)
            query = query.Where(x => !x.EsEliminado);

        if (!string.IsNullOrWhiteSpace(filtro.Nombres))
        {
            var nombres = filtro.Nombres.Trim();
            query = query.Where(x => x.Nombres.Contains(nombres, StringComparison.OrdinalIgnoreCase));
        }

        if (!string.IsNullOrWhiteSpace(filtro.Apellidos))
        {
            var apellidos = filtro.Apellidos.Trim();
            query = query.Where(x => x.Apellidos != null &&
                                     x.Apellidos.Contains(apellidos, StringComparison.OrdinalIgnoreCase));
        }

        if (!string.IsNullOrWhiteSpace(filtro.TipoIdentificacion))
        {
            var tipo = filtro.TipoIdentificacion.Trim().ToUpperInvariant();
            query = query.Where(x => x.TipoIdentificacion == tipo);
        }

        if (!string.IsNullOrWhiteSpace(filtro.NumeroIdentificacion))
        {
            var numero = filtro.NumeroIdentificacion.Trim();
            query = query.Where(x => x.NumeroIdentificacion.Contains(numero));
        }

        if (!string.IsNullOrWhiteSpace(filtro.Correo))
        {
            var correo = filtro.Correo.Trim().ToLowerInvariant();
            query = query.Where(x => x.Correo.Contains(correo));
        }

        if (filtro.IdPaisNacionalidad.HasValue)
            query = query.Where(x => x.IdPaisNacionalidad == filtro.IdPaisNacionalidad.Value);

        if (filtro.IdCiudadResidencia.HasValue)
            query = query.Where(x => x.IdCiudadResidencia == filtro.IdCiudadResidencia.Value);

        if (!string.IsNullOrWhiteSpace(filtro.Estado))
            query = query.Where(x => x.Estado == filtro.Estado.Trim().ToUpperInvariant());

        query = query.OrderBy(x => x.Nombres).ThenBy(x => x.Apellidos).ThenBy(x => x.IdCliente);

        var total = query.Count();

        var items = query
            .Skip((filtro.PageNumber - 1) * filtro.PageSize)
            .Take(filtro.PageSize)
            .Select(ClienteDataMapper.ToDataModel)
            .ToList();

        return new DataPagedResult<ClienteDataModel>
        {
            Items = items,
            PageNumber = filtro.PageNumber,
            PageSize = filtro.PageSize,
            TotalRecords = total
        };
    }

    public async Task<ClienteDataModel?> GetByIdAsync(int id)
    {
        var entity = await _repo.ObtenerPorIdAsync(id);
        return entity is null || entity.EsEliminado ? null : ClienteDataMapper.ToDataModel(entity);
    }

    public async Task<ClienteDataModel> CreateAsync(ClienteDataModel model)
    {
        var entity = ClienteDataMapper.ToEntity(model);
        entity.EsEliminado = false;
        entity.Estado = "ACT";
        entity.FechaRegistroUtc = DateTime.UtcNow;

        if (entity.ClienteGuid == Guid.Empty)
            entity.ClienteGuid = Guid.NewGuid();

        if (string.IsNullOrWhiteSpace(entity.ServicioOrigen))
            entity.ServicioOrigen = "VUELOS";

        await _repo.AgregarAsync(entity);
        await _uow.SaveChangesAsync();

        return ClienteDataMapper.ToDataModel(entity);
    }

    public async Task<ClienteDataModel?> UpdateAsync(ClienteDataModel model)
    {
        var entity = await _repo.ObtenerPorIdParaEditarAsync(model.IdCliente); // ✅ con tracking
        if (entity is null || entity.EsEliminado)
            return null;

        ClienteDataMapper.UpdateEntity(entity, model);
        entity.FechaModificacionUtc = DateTime.UtcNow;

        await _uow.SaveChangesAsync();

        return ClienteDataMapper.ToDataModel(entity);
    }

    public async Task<bool> DeleteAsync(int id, string modificadoPorUsuario)
    {
        var entity = await _repo.ObtenerPorIdParaEditarAsync(id); // ✅ con tracking
        if (entity is null || entity.EsEliminado)
            return false;

        entity.EsEliminado = true;
        entity.Estado = "INA";
        entity.FechaInhabilitacionUtc = DateTime.UtcNow;
        entity.ModificadoPorUsuario = modificadoPorUsuario.Trim();
        entity.FechaModificacionUtc = DateTime.UtcNow;

        await _uow.SaveChangesAsync();
        return true;
    }
}