using AutoMapper;
using MccApi.DTOs;
using MccApi.Models;
using MccApi.Repositories;
using MccApi.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MccApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AutorizationsController : ControllerBase
    {
        private readonly IAutorizationRepository _repository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IRolesRepository _rolesRepository;
        private readonly IMapper _mapper;

        public AutorizationsController(
            IAutorizationRepository repository,
            IEmployeeRepository employeeRepository,
            IRolesRepository rolesRepository,
            IMapper mapper)
        {
            _repository = repository;
            _employeeRepository = employeeRepository;
            _rolesRepository = rolesRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AutorizationReadDto>>> GetAutorizations()
        {
            var autorizations = await _repository.GetWithIncludesAsync(
                a => a.Employee,
                a => a.Role);

            var autorizationDtos = _mapper.Map<IEnumerable<AutorizationReadDto>>(autorizations);
            return Ok(autorizationDtos);
        }

        [HttpGet("{employeeId}")]
        public async Task<ActionResult<AutorizationReadDto>> GetAutorization(int employeeId)
        {
            var autorization = await _repository.GetByEmployeeIdAsync(employeeId);
            if (autorization == null)
                return NotFound(new { message = $"Autorization for employee with ID {employeeId} not found" });

            var autorizationDto = _mapper.Map<AutorizationReadDto>(autorization);
            return Ok(autorizationDto);
        }

        [HttpPost]
        public async Task<ActionResult<AutorizationReadDto>> PostAutorization(AutorizationCreateDto createDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Проверка существования сотрудника
            if (!await _employeeRepository.ExistsAsync(createDto.EmployeeId))
                return BadRequest(new { message = $"Employee with ID {createDto.EmployeeId} not found" });

            // Проверка существования роли
            if (!await _rolesRepository.ExistsAsync(createDto.RoleId))
                return BadRequest(new { message = $"Role with ID {createDto.RoleId} not found" });

            // Проверка уникальности логина
            var existingByLogin = await _repository.GetByLoginAsync(createDto.Login);
            if (existingByLogin != null)
                return BadRequest(new { message = $"Login '{createDto.Login}' already exists" });

            // Проверка что у сотрудника еще нет авторизации
            var existingByEmployee = await _repository.GetByEmployeeIdAsync(createDto.EmployeeId);
            if (existingByEmployee != null)
                return BadRequest(new { message = $"Employee with ID {createDto.EmployeeId} already has autorization" });

            var autorization = _mapper.Map<Autorization>(createDto);
            var createdAutorization = await _repository.CreateAsync(autorization);
            var autorizationReadDto = _mapper.Map<AutorizationReadDto>(createdAutorization);

            return CreatedAtAction(nameof(GetAutorization), new { employeeId = autorizationReadDto.EmployeeId }, autorizationReadDto);
        }

        [HttpPut("{employeeId}")]
        public async Task<IActionResult> PutAutorization(int employeeId, AutorizationUpdateDto updateDto)
        {
            var existingAutorization = await _repository.GetByEmployeeIdAsync(employeeId);
            if (existingAutorization == null)
                return NotFound(new { message = $"Autorization for employee with ID {employeeId} not found" });

            // Проверка существования роли
            if (!await _rolesRepository.ExistsAsync(updateDto.RoleId))
                return BadRequest(new { message = $"Role with ID {updateDto.RoleId} not found" });

            // Проверка уникальности логина (если изменился)
            if (existingAutorization.Login != updateDto.Login)
            {
                var existingByLogin = await _repository.GetByLoginAsync(updateDto.Login);
                if (existingByLogin != null)
                    return BadRequest(new { message = $"Login '{updateDto.Login}' already exists" });
            }

            var autorization = _mapper.Map<Autorization>(updateDto);
            autorization.EmployeeId = employeeId;

            var updatedAutorization = await _repository.UpdateAsync(employeeId, autorization);
            if (updatedAutorization == null)
                return BadRequest(new { message = "Failed to update autorization" });

            return NoContent();
        }

        [HttpDelete("{employeeId}")]
        public async Task<IActionResult> DeleteAutorization(int employeeId)
        {
            var result = await _repository.DeleteAsync(employeeId);
            if (!result)
                return NotFound(new { message = $"Autorization for employee with ID {employeeId} not found" });

            return NoContent();
        }

        [HttpGet("login/{login}")]
        public async Task<ActionResult<AutorizationReadDto>> GetAutorizationByLogin(string login)
        {
            var autorization = await _repository.GetByLoginAsync(login);
            if (autorization == null)
                return NotFound(new { message = $"Autorization with login '{login}' not found" });

            var autorizationDto = _mapper.Map<AutorizationReadDto>(autorization);
            return Ok(autorizationDto);
        }

        [HttpPost("validate")]
        public async Task<ActionResult<AutorizationReadDto>> ValidateCredentials([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var isValid = await _repository.ValidateCredentialsAsync(request.Login, request.Password);
            if (!isValid)
                return Unauthorized(new { message = "Invalid login or password" });

            var autorization = await _repository.GetByLoginAsync(request.Login);
            var autorizationDto = _mapper.Map<AutorizationReadDto>(autorization);

            return Ok(new
            {
                message = "Login successful",
                user = autorizationDto
            });
        }

        [HttpGet("role/{roleId}")]
        public async Task<ActionResult<IEnumerable<AutorizationReadDto>>> GetAutorizationsByRole(int roleId)
        {
            var autorizations = await _repository.GetByRoleAsync(roleId);
            var autorizationDtos = _mapper.Map<IEnumerable<AutorizationReadDto>>(autorizations);
            return Ok(autorizationDtos);
        }
    }

    // DTO для запроса логина
    public class LoginRequest
    {
        public string Login { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
