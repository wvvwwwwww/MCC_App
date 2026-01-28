using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MccApi.Models
{
    public class MeetingStatus
    {
        [Key]
        [Column("IdMeetingStatus")]
        public int Id { get; set; }

        [Required]
        [Column("StatusName")]
        public string StatusName { get; set; } = string.Empty;

        // Навигационные свойства
        public virtual ICollection<Meeting> Meetings { get; set; } = new List<Meeting>();
    }
}
