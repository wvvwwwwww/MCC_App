namespace MyCoffeeCupApp.DTOs
{
    public class MeetingTopicReadDto
    {
        public int Id { get; set; }
        public string TopicName { get; set; } = string.Empty;
    }

    public class MeetingTopicCreateDto
    {
        public string TopicName { get; set; } = string.Empty;
    }

    public class MeetingTopicUpdateDto
    {
        public string TopicName { get; set; } = string.Empty;
    }
}
