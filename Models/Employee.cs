using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net;


namespace MccApi.Models
{
    public class Employee
    {
        
            [Key]
            [Column("IdEmployee")]
            public int Id { get; set; }

            [Column("idTitle")]
            public int? TitleId { get; set; }

            [Column("Standing")]
            public int Standing { get; set; }

            [Column("photo")]
            public string? Photo { get; set; }

            [Required]
            [Column("Name")]
            public string Name { get; set; } = string.Empty;

            [Column("SecondName")]
            public string? SecondName { get; set; }

            [Required]
            [Column("Number")]
            public string Number { get; set; } = string.Empty;

            [Required]
            [Column("PasportData")]
            public string PasportData { get; set; } = string.Empty;

            // Навигационные свойства
            public virtual Title? Title { get; set; }
            public virtual ICollection<Autorization> Autorizations { get; set; } = new List<Autorization>();
            public virtual ICollection<Change> Changes { get; set; } = new List<Change>();
            public virtual ICollection<EmployeeSchedule> EmployeeSchedules { get; set; } = new List<EmployeeSchedule>();
            public virtual ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();
            public virtual ICollection<ChangesHistory> ChangesHistories { get; set; } = new List<ChangesHistory>();
            public virtual ICollection<MeetingAttend> MeetingAttends { get; set; } = new List<MeetingAttend>();
        }
    }
