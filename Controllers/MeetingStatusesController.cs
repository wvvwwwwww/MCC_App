using AutoMapper;
using MccApi.DTOs;
using MccApi.Models;
using MccApi.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MccApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MeetingStatusesController : ControllerBase
    {
        private readonly IMeetingStatusRepository _repository;
        private readonly IMapper _mapper;

        public MeetingStatusesController(IMeetingStatusRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MeetingStatusReadDto>>> GetMeetingStatuses()
        {
            var statuses = await _repository.GetAllAsync();
            var statusDtos = _mapper.Map<IEnumerable<MeetingStatusReadDto>>(statuses);
            return Ok(statusDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MeetingStatusReadDto>> GetMeetingStatus(int id)
        {
            var status = await _repository.GetByIdAsync(id);
            if (status == null)
                return NotFound(new { message = $"Meeting status with ID {id} not found" });

            var statusDto = _mapper.Map<MeetingStatusReadDto>(status);
            return Ok(statusDto);
        }

        [HttpPost]
        public async Task<ActionResult<MeetingStatusReadDto>> PostMeetingStatus(MeetingStatusCreateDto createDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var status = _mapper.Map<MeetingStatus>(createDto);
            var createdStatus = await _repository.CreateAsync(status);
            var statusReadDto = _mapper.Map<MeetingStatusReadDto>(createdStatus);

            return CreatedAtAction(nameof(GetMeetingStatus), new { id = statusReadDto.Id }, statusReadDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutMeetingStatus(int id, MeetingStatusUpdateDto updateDto)
        {
            if (!await _repository.ExistsAsync(id))
                return NotFound(new { message = $"Meeting status with ID {id} not found" });

            var status = _mapper.Map<MeetingStatus>(updateDto);
            status.Id = id;

            var updatedStatus = await _repository.UpdateAsync(id, status);
            if (updatedStatus == null)
                return BadRequest(new { message = "Failed to update meeting status" });

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMeetingStatus(int id)
        {
            var result = await _repository.DeleteAsync(id);
            if (!result)
                return NotFound(new { message = $"Meeting status with ID {id} not found" });

            return NoContent();
        }

        [HttpGet("name/{statusName}")]
        public async Task<ActionResult<MeetingStatusReadDto>> GetMeetingStatusByName(string statusName)
        {
            var status = await _repository.GetStatusByNameAsync(statusName);
            if (status == null)
                return NotFound(new { message = $"Meeting status with name '{statusName}' not found" });

            var statusDto = _mapper.Map<MeetingStatusReadDto>(status);
            return Ok(statusDto);
        }
    }
}
