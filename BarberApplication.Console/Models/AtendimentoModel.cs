using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BarberApplication.Console.Models;

[Table("atendimento")]
public class AtendimentoModel
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id_atendimento")]
    public int IdAtendimento { get; set; }

    [Column("fk_profissional")]
    public int? FkProfissional { get; set; }

    [Column("fk_cliente")]
    public int? FkCliente { get; set; }

    [Column("fk_servico")]
    public int? FkServico { get; set; }

    [Required]
    [Column("data_hora")]
    public DateTime DataHora { get; set; }

    /// <summary>
    /// A=Agendado, R=Realizado, C=Cancelado
    /// </summary>
    [Column("status_atendimento")]
    [StringLength(1)]
    public string StatusAtendimento { get; set; } = "A";

    [ForeignKey(nameof(FkProfissional))]
    public virtual UsuarioModel? Profissional { get; set; }

    [ForeignKey(nameof(FkCliente))]
    public virtual UsuarioModel? Cliente { get; set; }

    [ForeignKey(nameof(FkServico))]
    public virtual ServicoModel? Servico { get; set; }
}
