namespace MyCoffeeCupApp.DTOs
{
    public class WeekDayReadDto // ← Изменили здесь
    {
        public int Id { get; set; }
        public bool? IsDayOfInventory { get; set; }
        public string DayOfWeekName { get; set; } = string.Empty;
    }

    public class WeekDayCreateDto // ← Изменили здесь
    {
        public bool? IsDayOfInventory { get; set; }
        public string DayOfWeekName { get; set; } = string.Empty;
    }

    public class WeekDayUpdateDto // ← Изменили здесь
    {
        public bool? IsDayOfInventory { get; set; }
        public string DayOfWeekName { get; set; } = string.Empty;
    }

}
