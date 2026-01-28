using AutoMapper;
using MccApi.Models;
using MccApi.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using static MccApi.DTOs.PointDtos;

namespace MccApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PointsController : ControllerBase
    {
        private readonly IPointRepository _repository;
        private readonly IMapper _mapper;

        public PointsController(IPointRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PointReadDto>>> GetPoints()
        {
            var points = await _repository.GetPointsWithEmployeesAsync();
            var pointDtos = _mapper.Map<IEnumerable<PointReadDto>>(points);
            return Ok(pointDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PointReadDto>> GetPoint(int id)
        {
            var points = await _repository.GetPointsWithEmployeesAsync();
            var point = points.FirstOrDefault(p => p.Id == id);

            if (point == null)
                return NotFound(new { message = $"Point with ID {id} not found" });

            var pointDto = _mapper.Map<PointReadDto>(point);
            return Ok(pointDto);
        }

        [HttpPost]
        public async Task<ActionResult<PointReadDto>> PostPoint(PointCreateDto createDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var point = _mapper.Map<Point>(createDto);
            var createdPoint = await _repository.CreateAsync(point);
            var pointReadDto = _mapper.Map<PointReadDto>(createdPoint);

            return CreatedAtAction(nameof(GetPoint), new { id = pointReadDto.Id }, pointReadDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutPoint(int id, PointUpdateDto updateDto)
        {
            if (!await _repository.ExistsAsync(id))
                return NotFound(new { message = $"Point with ID {id} not found" });

            var point = _mapper.Map<Point>(updateDto);
            point.Id = id;

            var updatedPoint = await _repository.UpdateAsync(id, point);
            if (updatedPoint == null)
                return BadRequest(new { message = "Failed to update point" });

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePoint(int id)
        {
            var result = await _repository.DeleteAsync(id);
            if (!result)
                return NotFound(new { message = $"Point with ID {id} not found" });

            return NoContent();
        }

        [HttpGet("with-kitchen")]
        public async Task<ActionResult<IEnumerable<PointReadDto>>> GetPointsWithKitchen()
        {
            var points = await _repository.GetPointsWithKitchenAsync();
            var pointDtos = _mapper.Map<IEnumerable<PointReadDto>>(points);
            return Ok(pointDtos);
        }

        [HttpGet("employee/{employeeId}")]
        public async Task<ActionResult<IEnumerable<PointReadDto>>> GetPointsByEmployee(int employeeId)
        {
            var points = await _repository.GetPointsByEmployeeAsync(employeeId);
            var pointDtos = _mapper.Map<IEnumerable<PointReadDto>>(points);
            return Ok(pointDtos);
        }
    }
}
