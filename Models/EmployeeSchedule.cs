using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MccApi.Models
{
    public class EmployeeSchedule
    {
        [Key]
        [Column("IdEmployeeSchedule")]
        public int Id { get; set; }

        [Required]
        [Column("Date")]
        public DateOnly Date { get; set; }

        [Column("Note")]
        public string? Note { get; set; }

        [Column("IdEmployee")]
        public int? EmployeeId { get; set; }

        [Column("TimeOfStart")]
        public TimeSpan TimeOfStart { get; set; } = new TimeSpan(7, 30, 0);

        [Column("TimeOfEnd")]
        public TimeSpan TimeOfEnd { get; set; } = new TimeSpan(22, 30, 0);

        // Навигационные свойства
        [ForeignKey("EmployeeId")]
        public virtual Employee? Employee { get; set; }

        public virtual ICollection<Change> Changes { get; set; } = new List<Change>();
    }
}
