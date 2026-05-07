using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BarberApplication.Console.Models;

[Table("servico")]
public class ServicoModel
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id_servico")]
    public int IdServico { get; set; }

    [Required]
    [StringLength(50)]
    [Column("nome")]
    public required string Nome { get; set; }

    [Column("fk_especialidade")]
    public int? FkEspecialidade { get; set; }

    [Required]
    [Column("preco")]
    public decimal Preco { get; set; }

    [Required]
    [Column("duracao_estimada")]
    public int DuracaoEstimada { get; set; }
}
