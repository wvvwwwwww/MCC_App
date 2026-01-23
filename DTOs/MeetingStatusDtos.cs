namespace MyCoffeeCupApp.DTOs
{
    public class MeetingStatusReadDto
    {
        public int Id { get; set; }
        public string StatusName { get; set; } = string.Empty;
    }

    public class MeetingStatusCreateDto
    {
        public string StatusName { get; set; } = string.Empty;
    }

    public class MeetingStatusUpdateDto
    {
        public string StatusName { get; set; } = string.Empty;
    }
}
