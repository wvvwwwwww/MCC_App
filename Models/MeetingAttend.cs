using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MccApi.Models
{
    public class MeetingAttend
    {
        [Key]
        [Column("IdMeetingAttend")]
        public int Id { get; set; }

        [Required]
        [Column("IdMeeting")]
        public int MeetingId { get; set; }

        [Required]
        [Column("IdEmployee")]
        public int EmployeeId { get; set; }

        // Навигационные свойства
        [ForeignKey("MeetingId")]
        public virtual Meeting Meeting { get; set; } = null!;

        [ForeignKey("EmployeeId")]
        public virtual Employee Employee { get; set; } = null!;
    }
}
