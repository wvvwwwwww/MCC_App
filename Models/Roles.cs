using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MccApi.Models
{
    public class Roles
    {
        [Key]
        [Column("IdRole")]
        public int Id { get; set; }

        [Required]
        [Column("RoleName")]
        public string RoleName { get; set; } = string.Empty;

        // Навигационные свойства
        public virtual ICollection<Autorization> Autorizations { get; set; } = new List<Autorization>();
    }
}

