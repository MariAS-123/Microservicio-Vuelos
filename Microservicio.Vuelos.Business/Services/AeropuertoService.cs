using System;
using System.Linq;
using System.Threading.Tasks;
using Microservicio.Vuelos.Business.DTOs.Aeropuerto;
using Microservicio.Vuelos.Business.Exceptions;
using Microservicio.Vuelos.Business.Interfaces;
using Microservicio.Vuelos.Business.Mappers;
using Microservicio.Vuelos.Business.Validators;
using Microservicio.Vuelos.DataManagement.Interfaces;
using Microservicio.Vuelos.DataManagement.Models;

namespace Microservicio.Vuelos.Business.Services;

public class AeropuertoService : IAeropuertoService
{
    private readonly IAeropuertoDataService _aeropuertoDataService;
    private readonly ICiudadDataService _ciudadDataService;
    private readonly IPaisDataService _paisDataService;
    private readonly AeropuertoValidator _validator;

    public AeropuertoService(
        IAeropuertoDataService aeropuertoDataService,
        ICiudadDataService ciudadDataService,
        IPaisDataService paisDataService)
    {
        _aeropuertoDataService = aeropuertoDataService;
        _ciudadDataService = ciudadDataService;
        _paisDataService = paisDataService;
        _validator = new AeropuertoValidator();
    }

    public async Task<DataPagedResult<AeropuertoResponseDto>> GetPagedAsync(AeropuertoFilterDto filter)
    {
        _validator.ValidateFilter(filter);

        var filtro = AeropuertoBusinessMapper.ToFiltroDataModel(filter);
        var result = await _aeropuertoDataService.GetPagedAsync(filtro);

        return new DataPagedResult<AeropuertoResponseDto>
        {
            Items = AeropuertoBusinessMapper.ToResponseDtoList(result.Items),
            PageNumber = result.PageNumber,
            PageSize = result.PageSize,
            TotalRecords = result.TotalRecords
        };
    }

    public async Task<AeropuertoResponseDto?> GetByIdAsync(int idAeropuerto)
    {
        if (idAeropuerto <= 0)
            throw new ValidationException("El id del aeropuerto debe ser mayor que 0.");

        var data = await _aeropuertoDataService.GetByIdAsync(idAeropuerto);

        return data == null ? null : AeropuertoBusinessMapper.ToResponseDto(data);
    }

    public async Task<AeropuertoResponseDto> CreateAsync(AeropuertoRequestDto request, string creadoPorUsuario)
    {
        if (string.IsNullOrWhiteSpace(creadoPorUsuario))
            throw new UnauthorizedBusinessException("No se pudo identificar el usuario creador.");

        _validator.ValidateRequest(request);

        var pais = await _paisDataService.GetByIdAsync(request.IdPais);
        if (pais == null)
            throw new NotFoundException("El país indicado no existe.");
        if (pais.Eliminado || pais.Estado != "ACTIVO")
            throw new BusinessException("El país indicado está inactivo o eliminado.");

        var ciudad = await _ciudadDataService.GetByIdAsync(request.IdCiudad);
        if (ciudad == null)
            throw new NotFoundException("La ciudad indicada no existe.");
        if (ciudad.Eliminado || ciudad.Estado != "ACTIVO")
            throw new BusinessException("La ciudad indicada está inactiva o eliminada.");
        if (ciudad.IdPais != request.IdPais)
            throw new BusinessException("La ciudad indicada no pertenece al país indicado.");

        var existentes = await _aeropuertoDataService.GetPagedAsync(new AeropuertoFiltroDataModel
        {
            PageNumber = 1,
            PageSize = 10000
        });

        var codigoIata = request.CodigoIata.Trim().ToUpperInvariant();

        if (existentes.Items.Any(x => x.CodigoIata.Trim().ToUpperInvariant() == codigoIata))
            throw new BusinessException("Ya existe un aeropuerto con el mismo código IATA.");

        var dataModel = AeropuertoBusinessMapper.ToDataModel(request, creadoPorUsuario);
        var creado = await _aeropuertoDataService.CreateAsync(dataModel);

        return AeropuertoBusinessMapper.ToResponseDto(creado);
    }

    public async Task<AeropuertoResponseDto?> UpdateAsync(int idAeropuerto, AeropuertoUpdateRequestDto request, string modificadoPorUsuario)
    {
        if (idAeropuerto <= 0)
            throw new ValidationException("El id del aeropuerto debe ser mayor que 0.");

        if (string.IsNullOrWhiteSpace(modificadoPorUsuario))
            throw new UnauthorizedBusinessException("No se pudo identificar el usuario modificador.");

        _validator.ValidateUpdate(request);

        var actual = await _aeropuertoDataService.GetByIdAsync(idAeropuerto);
        if (actual == null)
            throw new NotFoundException("Aeropuerto no encontrado.");

        var pais = await _paisDataService.GetByIdAsync(request.IdPais);
        if (pais == null)
            throw new NotFoundException("El país indicado no existe.");
        if (pais.Eliminado || pais.Estado != "ACTIVO")
            throw new BusinessException("El país indicado está inactivo o eliminado.");

        var ciudad = await _ciudadDataService.GetByIdAsync(request.IdCiudad);
        if (ciudad == null)
            throw new NotFoundException("La ciudad indicada no existe.");
        if (ciudad.Eliminado || ciudad.Estado != "ACTIVO")
            throw new BusinessException("La ciudad indicada está inactiva o eliminada.");
        if (ciudad.IdPais != request.IdPais)
            throw new BusinessException("La ciudad indicada no pertenece al país indicado.");

        var existentes = await _aeropuertoDataService.GetPagedAsync(new AeropuertoFiltroDataModel
        {
            PageNumber = 1,
            PageSize = 10000
        });

        var codigoIata = request.CodigoIata.Trim().ToUpperInvariant();

        if (existentes.Items.Any(x => x.IdAeropuerto != idAeropuerto && x.CodigoIata.Trim().ToUpperInvariant() == codigoIata))
            throw new BusinessException("Ya existe otro aeropuerto con el mismo código IATA.");

        var dataModel = AeropuertoBusinessMapper.ToDataModel(idAeropuerto, request);
        dataModel.ModificadoPorUsuario = modificadoPorUsuario;

        var actualizado = await _aeropuertoDataService.UpdateAsync(dataModel);

        return actualizado == null ? null : AeropuertoBusinessMapper.ToResponseDto(actualizado);
    }

    public async Task<bool> DeleteAsync(int idAeropuerto, string modificadoPorUsuario)
    {
        if (idAeropuerto <= 0)
            throw new ValidationException("El id del aeropuerto debe ser mayor que 0.");

        if (string.IsNullOrWhiteSpace(modificadoPorUsuario))
            throw new UnauthorizedBusinessException("No se pudo identificar el usuario modificador.");

        var actual = await _aeropuertoDataService.GetByIdAsync(idAeropuerto);
        if (actual == null)
            throw new NotFoundException("Aeropuerto no encontrado.");

        return await _aeropuertoDataService.DeleteAsync(idAeropuerto, modificadoPorUsuario);
    }
}