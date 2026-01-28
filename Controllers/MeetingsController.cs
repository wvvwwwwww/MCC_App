using AutoMapper;
using MccApi.DTOs;
using MccApi.Models;
using MccApi.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MccApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MeetingsController : ControllerBase
    {
        private readonly IMeetingRepository _repository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IPointRepository _pointRepository;
        private readonly IMeetingTopicRepository _topicRepository;
        private readonly IMeetingStatusRepository _statusRepository;
        private readonly IMapper _mapper;

        public MeetingsController(
            IMeetingRepository repository,
            IEmployeeRepository employeeRepository,
            IPointRepository pointRepository,
            IMeetingTopicRepository topicRepository,
            IMeetingStatusRepository statusRepository,
            IMapper mapper)
        {
            _repository = repository;
            _employeeRepository = employeeRepository;
            _pointRepository = pointRepository;
            _topicRepository = topicRepository;
            _statusRepository = statusRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MeetingReadDto>>> GetMeetings()
        {
            var meetings = await _repository.GetMeetingsWithDetailsAsync();
            var meetingDtos = _mapper.Map<IEnumerable<MeetingReadDto>>(meetings);
            return Ok(meetingDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MeetingReadDto>> GetMeeting(int id)
        {
            var meeting = await _repository.GetMeetingWithAttendeesAsync(id);
            if (meeting == null)
                return NotFound(new { message = $"Meeting with ID {id} not found" });

            var meetingDto = _mapper.Map<MeetingReadDto>(meeting);
            return Ok(meetingDto);
        }

        [HttpPost]
        public async Task<ActionResult<MeetingReadDto>> PostMeeting(MeetingCreateDto createDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Проверка существования точки
            if (!await _pointRepository.ExistsAsync(createDto.PointId))
                return BadRequest(new { message = $"Point with ID {createDto.PointId} not found" });

            // Проверка существования темы
            if (!await _topicRepository.ExistsAsync(createDto.MeetingTopicId))
                return BadRequest(new { message = $"Meeting topic with ID {createDto.MeetingTopicId} not found" });

            // Проверка существования статуса, если указан
            if (createDto.StatusId.HasValue && !await _statusRepository.ExistsAsync(createDto.StatusId.Value))
                return BadRequest(new { message = $"Meeting status with ID {createDto.StatusId} not found" });

            var meeting = _mapper.Map<Meeting>(createDto);
            var createdMeeting = await _repository.CreateAsync(meeting);

            // Добавление участников
            if (createDto.AttendeeIds != null && createDto.AttendeeIds.Any())
            {
                foreach (var attendeeId in createDto.AttendeeIds)
                {
                    if (await _employeeRepository.ExistsAsync(attendeeId))
                    {
                        await _repository.AddAttendeeToMeetingAsync(createdMeeting.Id, attendeeId);
                    }
                }
            }

            var meetingReadDto = _mapper.Map<MeetingReadDto>(createdMeeting);
            return CreatedAtAction(nameof(GetMeeting), new { id = meetingReadDto.Id }, meetingReadDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutMeeting(int id, MeetingUpdateDto updateDto)
        {
            if (!await _repository.ExistsAsync(id))
                return NotFound(new { message = $"Meeting with ID {id} not found" });

            // Проверка существования точки
            if (!await _pointRepository.ExistsAsync(updateDto.PointId))
                return BadRequest(new { message = $"Point with ID {updateDto.PointId} not found" });

            // Проверка существования темы
            if (!await _topicRepository.ExistsAsync(updateDto.MeetingTopicId))
                return BadRequest(new { message = $"Meeting topic with ID {updateDto.MeetingTopicId} not found" });

            var meeting = _mapper.Map<Meeting>(updateDto);
            meeting.Id = id;

            var updatedMeeting = await _repository.UpdateAsync(id, meeting);
            if (updatedMeeting == null)
                return BadRequest(new { message = "Failed to update meeting" });

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMeeting(int id)
        {
            var result = await _repository.DeleteAsync(id);
            if (!result)
                return NotFound(new { message = $"Meeting with ID {id} not found" });

            return NoContent();
        }

        [HttpGet("point/{pointId}")]
        public async Task<ActionResult<IEnumerable<MeetingReadDto>>> GetMeetingsByPoint(int pointId)
        {
            var meetings = await _repository.GetMeetingsByPointAsync(pointId);
            var meetingDtos = _mapper.Map<IEnumerable<MeetingReadDto>>(meetings);
            return Ok(meetingDtos);
        }

        [HttpGet("date/{date}")]
        public async Task<ActionResult<IEnumerable<MeetingReadDto>>> GetMeetingsByDate(DateOnly date)
        {
            var meetings = await _repository.GetMeetingsByDateAsync(date);
            var meetingDtos = _mapper.Map<IEnumerable<MeetingReadDto>>(meetings);
            return Ok(meetingDtos);
        }

        [HttpGet("status/{statusId}")]
        public async Task<ActionResult<IEnumerable<MeetingReadDto>>> GetMeetingsByStatus(int statusId)
        {
            var meetings = await _repository.GetMeetingsByStatusAsync(statusId);
            var meetingDtos = _mapper.Map<IEnumerable<MeetingReadDto>>(meetings);
            return Ok(meetingDtos);
        }

        [HttpPost("{meetingId}/attendees/{employeeId}")]
        public async Task<IActionResult> AddAttendeeToMeeting(int meetingId, int employeeId)
        {
            var result = await _repository.AddAttendeeToMeetingAsync(meetingId, employeeId);
            if (!result)
                return BadRequest(new { message = "Failed to add attendee to meeting" });

            return Ok(new { message = "Attendee added successfully" });
        }

        [HttpDelete("{meetingId}/attendees/{employeeId}")]
        public async Task<IActionResult> RemoveAttendeeFromMeeting(int meetingId, int employeeId)
        {
            var result = await _repository.RemoveAttendeeFromMeetingAsync(meetingId, employeeId);
            if (!result)
                return BadRequest(new { message = "Failed to remove attendee from meeting" });

            return Ok(new { message = "Attendee removed successfully" });
        }
    }
}
