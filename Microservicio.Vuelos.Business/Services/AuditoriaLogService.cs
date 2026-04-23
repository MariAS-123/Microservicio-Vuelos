using System;
using System.Threading.Tasks;
using Microservicio.Vuelos.Business.DTOs.AuditoriaLog;
using Microservicio.Vuelos.Business.Exceptions;
using Microservicio.Vuelos.Business.Interfaces;
using Microservicio.Vuelos.Business.Mappers;
using Microservicio.Vuelos.Business.Validators;
using Microservicio.Vuelos.DataManagement.Interfaces;
using Microservicio.Vuelos.DataManagement.Models;

namespace Microservicio.Vuelos.Business.Services;

public class AuditoriaLogService : IAuditoriaLogService
{
    private readonly IAuditoriaLogDataService _auditoriaLogDataService;
    private readonly AuditoriaLogValidator _validator;

    public AuditoriaLogService(IAuditoriaLogDataService auditoriaLogDataService)
    {
        _auditoriaLogDataService = auditoriaLogDataService;
        _validator = new AuditoriaLogValidator();
    }

    public async Task<DataPagedResult<AuditoriaLogResponseDto>> GetPagedAsync(AuditoriaLogFilterDto filter)
    {
        _validator.ValidateFilter(filter);

        var filtro = AuditoriaLogBusinessMapper.ToFiltroDataModel(filter);
        var result = await _auditoriaLogDataService.GetPagedAsync(filtro);

        return new DataPagedResult<AuditoriaLogResponseDto>
        {
            Items = AuditoriaLogBusinessMapper.ToResponseDtoList(result.Items),
            PageNumber = result.PageNumber,
            PageSize = result.PageSize,
            TotalRecords = result.TotalRecords
        };
    }

    public async Task<AuditoriaLogResponseDto?> GetByIdAsync(long idAuditoria)
    {
        if (idAuditoria <= 0)
            throw new ValidationException("El id de auditoría debe ser mayor que 0.");

        var data = await _auditoriaLogDataService.GetByIdAsync(idAuditoria);

        return data == null ? null : AuditoriaLogBusinessMapper.ToResponseDto(data);
    }

    public async Task<AuditoriaLogResponseDto> CreateAsync(AuditoriaLogRequestDto request)
    {
        _validator.ValidateRequest(request);

        var dataModel = AuditoriaLogBusinessMapper.ToDataModel(request);
        var creado = await _auditoriaLogDataService.CreateAsync(dataModel);

        return AuditoriaLogBusinessMapper.ToResponseDto(creado);
    }

    public async Task<AuditoriaLogResponseDto?> UpdateAsync(long idAuditoria, AuditoriaLogUpdateRequestDto request)
    {
        if (idAuditoria <= 0)
            throw new ValidationException("El id de auditoría debe ser mayor que 0.");

        _validator.ValidateUpdate(request);

        var actual = await _auditoriaLogDataService.GetByIdAsync(idAuditoria);
        if (actual == null)
            throw new NotFoundException("Registro de auditoría no encontrado.");

        var dataModel = AuditoriaLogBusinessMapper.ToDataModel(idAuditoria, request);
        dataModel.TablaAfectada = actual.TablaAfectada;
        dataModel.Operacion = actual.Operacion;
        dataModel.IdRegistroAfectado = actual.IdRegistroAfectado;
        dataModel.UsuarioEjecutor = actual.UsuarioEjecutor;
        dataModel.IpOrigen = actual.IpOrigen;
        dataModel.Activo = actual.Activo;

        var actualizado = await _auditoriaLogDataService.UpdateAsync(dataModel);

        return actualizado == null ? null : AuditoriaLogBusinessMapper.ToResponseDto(actualizado);
    }

    public async Task<bool> DeleteAsync(long idAuditoria)
    {
        if (idAuditoria <= 0)
            throw new ValidationException("El id de auditoría debe ser mayor que 0.");

        var actual = await _auditoriaLogDataService.GetByIdAsync(idAuditoria);
        if (actual == null)
            throw new NotFoundException("Registro de auditoría no encontrado.");

        throw new BusinessException("No está permitido eliminar registros de auditoría.");
    }
}