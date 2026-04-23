using System.Linq;
using System.Threading.Tasks;
using Microservicio.Vuelos.Business.DTOs.UsuarioRol;
using Microservicio.Vuelos.Business.Exceptions;
using Microservicio.Vuelos.Business.Interfaces;
using Microservicio.Vuelos.Business.Mappers;
using Microservicio.Vuelos.Business.Validators;
using Microservicio.Vuelos.DataManagement.Interfaces;
using Microservicio.Vuelos.DataManagement.Models;

namespace Microservicio.Vuelos.Business.Services;

public class UsuarioRolService : IUsuarioRolService
{
    private readonly IUsuarioRolDataService _usuarioRolDataService;
    private readonly IUsuarioAppDataService _usuarioAppDataService;
    private readonly IRolDataService _rolDataService;
    private readonly UsuarioRolValidator _validator;

    public UsuarioRolService(
        IUsuarioRolDataService usuarioRolDataService,
        IUsuarioAppDataService usuarioAppDataService,
        IRolDataService rolDataService)
    {
        _usuarioRolDataService = usuarioRolDataService;
        _usuarioAppDataService = usuarioAppDataService;
        _rolDataService = rolDataService;
        _validator = new UsuarioRolValidator();
    }

    public async Task<DataPagedResult<UsuarioRolResponseDto>> GetPagedAsync(UsuarioRolFilterDto filter)
    {
        _validator.ValidateFilter(filter);

        var filtro = UsuarioRolBusinessMapper.ToFiltroDataModel(filter);
        var result = await _usuarioRolDataService.GetPagedAsync(filtro);

        return new DataPagedResult<UsuarioRolResponseDto>
        {
            Items = UsuarioRolBusinessMapper.ToResponseDtoList(result.Items),
            PageNumber = result.PageNumber,
            PageSize = result.PageSize,
            TotalRecords = result.TotalRecords
        };
    }

    public async Task<UsuarioRolResponseDto?> GetByIdAsync(int idUsuarioRol)
    {
        if (idUsuarioRol <= 0)
            throw new ValidationException("El id usuario-rol debe ser mayor que 0.");

        var data = await _usuarioRolDataService.GetByIdAsync(idUsuarioRol);

        return data == null ? null : UsuarioRolBusinessMapper.ToResponseDto(data);
    }

    public async Task<UsuarioRolResponseDto> CreateAsync(UsuarioRolRequestDto request, string creadoPorUsuario)
    {
        if (string.IsNullOrWhiteSpace(creadoPorUsuario))
            throw new UnauthorizedBusinessException("No se pudo identificar el usuario creador.");

        _validator.ValidateRequest(request);

        var usuario = await _usuarioAppDataService.GetByIdAsync(request.IdUsuario);
        if (usuario == null)
            throw new NotFoundException("El usuario indicado no existe.");
        if (usuario.EsEliminado || !usuario.Activo || usuario.EstadoUsuario != "ACT")
            throw new BusinessException("El usuario indicado está inactivo o eliminado.");

        var rol = await _rolDataService.GetByIdAsync(request.IdRol);
        if (rol == null)
            throw new NotFoundException("El rol indicado no existe.");
        if (rol.EsEliminado || !rol.Activo || rol.EstadoRol != "ACT")
            throw new BusinessException("El rol indicado está inactivo o eliminado.");

        var existentes = await _usuarioRolDataService.GetPagedAsync(new UsuarioRolFiltroDataModel
        {
            IdUsuario = request.IdUsuario,
            IdRol = request.IdRol,
            PageNumber = 1,
            PageSize = 10000
        });

        if (existentes.Items.Any(x => x.IdUsuario == request.IdUsuario && x.IdRol == request.IdRol))
            throw new BusinessException("El usuario ya tiene asignado ese rol.");

        var dataModel = UsuarioRolBusinessMapper.ToDataModel(request, creadoPorUsuario);
        var creado = await _usuarioRolDataService.CreateAsync(dataModel);

        return UsuarioRolBusinessMapper.ToResponseDto(creado);
    }

    public async Task<bool> DeleteAsync(int idUsuarioRol, string modificadoPorUsuario)
    {
        if (idUsuarioRol <= 0)
            throw new ValidationException("El id usuario-rol debe ser mayor que 0.");

        if (string.IsNullOrWhiteSpace(modificadoPorUsuario))
            throw new UnauthorizedBusinessException("No se pudo identificar el usuario modificador.");

        var actual = await _usuarioRolDataService.GetByIdAsync(idUsuarioRol);
        if (actual == null)
            throw new NotFoundException("Asignación usuario-rol no encontrada.");

        return await _usuarioRolDataService.DeleteAsync(idUsuarioRol, modificadoPorUsuario);
    }
}