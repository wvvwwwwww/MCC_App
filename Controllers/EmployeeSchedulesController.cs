using AutoMapper;
using MccApi.DTOs;
using MccApi.Models;
using MccApi.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MccApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeSchedulesController : ControllerBase
    {
        private readonly IEmployeeScheduleRepository _repository;
        private readonly IMapper _mapper;

        public EmployeeSchedulesController(IEmployeeScheduleRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeScheduleReadDto>>> GetEmployeeSchedules()
        {
            var schedules = await _repository.GetSchedulesWithEmployeesAsync();
            var scheduleDtos = _mapper.Map<IEnumerable<EmployeeScheduleReadDto>>(schedules);
            return Ok(scheduleDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EmployeeScheduleReadDto>> GetEmployeeSchedule(int id)
        {
            var schedule = await _repository.GetByIdAsync(id);
            if (schedule == null)
                return NotFound(new { message = $"Employee schedule with ID {id} not found" });

            var scheduleDto = _mapper.Map<EmployeeScheduleReadDto>(schedule);
            return Ok(scheduleDto);
        }

        [HttpPost]
        public async Task<ActionResult<EmployeeScheduleReadDto>> PostEmployeeSchedule(EmployeeScheduleCreateDto createDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var schedule = _mapper.Map<EmployeeSchedule>(createDto);
            var createdSchedule = await _repository.CreateAsync(schedule);
            var scheduleReadDto = _mapper.Map<EmployeeScheduleReadDto>(createdSchedule);

            return CreatedAtAction(nameof(GetEmployeeSchedule), new { id = scheduleReadDto.Id }, scheduleReadDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmployeeSchedule(int id, EmployeeScheduleUpdateDto updateDto)
        {
            if (!await _repository.ExistsAsync(id))
                return NotFound(new { message = $"Employee schedule with ID {id} not found" });

            var schedule = _mapper.Map<EmployeeSchedule>(updateDto);
            schedule.Id = id;

            var updatedSchedule = await _repository.UpdateAsync(id, schedule);
            if (updatedSchedule == null)
                return BadRequest(new { message = "Failed to update employee schedule" });

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployeeSchedule(int id)
        {
            var result = await _repository.DeleteAsync(id);
            if (!result)
                return NotFound(new { message = $"Employee schedule with ID {id} not found" });

            return NoContent();
        }

        [HttpGet("employee/{employeeId}")]
        public async Task<ActionResult<IEnumerable<EmployeeScheduleReadDto>>> GetSchedulesByEmployee(int employeeId)
        {
            var schedules = await _repository.GetSchedulesByEmployeeAsync(employeeId);
            var scheduleDtos = _mapper.Map<IEnumerable<EmployeeScheduleReadDto>>(schedules);
            return Ok(scheduleDtos);
        }

        [HttpGet("date/{date}")]
        public async Task<ActionResult<IEnumerable<EmployeeScheduleReadDto>>> GetSchedulesByDate(DateOnly date)
        {
            var schedules = await _repository.GetSchedulesByDateAsync(date);
            var scheduleDtos = _mapper.Map<IEnumerable<EmployeeScheduleReadDto>>(schedules);
            return Ok(scheduleDtos);
        }

        [HttpGet("date-range")]
        public async Task<ActionResult<IEnumerable<EmployeeScheduleReadDto>>> GetSchedulesByDateRange(
            [FromQuery] DateOnly startDate,
            [FromQuery] DateOnly endDate)
        {
            var schedules = await _repository.GetSchedulesByDateRangeAsync(startDate, endDate);
            var scheduleDtos = _mapper.Map<IEnumerable<EmployeeScheduleReadDto>>(schedules);
            return Ok(scheduleDtos);
        }
    }
}
