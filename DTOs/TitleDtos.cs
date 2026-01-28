namespace MccApi.DTOs
{
    public class TitleReadDto
    {
        public int Id { get; set; }
        public int Bid { get; set; }
        public string? TitleName { get; set; }
    }

    public class TitleCreateDto
    {
        public int Bid { get; set; }
        public string? TitleName { get; set; }
    }

    public class TitleUpdateDto
    {
        public int Bid { get; set; }
        public string? TitleName { get; set; }
    }
}
