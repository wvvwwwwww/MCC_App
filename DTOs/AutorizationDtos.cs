namespace MccApi.DTOs
{
    public class AutorizationReadDto
    {
        public int EmployeeId { get; set; }
        public string Password { get; set; } = string.Empty;
        public string Login { get; set; } = string.Empty;
        public int RoleId { get; set; }
        public string? EmployeeName { get; set; }
        public string? RoleName { get; set; }
    }

    public class AutorizationCreateDto
    {
        public int EmployeeId { get; set; }
        public string Password { get; set; } = string.Empty;
        public string Login { get; set; } = string.Empty;
        public int RoleId { get; set; }
    }

    public class AutorizationUpdateDto
    {
        public string Password { get; set; } = string.Empty;
        public string Login { get; set; } = string.Empty;
        public int RoleId { get; set; }
    }
}
