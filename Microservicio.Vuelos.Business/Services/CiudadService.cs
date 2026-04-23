using System;
using System.Linq;
using System.Threading.Tasks;
using Microservicio.Vuelos.Business.DTOs.Ciudad;
using Microservicio.Vuelos.Business.Exceptions;
using Microservicio.Vuelos.Business.Interfaces;
using Microservicio.Vuelos.Business.Mappers;
using Microservicio.Vuelos.Business.Validators;
using Microservicio.Vuelos.DataManagement.Interfaces;
using Microservicio.Vuelos.DataManagement.Models;

namespace Microservicio.Vuelos.Business.Services;

public class CiudadService : ICiudadService
{
    private readonly ICiudadDataService _ciudadDataService;
    private readonly IPaisDataService _paisDataService;
    private readonly CiudadValidator _validator;

    public CiudadService(ICiudadDataService ciudadDataService, IPaisDataService paisDataService)
    {
        _ciudadDataService = ciudadDataService;
        _paisDataService = paisDataService;
        _validator = new CiudadValidator();
    }

    public async Task<DataPagedResult<CiudadResponseDto>> GetPagedAsync(CiudadFilterDto filter)
    {
        _validator.ValidateFilter(filter);

        var filtro = CiudadBusinessMapper.ToFiltroDataModel(filter);
        var result = await _ciudadDataService.GetPagedAsync(filtro);

        return new DataPagedResult<CiudadResponseDto>
        {
            Items = CiudadBusinessMapper.ToResponseDtoList(result.Items),
            PageNumber = result.PageNumber,
            PageSize = result.PageSize,
            TotalRecords = result.TotalRecords
        };
    }

    public async Task<CiudadResponseDto?> GetByIdAsync(int idCiudad)
    {
        if (idCiudad <= 0)
            throw new ValidationException("El id de la ciudad debe ser mayor que 0.");

        var data = await _ciudadDataService.GetByIdAsync(idCiudad);

        return data == null ? null : CiudadBusinessMapper.ToResponseDto(data);
    }

    public async Task<CiudadResponseDto> CreateAsync(CiudadRequestDto request, string creadoPorUsuario)
    {
        if (string.IsNullOrWhiteSpace(creadoPorUsuario))
            throw new UnauthorizedBusinessException("No se pudo identificar el usuario creador.");

        _validator.ValidateRequest(request);

        var pais = await _paisDataService.GetByIdAsync(request.IdPais);
        if (pais == null)
            throw new NotFoundException("El país indicado no existe.");
        if (pais.Eliminado || pais.Estado != "ACTIVO")
            throw new BusinessException("El país indicado está inactivo o eliminado.");

        var existentes = await _ciudadDataService.GetPagedAsync(new CiudadFiltroDataModel
        {
            IdPais = request.IdPais,
            PageNumber = 1,
            PageSize = 10000
        });

        var nombre = request.Nombre.Trim().ToUpperInvariant();

        if (existentes.Items.Any(x => x.Nombre.Trim().ToUpperInvariant() == nombre))
            throw new BusinessException("Ya existe una ciudad con el mismo nombre en el país indicado.");

        var dataModel = CiudadBusinessMapper.ToDataModel(request, creadoPorUsuario);
        var creada = await _ciudadDataService.CreateAsync(dataModel);

        return CiudadBusinessMapper.ToResponseDto(creada);
    }

    public async Task<CiudadResponseDto?> UpdateAsync(int idCiudad, CiudadUpdateRequestDto request, string modificadoPorUsuario)
    {
        if (idCiudad <= 0)
            throw new ValidationException("El id de la ciudad debe ser mayor que 0.");

        if (string.IsNullOrWhiteSpace(modificadoPorUsuario))
            throw new UnauthorizedBusinessException("No se pudo identificar el usuario modificador.");

        _validator.ValidateUpdate(request);

        var actual = await _ciudadDataService.GetByIdAsync(idCiudad);
        if (actual == null)
            throw new NotFoundException("Ciudad no encontrada.");

        var pais = await _paisDataService.GetByIdAsync(request.IdPais);
        if (pais == null)
            throw new NotFoundException("El país indicado no existe.");
        if (pais.Eliminado || pais.Estado != "ACTIVO")
            throw new BusinessException("El país indicado está inactivo o eliminado.");

        var existentes = await _ciudadDataService.GetPagedAsync(new CiudadFiltroDataModel
        {
            IdPais = request.IdPais,
            PageNumber = 1,
            PageSize = 10000
        });

        var nombre = request.Nombre.Trim().ToUpperInvariant();

        if (existentes.Items.Any(x => x.IdCiudad != idCiudad && x.Nombre.Trim().ToUpperInvariant() == nombre))
            throw new BusinessException("Ya existe otra ciudad con el mismo nombre en el país indicado.");

        var dataModel = CiudadBusinessMapper.ToDataModel(idCiudad, request);
        dataModel.ModificadoPorUsuario = modificadoPorUsuario;

        var actualizada = await _ciudadDataService.UpdateAsync(dataModel);

        return actualizada == null ? null : CiudadBusinessMapper.ToResponseDto(actualizada);
    }

    public async Task<bool> DeleteAsync(int idCiudad, string modificadoPorUsuario)
    {
        if (idCiudad <= 0)
            throw new ValidationException("El id de la ciudad debe ser mayor que 0.");

        if (string.IsNullOrWhiteSpace(modificadoPorUsuario))
            throw new UnauthorizedBusinessException("No se pudo identificar el usuario modificador.");

        var actual = await _ciudadDataService.GetByIdAsync(idCiudad);
        if (actual == null)
            throw new NotFoundException("Ciudad no encontrada.");

        return await _ciudadDataService.DeleteAsync(idCiudad, modificadoPorUsuario);
    }
}