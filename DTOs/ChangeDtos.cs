namespace MccApi.DTOs
{
    public class ChangeReadDto
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public TimeSpan TimeOfStart { get; set; }
        public TimeSpan TimeOfEnd { get; set; }
        public int StatusId { get; set; }
        public int? EmployeeScheduleId { get; set; }
        public string? Notes { get; set; }
        public string? EmployeeName { get; set; }
        public string? StatusName { get; set; }
    }

    public class ChangeCreateDto
    {
        public int EmployeeId { get; set; }
        public TimeSpan TimeOfStart { get; set; }
        public TimeSpan TimeOfEnd { get; set; }
        public int StatusId { get; set; }
        public int? EmployeeScheduleId { get; set; }
        public string? Notes { get; set; }
    }

    public class ChangeUpdateDto
    {
        public int EmployeeId { get; set; }
        public TimeSpan TimeOfStart { get; set; }
        public TimeSpan TimeOfEnd { get; set; }
        public int StatusId { get; set; }
        public int? EmployeeScheduleId { get; set; }
        public string? Notes { get; set; }
    }
}
