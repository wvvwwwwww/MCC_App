namespace MccApi.DTOs
{
    public class MeetingReadDto
    {
        public int Id { get; set; }
        public DateOnly Date { get; set; }
        public int PointId { get; set; }
        public int? StatusId { get; set; }
        public int MeetingTopicId { get; set; }
        public string? PointAddress { get; set; }
        public string? StatusName { get; set; }
        public string? TopicName { get; set; }
        public List<MeetingAttendeeDto> Attendees { get; set; } = new();
    }

    public class MeetingCreateDto
    {
        public DateOnly Date { get; set; }
        public int PointId { get; set; }
        public int? StatusId { get; set; }
        public int MeetingTopicId { get; set; }
        public List<int> AttendeeIds { get; set; } = new();
    }

    public class MeetingUpdateDto
    {
        public DateOnly Date { get; set; }
        public int PointId { get; set; }
        public int? StatusId { get; set; }
        public int MeetingTopicId { get; set; }
        public List<int> AttendeeIds { get; set; } = new();
    }

    public class MeetingAttendeeDto
    {
        public int EmployeeId { get; set; }
        public string? EmployeeName { get; set; }
    }
}
