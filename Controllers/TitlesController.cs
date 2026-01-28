using AutoMapper;
using MccApi.DTOs;
using MccApi.Models;
using MccApi.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MccApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TitlesController : ControllerBase
    {
        private readonly ITitleRepository _repository;
        private readonly IMapper _mapper;

        public TitlesController(ITitleRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TitleReadDto>>> GetTitles()
        {
            var titles = await _repository.GetAllAsync();
            var titleDtos = _mapper.Map<IEnumerable<TitleReadDto>>(titles);
            return Ok(titleDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TitleReadDto>> GetTitle(int id)
        {
            var title = await _repository.GetByIdAsync(id);
            if (title == null)
                return NotFound(new { message = $"Title with ID {id} not found" });

            var titleDto = _mapper.Map<TitleReadDto>(title);
            return Ok(titleDto);
        }

        [HttpPost]
        public async Task<ActionResult<TitleReadDto>> PostTitle(TitleCreateDto createDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var title = _mapper.Map<Title>(createDto);
            var createdTitle = await _repository.CreateAsync(title);
            var titleReadDto = _mapper.Map<TitleReadDto>(createdTitle);

            return CreatedAtAction(nameof(GetTitle), new { id = titleReadDto.Id }, titleReadDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutTitle(int id, TitleUpdateDto updateDto)
        {
            if (!await _repository.ExistsAsync(id))
                return NotFound(new { message = $"Title with ID {id} not found" });

            var title = _mapper.Map<Title>(updateDto);
            title.Id = id;

            var updatedTitle = await _repository.UpdateAsync(id, title);
            if (updatedTitle == null)
                return BadRequest(new { message = "Failed to update title" });

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTitle(int id)
        {
            var result = await _repository.DeleteAsync(id);
            if (!result)
                return NotFound(new { message = $"Title with ID {id} not found" });

            return NoContent();
        }

        [HttpGet("with-employees")]
        public async Task<ActionResult<IEnumerable<TitleReadDto>>> GetTitlesWithEmployees()
        {
            var titles = await _repository.GetTitlesWithEmployeesAsync();
            var titleDtos = _mapper.Map<IEnumerable<TitleReadDto>>(titles);
            return Ok(titleDtos);
        }
    }
}
