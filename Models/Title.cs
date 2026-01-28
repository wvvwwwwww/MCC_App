using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MccApi.Models
{
    public class Title
    {
        [Key]
        [Column("idTitle")]
        public int Id { get; set; }

        [Required]
        [Column("bid")]
        public int Bid { get; set; }

        [Column("titleName")]
        public string? TitleName { get; set; }

        // Навигационные свойства
        public virtual ICollection<Employee> Employee { get; set; } = new List<Employee>();
    }
}
