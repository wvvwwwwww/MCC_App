using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MccApi.Models
{
    public class Schedule
    {
        [Key]
        [Column("IdSchedule")]
        public int Id { get; set; }

        [Required]
        [Column("IdDayOfWeek")]
        public int DayOfWeekId { get; set; }

        [Required]
        [Column("IsBar")]
        public bool IsBar { get; set; }

        [Required]
        [Column("IdPoint")]
        public int PointId { get; set; }

        [Required]
        [Column("IdEmployeer")]
        public int EmployeeId { get; set; }

        [Required]
        [Column("TimeOfStart")]
        public TimeSpan TimeOfStart { get; set; }

        [Required]
        [Column("TimeOfEnd")]
        public TimeSpan TimeOfEnd { get; set; }

        [Column("date")]
        public DateOnly? Date { get; set; }

        // Навигационные свойства
        [ForeignKey("DayOfWeekId")]
        public virtual WeekDay DayOfWeek { get; set; } = null!;

        [ForeignKey("PointId")]
        public virtual Point Point { get; set; } = null!;

        [ForeignKey("EmployeeId")]
        public virtual Employee Employee { get; set; } = null!;

        public virtual ICollection<ChangesHistory> ChangesHistories { get; set; } = new List<ChangesHistory>();
    }
}
