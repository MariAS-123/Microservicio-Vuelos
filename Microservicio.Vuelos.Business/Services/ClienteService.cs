using System.Linq;
using System.Threading.Tasks;
using Microservicio.Vuelos.Business.DTOs.Cliente;
using Microservicio.Vuelos.Business.Exceptions;
using Microservicio.Vuelos.Business.Interfaces;
using Microservicio.Vuelos.Business.Mappers;
using Microservicio.Vuelos.Business.Validators;
using Microservicio.Vuelos.DataManagement.Interfaces;
using Microservicio.Vuelos.DataManagement.Models;

namespace Microservicio.Vuelos.Business.Services;

public class ClienteService : IClienteService
{
    private readonly IClienteDataService _clienteDataService;
    private readonly ICiudadDataService _ciudadDataService;
    private readonly IPaisDataService _paisDataService;
    private readonly ClienteValidator _validator;

    public ClienteService(
        IClienteDataService clienteDataService,
        ICiudadDataService ciudadDataService,
        IPaisDataService paisDataService)
    {
        _clienteDataService = clienteDataService;
        _ciudadDataService = ciudadDataService;
        _paisDataService = paisDataService;
        _validator = new ClienteValidator();
    }

    public async Task<DataPagedResult<ClienteResponseDto>> GetPagedAsync(ClienteFilterDto filter)
    {
        _validator.ValidateFilter(filter);

        var filtro = ClienteBusinessMapper.ToFiltroDataModel(filter);
        var result = await _clienteDataService.GetPagedAsync(filtro);

        return new DataPagedResult<ClienteResponseDto>
        {
            Items = ClienteBusinessMapper.ToResponseDtoList(result.Items),
            PageNumber = result.PageNumber,
            PageSize = result.PageSize,
            TotalRecords = result.TotalRecords
        };
    }

    public async Task<ClienteResponseDto?> GetByIdAsync(int idCliente, int? idClienteDelToken, string rolDelToken)
    {
        if (idCliente <= 0)
            throw new ValidationException("El id del cliente debe ser mayor que 0.");

        // ✅ CLIENTE solo puede ver su propio perfil
        if (rolDelToken == "CLIENTE")
        {
            if (idClienteDelToken == null || idClienteDelToken != idCliente)
                throw new UnauthorizedBusinessException("No tienes permiso para ver este cliente.");
        }

        var data = await _clienteDataService.GetByIdAsync(idCliente);

        return data == null ? null : ClienteBusinessMapper.ToResponseDto(data);
    }

    public async Task<ClienteResponseDto> CreateAsync(ClienteRequestDto request, string creadoPorUsuario)
    {
        if (string.IsNullOrWhiteSpace(creadoPorUsuario))
            throw new UnauthorizedBusinessException("No se pudo identificar el usuario creador.");

        _validator.ValidateRequest(request);

        var ciudad = await _ciudadDataService.GetByIdAsync(request.IdCiudadResidencia);
        if (ciudad == null)
            throw new NotFoundException("La ciudad de residencia no existe.");

        var pais = await _paisDataService.GetByIdAsync(request.IdPaisNacionalidad);
        if (pais == null)
            throw new NotFoundException("El país de nacionalidad no existe.");

        var existentes = await _clienteDataService.GetPagedAsync(new ClienteFiltroDataModel
        {
            PageNumber = 1,
            PageSize = 10000
        });

        var numeroIdentificacion = request.NumeroIdentificacion.Trim().ToUpperInvariant();
        var correo = request.Correo.Trim().ToUpperInvariant();

        if (existentes.Items.Any(x => x.NumeroIdentificacion.Trim().ToUpperInvariant() == numeroIdentificacion))
            throw new BusinessException("Ya existe un cliente con el mismo número de identificación.");

        if (existentes.Items.Any(x => x.Correo.Trim().ToUpperInvariant() == correo))
            throw new BusinessException("Ya existe un cliente con el mismo correo.");

        var dataModel = ClienteBusinessMapper.ToDataModel(request, creadoPorUsuario);
        var creado = await _clienteDataService.CreateAsync(dataModel);

        return ClienteBusinessMapper.ToResponseDto(creado);
    }

    public async Task<ClienteResponseDto?> UpdateAsync(int idCliente, ClienteUpdateRequestDto request, string modificadoPorUsuario, int? idClienteDelToken, string rolDelToken)
    {
        if (idCliente <= 0)
            throw new ValidationException("El id del cliente debe ser mayor que 0.");

        if (string.IsNullOrWhiteSpace(modificadoPorUsuario))
            throw new UnauthorizedBusinessException("No se pudo identificar el usuario modificador.");

        // ✅ CLIENTE solo puede modificar su propio perfil
        if (rolDelToken == "CLIENTE")
        {
            if (idClienteDelToken == null || idClienteDelToken != idCliente)
                throw new UnauthorizedBusinessException("No tienes permiso para modificar este cliente.");
        }

        _validator.ValidateUpdate(request);

        var actual = await _clienteDataService.GetByIdAsync(idCliente);
        if (actual == null)
            throw new NotFoundException("Cliente no encontrado.");

        var ciudad = await _ciudadDataService.GetByIdAsync(request.IdCiudadResidencia);
        if (ciudad == null)
            throw new NotFoundException("La ciudad de residencia no existe.");

        var pais = await _paisDataService.GetByIdAsync(request.IdPaisNacionalidad);
        if (pais == null)
            throw new NotFoundException("El país de nacionalidad no existe.");

        var existentes = await _clienteDataService.GetPagedAsync(new ClienteFiltroDataModel
        {
            PageNumber = 1,
            PageSize = 10000
        });

        var numeroIdentificacion = request.NumeroIdentificacion.Trim().ToUpperInvariant();
        var correo = request.Correo.Trim().ToUpperInvariant();

        if (existentes.Items.Any(x => x.IdCliente != idCliente &&
                                      x.NumeroIdentificacion.Trim().ToUpperInvariant() == numeroIdentificacion))
            throw new BusinessException("Ya existe otro cliente con el mismo número de identificación.");

        if (existentes.Items.Any(x => x.IdCliente != idCliente &&
                                      x.Correo.Trim().ToUpperInvariant() == correo))
            throw new BusinessException("Ya existe otro cliente con el mismo correo.");

        var dataModel = ClienteBusinessMapper.ToDataModel(idCliente, request, modificadoPorUsuario);
        var actualizado = await _clienteDataService.UpdateAsync(dataModel);

        return actualizado == null ? null : ClienteBusinessMapper.ToResponseDto(actualizado);
    }

    public async Task<bool> DeleteAsync(int idCliente, string modificadoPorUsuario)
    {
        if (idCliente <= 0)
            throw new ValidationException("El id del cliente debe ser mayor que 0.");

        if (string.IsNullOrWhiteSpace(modificadoPorUsuario))
            throw new UnauthorizedBusinessException("No se pudo identificar el usuario modificador.");

        var actual = await _clienteDataService.GetByIdAsync(idCliente);
        if (actual == null)
            throw new NotFoundException("Cliente no encontrado.");

        return await _clienteDataService.DeleteAsync(idCliente, modificadoPorUsuario);
    }
}