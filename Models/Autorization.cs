
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace MccApi.Models
{
    
        [Table("Autorization")]
        public class Autorization
        {
            [Key]
            [Column("IdEmployee")]
            public int EmployeeId { get; set; }

            [Required]
            [Column("Password")]
            public string Password { get; set; } = string.Empty;

            [Required]
            [Column("Login")]
            public string Login { get; set; } = string.Empty;

            [Required]
            [Column("IdRole")]
            public int RoleId { get; set; }

            // Навигационные свойства
            [ForeignKey("EmployeeId")]
            public virtual Employee Employee { get; set; } = null!;

            [ForeignKey("RoleId")]
            public virtual Roles Role { get; set; } = null!;
        }
    
}
