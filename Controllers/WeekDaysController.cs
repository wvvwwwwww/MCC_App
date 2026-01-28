using AutoMapper;
using MccApi.DTOs;
using MccApi.Models;
using MccApi.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MccApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WeekDaysController : ControllerBase
    {
        private readonly IWeekDayRepository _repository;
        private readonly IMapper _mapper;

        public WeekDaysController(IWeekDayRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<WeekDayReadDto>>> GetWeekDays()
        {
            var weekDays = await _repository.GetAllAsync();
            var weekDayDtos = _mapper.Map<IEnumerable<WeekDayReadDto>>(weekDays);
            return Ok(weekDayDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<WeekDayReadDto>> GetWeekDay(int id)
        {
            var weekDay = await _repository.GetByIdAsync(id);
            if (weekDay == null)
                return NotFound(new { message = $"Week day with ID {id} not found" });

            var weekDayDto = _mapper.Map<WeekDayReadDto>(weekDay);
            return Ok(weekDayDto);
        }

        [HttpPost]
        public async Task<ActionResult<WeekDayReadDto>> PostWeekDay(WeekDayCreateDto createDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var weekDay = _mapper.Map<WeekDay>(createDto);
            var createdWeekDay = await _repository.CreateAsync(weekDay);
            var weekDayReadDto = _mapper.Map<WeekDayReadDto>(createdWeekDay);

            return CreatedAtAction(nameof(GetWeekDay), new { id = weekDayReadDto.Id }, weekDayReadDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutWeekDay(int id, WeekDayUpdateDto updateDto)
        {
            if (!await _repository.ExistsAsync(id))
                return NotFound(new { message = $"Week day with ID {id} not found" });

            var weekDay = _mapper.Map<WeekDay>(updateDto);
            weekDay.Id = id;

            var updatedWeekDay = await _repository.UpdateAsync(id, weekDay);
            if (updatedWeekDay == null)
                return BadRequest(new { message = "Failed to update week day" });

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWeekDay(int id)
        {
            var result = await _repository.DeleteAsync(id);
            if (!result)
                return NotFound(new { message = $"Week day with ID {id} not found" });

            return NoContent();
        }

        [HttpGet("inventory")]
        public async Task<ActionResult<IEnumerable<WeekDayReadDto>>> GetInventoryDays()
        {
            var weekDays = await _repository.GetInventoryDaysAsync();
            var weekDayDtos = _mapper.Map<IEnumerable<WeekDayReadDto>>(weekDays);
            return Ok(weekDayDtos);
        }

        [HttpGet("name/{dayName}")]
        public async Task<ActionResult<WeekDayReadDto>> GetWeekDayByName(string dayName)
        {
            var weekDay = await _repository.GetDayByNameAsync(dayName);
            if (weekDay == null)
                return NotFound(new { message = $"Week day with name '{dayName}' not found" });

            var weekDayDto = _mapper.Map<WeekDayReadDto>(weekDay);
            return Ok(weekDayDto);
        }
    }
}
