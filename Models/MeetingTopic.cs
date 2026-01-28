using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MccApi.Models
{
    public class MeetingTopic
    {
        [Key]
        [Column("IdMeetingTopic")]
        public int Id { get; set; }

        [Required]
        [Column("TopicName")]
        public string TopicName { get; set; } = string.Empty;

        // Навигационные свойства
        public virtual ICollection<Meeting> Meetings { get; set; } = new List<Meeting>();
    }
}
