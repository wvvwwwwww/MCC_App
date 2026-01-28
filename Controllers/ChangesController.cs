using AutoMapper;
using MccApi.DTOs;
using MccApi.Models;
using MccApi.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MccApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChangesController : ControllerBase
    {
        private readonly IChangeRepository _repository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IChangeStatusRepository _statusRepository;
        private readonly IMapper _mapper;

        public ChangesController(
            IChangeRepository repository,
            IEmployeeRepository employeeRepository,
            IChangeStatusRepository statusRepository,
            IMapper mapper)
        {
            _repository = repository;
            _employeeRepository = employeeRepository;
            _statusRepository = statusRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ChangeReadDto>>> GetChanges()
        {
            var changes = await _repository.GetChangesWithDetailsAsync();
            var changeDtos = _mapper.Map<IEnumerable<ChangeReadDto>>(changes);
            return Ok(changeDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ChangeReadDto>> GetChange(int id)
        {
            var changes = await _repository.GetChangesWithDetailsAsync();
            var change = changes.FirstOrDefault(c => c.Id == id);

            if (change == null)
                return NotFound(new { message = $"Change with ID {id} not found" });

            var changeDto = _mapper.Map<ChangeReadDto>(change);
            return Ok(changeDto);
        }

        [HttpPost]
        public async Task<ActionResult<ChangeReadDto>> PostChange(ChangeCreateDto createDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Проверка существования сотрудника
            if (!await _employeeRepository.ExistsAsync(createDto.EmployeeId))
                return BadRequest(new { message = $"Employee with ID {createDto.EmployeeId} not found" });

            // Проверка существования статуса
            if (!await _statusRepository.ExistsAsync(createDto.StatusId))
                return BadRequest(new { message = $"Change status with ID {createDto.StatusId} not found" });

            var change = _mapper.Map<Change>(createDto);
            var createdChange = await _repository.CreateAsync(change);
            var changeReadDto = _mapper.Map<ChangeReadDto>(createdChange);

            return CreatedAtAction(nameof(GetChange), new { id = changeReadDto.Id }, changeReadDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutChange(int id, ChangeUpdateDto updateDto)
        {
            if (!await _repository.ExistsAsync(id))
                return NotFound(new { message = $"Change with ID {id} not found" });

            // Проверка существования сотрудника
            if (!await _employeeRepository.ExistsAsync(updateDto.EmployeeId))
                return BadRequest(new { message = $"Employee with ID {updateDto.EmployeeId} not found" });

            // Проверка существования статуса
            if (!await _statusRepository.ExistsAsync(updateDto.StatusId))
                return BadRequest(new { message = $"Change status with ID {updateDto.StatusId} not found" });

            var change = _mapper.Map<Change>(updateDto);
            change.Id = id;

            var updatedChange = await _repository.UpdateAsync(id, change);
            if (updatedChange == null)
                return BadRequest(new { message = "Failed to update change" });

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteChange(int id)
        {
            var result = await _repository.DeleteAsync(id);
            if (!result)
                return NotFound(new { message = $"Change with ID {id} not found" });

            return NoContent();
        }

        [HttpGet("employee/{employeeId}")]
        public async Task<ActionResult<IEnumerable<ChangeReadDto>>> GetChangesByEmployee(int employeeId)
        {
            var changes = await _repository.GetChangesByEmployeeAsync(employeeId);
            var changeDtos = _mapper.Map<IEnumerable<ChangeReadDto>>(changes);
            return Ok(changeDtos);
        }

        [HttpGet("status/{statusId}")]
        public async Task<ActionResult<IEnumerable<ChangeReadDto>>> GetChangesByStatus(int statusId)
        {
            var changes = await _repository.GetChangesByStatusAsync(statusId);
            var changeDtos = _mapper.Map<IEnumerable<ChangeReadDto>>(changes);
            return Ok(changeDtos);
        }

        [HttpGet("date-range")]
        public async Task<ActionResult<IEnumerable<ChangeReadDto>>> GetChangesByDateRange(
            [FromQuery] DateOnly startDate,
            [FromQuery] DateOnly endDate)
        {
            var changes = await _repository.GetChangesByDateRangeAsync(startDate, endDate);
            var changeDtos = _mapper.Map<IEnumerable<ChangeReadDto>>(changes);
            return Ok(changeDtos);
        }
    }
}
