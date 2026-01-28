using AutoMapper;
using MccApi.DTOs;
using MccApi.Models;
using MccApi.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MccApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChangeStatusesController : ControllerBase
    {
        private readonly IChangeStatusRepository _repository;
        private readonly IMapper _mapper;

        public ChangeStatusesController(IChangeStatusRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ChangeStatusReadDto>>> GetChangeStatuses()
        {
            var statuses = await _repository.GetAllAsync();
            var statusDtos = _mapper.Map<IEnumerable<ChangeStatusReadDto>>(statuses);
            return Ok(statusDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ChangeStatusReadDto>> GetChangeStatus(int id)
        {
            var status = await _repository.GetByIdAsync(id);
            if (status == null)
                return NotFound(new { message = $"Change status with ID {id} not found" });

            var statusDto = _mapper.Map<ChangeStatusReadDto>(status);
            return Ok(statusDto);
        }

        [HttpPost]
        public async Task<ActionResult<ChangeStatusReadDto>> PostChangeStatus(ChangeStatusCreateDto createDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var status = _mapper.Map<ChangeStatus>(createDto);
            var createdStatus = await _repository.CreateAsync(status);
            var statusReadDto = _mapper.Map<ChangeStatusReadDto>(createdStatus);

            return CreatedAtAction(nameof(GetChangeStatus), new { id = statusReadDto.Id }, statusReadDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutChangeStatus(int id, ChangeStatusUpdateDto updateDto)
        {
            if (!await _repository.ExistsAsync(id))
                return NotFound(new { message = $"Change status with ID {id} not found" });

            var status = _mapper.Map<ChangeStatus>(updateDto);
            status.Id = id;

            var updatedStatus = await _repository.UpdateAsync(id, status);
            if (updatedStatus == null)
                return BadRequest(new { message = "Failed to update change status" });

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteChangeStatus(int id)
        {
            var result = await _repository.DeleteAsync(id);
            if (!result)
                return NotFound(new { message = $"Change status with ID {id} not found" });

            return NoContent();
        }

        [HttpGet("name/{statusName}")]
        public async Task<ActionResult<ChangeStatusReadDto>> GetChangeStatusByName(string statusName)
        {
            var status = await _repository.GetStatusByNameAsync(statusName);
            if (status == null)
                return NotFound(new { message = $"Change status with name '{statusName}' not found" });

            var statusDto = _mapper.Map<ChangeStatusReadDto>(status);
            return Ok(statusDto);
        }
    }
}
