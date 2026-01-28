namespace MccApi.DTOs
{
    public class ChangeStatusReadDto
    {
        public int Id { get; set; }
        public string StatusName { get; set; } = string.Empty;
    }

    public class ChangeStatusCreateDto
    {
        public string StatusName { get; set; } = string.Empty;
    }

    public class ChangeStatusUpdateDto
    {
        public string StatusName { get; set; } = string.Empty;
    }
}