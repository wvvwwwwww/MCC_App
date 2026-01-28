using AutoMapper;
using MccApi.Models;
using MccApi.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using static MccApi.DTOs.ScheduleDtos;

namespace MccApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SchedulesController : ControllerBase
    {
        private readonly IScheduleRepository _repository;
        private readonly IMapper _mapper;

        public SchedulesController(IScheduleRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ScheduleReadDto>>> GetSchedules()
        {
            var schedules = await _repository.GetSchedulesWithDetailsAsync();
            var scheduleDtos = _mapper.Map<IEnumerable<ScheduleReadDto>>(schedules);
            return Ok(scheduleDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ScheduleReadDto>> GetSchedule(int id)
        {
            var schedules = await _repository.GetSchedulesWithDetailsAsync();
            var schedule = schedules.FirstOrDefault(s => s.Id == id);

            if (schedule == null)
                return NotFound(new { message = $"Schedule with ID {id} not found" });

            var scheduleDto = _mapper.Map<ScheduleReadDto>(schedule);
            return Ok(scheduleDto);
        }

        [HttpPost]
        public async Task<ActionResult<ScheduleReadDto>> PostSchedule(ScheduleCreateDto createDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var schedule = _mapper.Map<Schedule>(createDto);
            var createdSchedule = await _repository.CreateAsync(schedule);
            var scheduleReadDto = _mapper.Map<ScheduleReadDto>(createdSchedule);

            return CreatedAtAction(nameof(GetSchedule), new { id = scheduleReadDto.Id }, scheduleReadDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutSchedule(int id, ScheduleUpdateDto updateDto)
        {
            if (!await _repository.ExistsAsync(id))
                return NotFound(new { message = $"Schedule with ID {id} not found" });

            var schedule = _mapper.Map<Schedule>(updateDto);
            schedule.Id = id;

            var updatedSchedule = await _repository.UpdateAsync(id, schedule);
            if (updatedSchedule == null)
                return BadRequest(new { message = "Failed to update schedule" });

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSchedule(int id)
        {
            var result = await _repository.DeleteAsync(id);
            if (!result)
                return NotFound(new { message = $"Schedule with ID {id} not found" });

            return NoContent();
        }

        [HttpGet("employee/{employeeId}")]
        public async Task<ActionResult<IEnumerable<ScheduleReadDto>>> GetSchedulesByEmployee(int employeeId)
        {
            var schedules = await _repository.GetSchedulesByEmployeeAsync(employeeId);
            var scheduleDtos = _mapper.Map<IEnumerable<ScheduleReadDto>>(schedules);
            return Ok(scheduleDtos);
        }

        [HttpGet("point/{pointId}")]
        public async Task<ActionResult<IEnumerable<ScheduleReadDto>>> GetSchedulesByPoint(int pointId)
        {
            var schedules = await _repository.GetSchedulesByPointAsync(pointId);
            var scheduleDtos = _mapper.Map<IEnumerable<ScheduleReadDto>>(schedules);
            return Ok(scheduleDtos);
        }

        [HttpGet("date-range")]
        public async Task<ActionResult<IEnumerable<ScheduleReadDto>>> GetSchedulesByDateRange(
            [FromQuery] DateOnly startDate,
            [FromQuery] DateOnly endDate)
        {
            var schedules = await _repository.GetSchedulesByDateRangeAsync(startDate, endDate);
            var scheduleDtos = _mapper.Map<IEnumerable<ScheduleReadDto>>(schedules);
            return Ok(scheduleDtos);
        }
    }
}
