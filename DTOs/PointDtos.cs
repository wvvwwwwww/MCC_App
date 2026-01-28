namespace MccApi.DTOs
{
    public class PointDtos
    {
        public class PointReadDto
        {
            public int Id { get; set; }
            public string Address { get; set; } = string.Empty;
            public TimeSpan Start { get; set; }
            public TimeSpan End { get; set; }
            public bool Kitchen { get; set; }
            public int GeneralEmployeeId { get; set; }
            public string? GeneralEmployeeName { get; set; }
        }

        public class PointCreateDto
        {
            public string Address { get; set; } = string.Empty;
            public TimeSpan Start { get; set; }
            public TimeSpan End { get; set; }
            public bool Kitchen { get; set; } = false;
            public int GeneralEmployeeId { get; set; }
        }

        public class PointUpdateDto
        {
            public string Address { get; set; } = string.Empty;
            public TimeSpan Start { get; set; }
            public TimeSpan End { get; set; }
            public bool Kitchen { get; set; }
            public int GeneralEmployeeId { get; set; }
        }
    }
}
