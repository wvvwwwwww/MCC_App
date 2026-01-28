namespace MccApi.DTOs
{
    public class EmployeeScheduleReadDto
    {
        public int Id { get; set; }
        public DateOnly Date { get; set; }
        public string? Note { get; set; }
        public int? EmployeeId { get; set; }
        public TimeSpan TimeOfStart { get; set; }
        public TimeSpan TimeOfEnd { get; set; }
        public string? EmployeeName { get; set; }
    }

    public class EmployeeScheduleCreateDto
    {
        public DateOnly Date { get; set; }
        public string? Note { get; set; }
        public int? EmployeeId { get; set; }
        public TimeSpan TimeOfStart { get; set; } = new TimeSpan(7, 30, 0);
        public TimeSpan TimeOfEnd { get; set; } = new TimeSpan(22, 30, 0);
    }

    public class EmployeeScheduleUpdateDto
    {
        public DateOnly Date { get; set; }
        public string? Note { get; set; }
        public int? EmployeeId { get; set; }
        public TimeSpan TimeOfStart { get; set; }
        public TimeSpan TimeOfEnd { get; set; }
    }
}
