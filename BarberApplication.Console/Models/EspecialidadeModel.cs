using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BarberApplication.Console.Models;

[Table("especialidade")]
public class EspecialidadeModel
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id_especialidade")]
    public int IdEspecialidade { get; set; }

    [Required]
    [StringLength(50)]
    [Column("nome")]
    public required string Nome { get; set; }
}
