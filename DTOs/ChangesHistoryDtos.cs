namespace MyCoffeeCupApp.DTOs
{
    public class ChangesHistoryReadDto
    {
        public int Id { get; set; }
        public int ScheduleId { get; set; }
        public int EmployeeId { get; set; }
        public TimeSpan OldStartTime { get; set; }
        public TimeSpan OldEndTime { get; set; }
        public int PointId { get; set; }
        public bool IsBar { get; set; }
        public string? EmployeeName { get; set; }
        public string? PointAddress { get; set; }
    }

    public class ChangesHistoryCreateDto
    {
        public int ScheduleId { get; set; }
        public int EmployeeId { get; set; }
        public TimeSpan OldStartTime { get; set; }
        public TimeSpan OldEndTime { get; set; }
        public int PointId { get; set; }
        public bool IsBar { get; set; }
    }

    public class ChangesHistoryUpdateDto
    {
        public int ScheduleId { get; set; }
        public int EmployeeId { get; set; }
        public TimeSpan OldStartTime { get; set; }
        public TimeSpan OldEndTime { get; set; }
        public int PointId { get; set; }
        public bool IsBar { get; set; }
    }
}
