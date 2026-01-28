using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MccApi.Models
{
    public class ChangesHistory
    {
        [Key]
        [Column("IdChangesHistory")]
        public int Id { get; set; }

        [Required]
        [Column("IdSchedule")]
        public int ScheduleId { get; set; }

        [Required]
        [Column("IdEmployee")]
        public int EmployeeId { get; set; }

        [Required]
        [Column("OldStartTime")]
        public TimeSpan OldStartTime { get; set; }

        [Required]
        [Column("OLdEndTime")]
        public TimeSpan OldEndTime { get; set; }

        [Required]
        [Column("IdPoint")]
        public int PointId { get; set; }

        [Required]
        [Column("IsBar")]
        public bool IsBar { get; set; }

        // Навигационные свойства
        [ForeignKey("ScheduleId")]
        public virtual Schedule Schedule { get; set; } = null!;

        [ForeignKey("EmployeeId")]
        public virtual Employee Employee { get; set; } = null!;

        [ForeignKey("PointId")]
        public virtual Point Point { get; set; } = null!;
    }
}
