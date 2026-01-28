namespace MccApi.DTOs
{
    public class ScheduleDtos
    {
        public class ScheduleReadDto
        {
            public int Id { get; set; }
            public int DayOfWeekId { get; set; }
            public bool IsBar { get; set; }
            public int PointId { get; set; }
            public int EmployeeId { get; set; }
            public TimeSpan TimeOfStart { get; set; }
            public TimeSpan TimeOfEnd { get; set; }
            public DateOnly? Date { get; set; }
            public string? DayOfWeekName { get; set; }
            public string? EmployeeName { get; set; }
            public string? PointAddress { get; set; }
        }

        public class ScheduleCreateDto
        {
            public int DayOfWeekId { get; set; }
            public bool IsBar { get; set; }
            public int PointId { get; set; }
            public int EmployeeId { get; set; }
            public TimeSpan TimeOfStart { get; set; }
            public TimeSpan TimeOfEnd { get; set; }
            public DateOnly? Date { get; set; }
        }

        public class ScheduleUpdateDto
        {
            public int DayOfWeekId { get; set; }
            public bool IsBar { get; set; }
            public int PointId { get; set; }
            public int EmployeeId { get; set; }
            public TimeSpan TimeOfStart { get; set; }
            public TimeSpan TimeOfEnd { get; set; }
            public DateOnly? Date { get; set; }
        }
    }
}
