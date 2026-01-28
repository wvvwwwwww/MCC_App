using AutoMapper;
using MccApi.DTOs;
using MccApi.Models;
using MccApi.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MccApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChangesHistoriesController : ControllerBase
    {
        private readonly IChangesHistoryRepository _repository;
        private readonly IMapper _mapper;

        public ChangesHistoriesController(IChangesHistoryRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ChangesHistoryReadDto>>> GetChangesHistories()
        {
            var histories = await _repository.GetHistoryWithDetailsAsync();
            var historyDtos = _mapper.Map<IEnumerable<ChangesHistoryReadDto>>(histories);
            return Ok(historyDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ChangesHistoryReadDto>> GetChangesHistory(int id)
        {
            var history = await _repository.GetByIdAsync(id);
            if (history == null)
                return NotFound(new { message = $"Changes history with ID {id} not found" });

            var historyDto = _mapper.Map<ChangesHistoryReadDto>(history);
            return Ok(historyDto);
        }

        [HttpPost]
        public async Task<ActionResult<ChangesHistoryReadDto>> PostChangesHistory(ChangesHistoryCreateDto createDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var history = _mapper.Map<ChangesHistory>(createDto);
            var createdHistory = await _repository.CreateAsync(history);
            var historyReadDto = _mapper.Map<ChangesHistoryReadDto>(createdHistory);

            return CreatedAtAction(nameof(GetChangesHistory), new { id = historyReadDto.Id }, historyReadDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutChangesHistory(int id, ChangesHistoryUpdateDto updateDto)
        {
            if (!await _repository.ExistsAsync(id))
                return NotFound(new { message = $"Changes history with ID {id} not found" });

            var history = _mapper.Map<ChangesHistory>(updateDto);
            history.Id = id;

            var updatedHistory = await _repository.UpdateAsync(id, history);
            if (updatedHistory == null)
                return BadRequest(new { message = "Failed to update changes history" });

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteChangesHistory(int id)
        {
            var result = await _repository.DeleteAsync(id);
            if (!result)
                return NotFound(new { message = $"Changes history with ID {id} not found" });

            return NoContent();
        }

        [HttpGet("schedule/{scheduleId}")]
        public async Task<ActionResult<IEnumerable<ChangesHistoryReadDto>>> GetHistoryBySchedule(int scheduleId)
        {
            var histories = await _repository.GetHistoryByScheduleAsync(scheduleId);
            var historyDtos = _mapper.Map<IEnumerable<ChangesHistoryReadDto>>(histories);
            return Ok(historyDtos);
        }

        [HttpGet("employee/{employeeId}")]
        public async Task<ActionResult<IEnumerable<ChangesHistoryReadDto>>> GetHistoryByEmployee(int employeeId)
        {
            var histories = await _repository.GetHistoryByEmployeeAsync(employeeId);
            var historyDtos = _mapper.Map<IEnumerable<ChangesHistoryReadDto>>(histories);
            return Ok(historyDtos);
        }

        [HttpGet("point/{pointId}")]
        public async Task<ActionResult<IEnumerable<ChangesHistoryReadDto>>> GetHistoryByPoint(int pointId)
        {
            var histories = await _repository.GetHistoryByPointAsync(pointId);
            var historyDtos = _mapper.Map<IEnumerable<ChangesHistoryReadDto>>(histories);
            return Ok(historyDtos);
        }
    }
}
