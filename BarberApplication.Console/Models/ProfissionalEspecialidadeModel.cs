using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BarberApplication.Console.Models;

[Table("profissional_especialidade")]
public class ProfissionalEspecialidadeModel
{
    [Key]
    [Column("fk_profissional", Order = 0)]
    public int FkProfissional { get; set; }

    [Key]
    [Column("fk_especialidade", Order = 1)]
    public int FkEspecialidade { get; set; }
}
