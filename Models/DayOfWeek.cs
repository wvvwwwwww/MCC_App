using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MccApi.Models
{
    [Table("DayOfWeek")] 
    public class WeekDay
    {
        [Key]
        [Column("IdDayOfWeek")]
        public int Id { get; set; }

        [Column("IsDayOfInventory")]
        public bool? IsDayOfInventory { get; set; }

        [Column("DayOfWeek")]
        public string DayOfWeekName { get; set; } = string.Empty;

        // Навигационные свойства
        public virtual ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();
    }
}
