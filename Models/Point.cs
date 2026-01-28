using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MccApi.Models
{
    public class Point
    {
        [Key]
        [Column("IdPoint")]
        public int Id { get; set; }

        [Required]
        [Column("Address")]
        public string Address { get; set; } = string.Empty;

        [Required]
        [Column("Start")]
        public TimeSpan Start { get; set; }

        [Required]
        [Column("End")]
        public TimeSpan End { get; set; }

        [Required]
        [Column("Kitchen")]
        public bool Kitchen { get; set; }

        [Required]
        [Column("GeneralEmployee")]
        public int GeneralEmployeeId { get; set; }

        // Навигационные свойства
        [ForeignKey("GeneralEmployeeId")]
        public virtual Employee GeneralEmployee { get; set; } = null!;

        public virtual ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();
        public virtual ICollection<ChangesHistory> ChangesHistories { get; set; } = new List<ChangesHistory>();
        public virtual ICollection<Meeting> Meetings { get; set; } = new List<Meeting>();
    }
}
