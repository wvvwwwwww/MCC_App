namespace MyCoffeeCupApp.DTOs
{
    public class RolesReadDto
    {
        public int Id { get; set; }
        public string RoleName { get; set; } = string.Empty;
    }

    public class RolesCreateDto
    {
        public string RoleName { get; set; } = string.Empty;
    }

    public class RolesUpdateDto
    {
        public string RoleName { get; set; } = string.Empty;
    }
}
