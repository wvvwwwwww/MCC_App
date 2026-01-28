using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MccApi.Models
{
    public class Change
    {
        [Key]
        [Column("IdChange")]
        public int Id { get; set; }

        [Required]
        [Column("IdEmployee")]
        public int EmployeeId { get; set; }

        [Required]
        [Column("TimeOfStar")]
        public TimeSpan TimeOfStart { get; set; }

        [Required]
        [Column("TimeOfEnd")]
        public TimeSpan TimeOfEnd { get; set; }

        [Required]
        [Column("IdStatus")]
        public int StatusId { get; set; }

        [Column("IdEmployeeShedule")]
        public int? EmployeeScheduleId { get; set; }

        [Column("Notes")]
        public string? Notes { get; set; }

        // Навигационные свойства
        [ForeignKey("EmployeeId")]
        public virtual Employee Employee { get; set; } = null!;

        [ForeignKey("StatusId")]
        public virtual ChangeStatus ChangeStatus { get; set; } = null!;

        [ForeignKey("EmployeeScheduleId")]
        public virtual EmployeeSchedule? EmployeeSchedule { get; set; }
    }
}
