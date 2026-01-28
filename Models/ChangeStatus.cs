using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MccApi.Models
{
    public class ChangeStatus
    {
        [Key]
        [Column("IdStatus")]
        public int Id { get; set; }

        [Required]
        [Column("StatusName")]
        public string StatusName { get; set; } = string.Empty;

        // Навигационные свойства
        public virtual ICollection<Change> Changes { get; set; } = new List<Change>();
    }
}
