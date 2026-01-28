using AutoMapper;
using MccApi.DTOs;
using MccApi.Models;
using MccApi.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MccApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MeetingTopicsController : ControllerBase
    {
        private readonly IMeetingTopicRepository _repository;
        private readonly IMapper _mapper;

        public MeetingTopicsController(IMeetingTopicRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MeetingTopicReadDto>>> GetMeetingTopics()
        {
            var topics = await _repository.GetAllAsync();
            var topicDtos = _mapper.Map<IEnumerable<MeetingTopicReadDto>>(topics);
            return Ok(topicDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MeetingTopicReadDto>> GetMeetingTopic(int id)
        {
            var topic = await _repository.GetByIdAsync(id);
            if (topic == null)
                return NotFound(new { message = $"Meeting topic with ID {id} not found" });

            var topicDto = _mapper.Map<MeetingTopicReadDto>(topic);
            return Ok(topicDto);
        }

        [HttpPost]
        public async Task<ActionResult<MeetingTopicReadDto>> PostMeetingTopic(MeetingTopicCreateDto createDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var topic = _mapper.Map<MeetingTopic>(createDto);
            var createdTopic = await _repository.CreateAsync(topic);
            var topicReadDto = _mapper.Map<MeetingTopicReadDto>(createdTopic);

            return CreatedAtAction(nameof(GetMeetingTopic), new { id = topicReadDto.Id }, topicReadDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutMeetingTopic(int id, MeetingTopicUpdateDto updateDto)
        {
            if (!await _repository.ExistsAsync(id))
                return NotFound(new { message = $"Meeting topic with ID {id} not found" });

            var topic = _mapper.Map<MeetingTopic>(updateDto);
            topic.Id = id;

            var updatedTopic = await _repository.UpdateAsync(id, topic);
            if (updatedTopic == null)
                return BadRequest(new { message = "Failed to update meeting topic" });

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMeetingTopic(int id)
        {
            var result = await _repository.DeleteAsync(id);
            if (!result)
                return NotFound(new { message = $"Meeting topic with ID {id} not found" });

            return NoContent();
        }

        [HttpGet("name/{topicName}")]
        public async Task<ActionResult<MeetingTopicReadDto>> GetMeetingTopicByName(string topicName)
        {
            var topic = await _repository.GetTopicByNameAsync(topicName);
            if (topic == null)
                return NotFound(new { message = $"Meeting topic with name '{topicName}' not found" });

            var topicDto = _mapper.Map<MeetingTopicReadDto>(topic);
            return Ok(topicDto);
        }
    }
}
