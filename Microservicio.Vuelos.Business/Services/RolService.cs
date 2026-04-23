using System;
using System.Linq;
using System.Threading.Tasks;
using Microservicio.Vuelos.Business.DTOs.Rol;
using Microservicio.Vuelos.Business.Exceptions;
using Microservicio.Vuelos.Business.Interfaces;
using Microservicio.Vuelos.Business.Mappers;
using Microservicio.Vuelos.Business.Validators;
using Microservicio.Vuelos.DataManagement.Interfaces;
using Microservicio.Vuelos.DataManagement.Models;

namespace Microservicio.Vuelos.Business.Services;

public class RolService : IRolService
{
    private readonly IRolDataService _rolDataService;
    private readonly RolValidator _validator;

    public RolService(IRolDataService rolDataService)
    {
        _rolDataService = rolDataService;
        _validator = new RolValidator();
    }

    public async Task<DataPagedResult<RolResponseDto>> GetPagedAsync(RolFilterDto filter)
    {
        _validator.ValidateFilter(filter);

        var filtro = RolBusinessMapper.ToFiltroDataModel(filter);
        var result = await _rolDataService.GetPagedAsync(filtro);

        return new DataPagedResult<RolResponseDto>
        {
            Items = RolBusinessMapper.ToResponseDtoList(result.Items),
            PageNumber = result.PageNumber,
            PageSize = result.PageSize,
            TotalRecords = result.TotalRecords
        };
    }

    public async Task<RolResponseDto?> GetByIdAsync(int idRol)
    {
        if (idRol <= 0)
            throw new ValidationException("El id del rol debe ser mayor que 0.");

        var data = await _rolDataService.GetByIdAsync(idRol);

        return data == null ? null : RolBusinessMapper.ToResponseDto(data);
    }

    public async Task<RolResponseDto> CreateAsync(RolRequestDto request, string creadoPorUsuario)
    {
        if (string.IsNullOrWhiteSpace(creadoPorUsuario))
            throw new UnauthorizedBusinessException("No se pudo identificar el usuario creador.");

        _validator.ValidateRequest(request);

        var existentes = await _rolDataService.GetPagedAsync(new RolFiltroDataModel
        {
            PageNumber = 1,
            PageSize = 10000
        });

        var nombreRol = request.NombreRol.Trim().ToUpperInvariant();

        if (existentes.Items.Any(x => x.NombreRol.Trim().ToUpperInvariant() == nombreRol))
            throw new BusinessException("Ya existe un rol con el mismo nombre.");

        var dataModel = RolBusinessMapper.ToDataModel(request, creadoPorUsuario);
        var creado = await _rolDataService.CreateAsync(dataModel);

        return RolBusinessMapper.ToResponseDto(creado);
    }

    public async Task<RolResponseDto?> UpdateAsync(int idRol, RolUpdateRequestDto request, string modificadoPorUsuario)
    {
        if (idRol <= 0)
            throw new ValidationException("El id del rol debe ser mayor que 0.");

        if (string.IsNullOrWhiteSpace(modificadoPorUsuario))
            throw new UnauthorizedBusinessException("No se pudo identificar el usuario modificador.");

        _validator.ValidateUpdate(request);

        var actual = await _rolDataService.GetByIdAsync(idRol);
        if (actual == null)
            throw new NotFoundException("Rol no encontrado.");

        var existentes = await _rolDataService.GetPagedAsync(new RolFiltroDataModel
        {
            PageNumber = 1,
            PageSize = 10000
        });

        var nombreRol = request.NombreRol.Trim().ToUpperInvariant();

        if (existentes.Items.Any(x => x.IdRol != idRol &&
                                      x.NombreRol.Trim().ToUpperInvariant() == nombreRol))
        {
            throw new BusinessException("Ya existe otro rol con el mismo nombre.");
        }

        var dataModel = RolBusinessMapper.ToDataModel(idRol, request, modificadoPorUsuario);
        var actualizado = await _rolDataService.UpdateAsync(dataModel);

        return actualizado == null ? null : RolBusinessMapper.ToResponseDto(actualizado);
    }

    public async Task<bool> DeleteAsync(int idRol, string modificadoPorUsuario)
    {
        if (idRol <= 0)
            throw new ValidationException("El id del rol debe ser mayor que 0.");

        if (string.IsNullOrWhiteSpace(modificadoPorUsuario))
            throw new UnauthorizedBusinessException("No se pudo identificar el usuario modificador.");

        var actual = await _rolDataService.GetByIdAsync(idRol);
        if (actual == null)
            throw new NotFoundException("Rol no encontrado.");

        return await _rolDataService.DeleteAsync(idRol, modificadoPorUsuario);
    }
}