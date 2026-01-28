using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MccApi.Models
{
    public class Meeting
    {
        [Key]
        [Column("IdMeeting")]
        public int Id { get; set; }

        [Required]
        [Column("Date")]
        public DateOnly Date { get; set; }

        [Required]
        [Column("IdPoint")]
        public int PointId { get; set; }

        [Column("IdStatus")]
        public int? StatusId { get; set; }

        [Required]
        [Column("IdMeetingTopic")]
        public int MeetingTopicId { get; set; }

        // Навигационные свойства
        [ForeignKey("PointId")]
        public virtual Point Point { get; set; } = null!;

        [ForeignKey("StatusId")]
        public virtual MeetingStatus? MeetingStatus { get; set; }

        [ForeignKey("MeetingTopicId")]
        public virtual MeetingTopic MeetingTopic { get; set; } = null!;

        public virtual ICollection<MeetingAttend> MeetingAttends { get; set; } = new List<MeetingAttend>();
    }
}
