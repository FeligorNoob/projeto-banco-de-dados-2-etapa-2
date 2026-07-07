using Microsoft.EntityFrameworkCore;
using BarberApplication.Console.Models;

namespace BarberApplication.Console.Data.MySql;

public class BarbeariaContext : DbContext
{
    public BarbeariaContext(DbContextOptions<BarbeariaContext> options) : base(options) { }

    public DbSet<UsuarioModel> Usuarios => Set<UsuarioModel>();
    public DbSet<CargoModel> Cargos => Set<CargoModel>();
    public DbSet<EspecialidadeModel> Especialidades => Set<EspecialidadeModel>();
    public DbSet<ProfissionalEspecialidadeModel> ProfissionalEspecialidades => Set<ProfissionalEspecialidadeModel>();
    public DbSet<ServicoModel> Servicos => Set<ServicoModel>();
    public DbSet<AtendimentoModel> Atendimentos => Set<AtendimentoModel>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configuração para Chave Primária Composta
        modelBuilder.Entity<ProfissionalEspecialidadeModel>()
            .HasKey(pe => new { pe.FkProfissional, pe.FkEspecialidade });

        // Configuração de Índice Único (UNIQUE) no CPF
        modelBuilder.Entity<UsuarioModel>()
            .HasIndex(u => u.Cpf)
            .IsUnique();

        // Relacionamento Usuario -> Cargo
        modelBuilder.Entity<UsuarioModel>()
            .HasOne(u => u.Cargo)
            .WithMany()
            .HasForeignKey(u => u.IdCargo)
            .OnDelete(DeleteBehavior.Restrict);

        // Relacionamento Atendimento -> Cliente/Profissional/Servico
        modelBuilder.Entity<AtendimentoModel>()
            .HasOne(a => a.Cliente)
            .WithMany()
            .HasForeignKey(a => a.FkCliente)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<AtendimentoModel>()
            .HasOne(a => a.Profissional)
            .WithMany()
            .HasForeignKey(a => a.FkProfissional)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<AtendimentoModel>()
            .HasOne(a => a.Servico)
            .WithMany()
            .HasForeignKey(a => a.FkServico)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
