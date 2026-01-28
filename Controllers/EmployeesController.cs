using AutoMapper;
using MccApi.Models;
using MccApi.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using static MccApi.DTOs.EmployeeDtos;

namespace MccApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeRepository _repository;
        private readonly IMapper _mapper;

        public EmployeesController(IEmployeeRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeReadDto>>> GetEmployees()
        {
            var employees = await _repository.GetEmployeesWithTitlesAsync();
            var employeeDtos = _mapper.Map<IEnumerable<EmployeeReadDto>>(employees);
            return Ok(employeeDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EmployeeReadDto>> GetEmployee(int id)
        {
            var employee = await _repository.GetEmployeeWithDetailsAsync(id);
            if (employee == null)
                return NotFound(new { message = $"Employee with ID {id} not found" });

            var employeeDto = _mapper.Map<EmployeeReadDto>(employee);
            return Ok(employeeDto);
        }

        [HttpPost]
        public async Task<ActionResult<EmployeeReadDto>> PostEmployee(EmployeeCreateDto createDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var employee = _mapper.Map<Employee>(createDto);
            var createdEmployee = await _repository.CreateAsync(employee);
            var employeeReadDto = _mapper.Map<EmployeeReadDto>(createdEmployee);

            return CreatedAtAction(nameof(GetEmployee), new { id = employeeReadDto.Id }, employeeReadDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmployee(int id, EmployeeUpdateDto updateDto)
        {
            if (!await _repository.ExistsAsync(id))
                return NotFound(new { message = $"Employee with ID {id} not found" });

            var employee = _mapper.Map<Employee>(updateDto);
            employee.Id = id;

            var updatedEmployee = await _repository.UpdateAsync(id, employee);
            if (updatedEmployee == null)
                return BadRequest(new { message = "Failed to update employee" });

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var result = await _repository.DeleteAsync(id);
            if (!result)
                return NotFound(new { message = $"Employee with ID {id} not found" });

            return NoContent();
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<EmployeeReadDto>>> SearchEmployees([FromQuery] string term)
        {
            if (string.IsNullOrWhiteSpace(term))
                return BadRequest(new { message = "Search term is required" });

            var employees = await _repository.SearchEmployeesAsync(term);
            var employeeDtos = _mapper.Map<IEnumerable<EmployeeReadDto>>(employees);
            return Ok(employeeDtos);
        }

        [HttpGet("title/{titleId}")]
        public async Task<ActionResult<IEnumerable<EmployeeReadDto>>> GetEmployeesByTitle(int titleId)
        {
            var employees = await _repository.GetEmployeesByTitleAsync(titleId);
            var employeeDtos = _mapper.Map<IEnumerable<EmployeeReadDto>>(employees);
            return Ok(employeeDtos);
        }
    }
}
