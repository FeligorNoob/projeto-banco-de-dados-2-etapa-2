using Microsoft.EntityFrameworkCore;
using BarberApplication.Console.Data.MySql;
using BarberApplication.Console.Models;
using BarberApplication.Console.Services;

namespace BarberApplication.Console.Services.MySql;

public class UsuarioService(BarbeariaContext context) : IUsuarioService
{
    public async Task<IEnumerable<UsuarioModel>> GetAllAsync() =>
        await context.Usuarios.Include(u => u.Cargo).ToListAsync();

    public async Task<UsuarioModel?> GetByIdAsync(int idUsuario) =>
        await context.Usuarios.Include(u => u.Cargo).FirstOrDefaultAsync(u => u.IdUsuario == idUsuario);

    public async Task<UsuarioModel> CreateAsync(UsuarioModel model)
    {
        bool cpfExiste = await context.Usuarios.AnyAsync(u => u.Cpf == model.Cpf);
        if (cpfExiste)
            throw new ArgumentException("Já existe um usuário cadastrado com este CPF.");

        context.Usuarios.Add(model);
        await context.SaveChangesAsync();
        return model;
    }

    public async Task<UsuarioModel?> UpdateAsync(int idUsuario, UsuarioModel model)
    {
        var existing = await context.Usuarios.FindAsync(idUsuario);
        if (existing == null) return null;

        existing.Cpf = model.Cpf;
        existing.Nome = model.Nome;
        existing.Telefone = model.Telefone;
        existing.EndRua = model.EndRua;
        existing.EndBairro = model.EndBairro;
        existing.EndCep = model.EndCep;
        existing.IdCargo = model.IdCargo;

        await context.SaveChangesAsync();
        return existing;
    }

    /// <summary>
    /// Conta quantos atendimentos (como cliente OU profissional) estão ligados a este usuário.
    /// </summary>
    public async Task<int> ContarAtendimentosAsync(int idUsuario) =>
        await context.Atendimentos.CountAsync(a =>
            a.FkCliente == idUsuario || a.FkProfissional == idUsuario);

    /// <summary>
    /// Remove o usuário e TODOS os seus atendimentos relacionados (como cliente e como profissional).
    /// Também remove os vínculos de especialidade caso seja profissional.
    /// </summary>
    public async Task<bool> DeleteAsync(int idUsuario)
    {
        var model = await context.Usuarios.FindAsync(idUsuario);
        if (model == null) return false;

        // Remove atendimentos relacionados
        var atendimentos = await context.Atendimentos
            .Where(a => a.FkCliente == idUsuario || a.FkProfissional == idUsuario)
            .ToListAsync();
        context.Atendimentos.RemoveRange(atendimentos);

        // Remove vínculos de especialidade (caso seja profissional)
        var vinculos = await context.ProfissionalEspecialidades
            .Where(pe => pe.FkProfissional == idUsuario)
            .ToListAsync();
        context.ProfissionalEspecialidades.RemoveRange(vinculos);

        context.Usuarios.Remove(model);
        await context.SaveChangesAsync();
        return true;
    }
}
