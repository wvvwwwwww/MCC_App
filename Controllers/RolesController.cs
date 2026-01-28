using AutoMapper;
using MccApi.DTOs;
using MccApi.Models;
using MccApi.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MccApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly IRolesRepository _repository;
        private readonly IMapper _mapper;

        public RolesController(IRolesRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RolesReadDto>>> GetRoles()
        {
            var roles = await _repository.GetAllAsync();
            var roleDtos = _mapper.Map<IEnumerable<RolesReadDto>>(roles);
            return Ok(roleDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RolesReadDto>> GetRole(int id)
        {
            var role = await _repository.GetByIdAsync(id);
            if (role == null)
                return NotFound(new { message = $"Role with ID {id} not found" });

            var roleDto = _mapper.Map<RolesReadDto>(role);
            return Ok(roleDto);
        }

        [HttpPost]
        public async Task<ActionResult<RolesReadDto>> PostRole(RolesCreateDto createDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var role = _mapper.Map<Roles>(createDto);
            var createdRole = await _repository.CreateAsync(role);
            var roleReadDto = _mapper.Map<RolesReadDto>(createdRole);

            return CreatedAtAction(nameof(GetRole), new { id = roleReadDto.Id }, roleReadDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutRole(int id, RolesUpdateDto updateDto)
        {
            if (!await _repository.ExistsAsync(id))
                return NotFound(new { message = $"Role with ID {id} not found" });

            var role = _mapper.Map<Roles>(updateDto);
            role.Id = id;

            var updatedRole = await _repository.UpdateAsync(id, role);
            if (updatedRole == null)
                return BadRequest(new { message = "Failed to update role" });

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRole(int id)
        {
            var result = await _repository.DeleteAsync(id);
            if (!result)
                return NotFound(new { message = $"Role with ID {id} not found" });

            return NoContent();
        }

        [HttpGet("name/{roleName}")]
        public async Task<ActionResult<RolesReadDto>> GetRoleByName(string roleName)
        {
            var role = await _repository.GetRoleByNameAsync(roleName);
            if (role == null)
                return NotFound(new { message = $"Role with name '{roleName}' not found" });

            var roleDto = _mapper.Map<RolesReadDto>(role);
            return Ok(roleDto);
        }
    }
}
