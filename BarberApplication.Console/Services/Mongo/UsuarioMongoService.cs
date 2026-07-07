using MongoDB.Driver;
using BarberApplication.Console.Data.Mongo;
using BarberApplication.Console.Models;

namespace BarberApplication.Console.Services.Mongo;

public class UsuarioMongoService(BarbeariaMongoContext context) : IUsuarioService
{
    public async Task<IEnumerable<UsuarioModel>> GetAllAsync()
    {
        var usuarios = await context.Usuarios.Find(_ => true).ToListAsync();
        // Preenche a navegação Cargo (equivalente ao Include do EF).
        var cargos = await context.Cargos.Find(_ => true).ToListAsync();
        foreach (var u in usuarios)
            u.Cargo = cargos.FirstOrDefault(c => c.IdCargo == u.IdCargo);
        return usuarios;
    }

    public async Task<UsuarioModel?> GetByIdAsync(int idUsuario)
    {
        var usuario = await context.Usuarios.Find(u => u.IdUsuario == idUsuario).FirstOrDefaultAsync();
        if (usuario != null)
            usuario.Cargo = await context.Cargos.Find(c => c.IdCargo == usuario.IdCargo).FirstOrDefaultAsync();
        return usuario;
    }

    public async Task<UsuarioModel> CreateAsync(UsuarioModel model)
    {
        bool cpfExiste = await context.Usuarios.Find(u => u.Cpf == model.Cpf).AnyAsync();
        if (cpfExiste)
            throw new ArgumentException("Já existe um usuário cadastrado com este CPF.");

        model.IdUsuario = await context.GetNextSequenceAsync("usuario");
        await context.Usuarios.InsertOneAsync(model);
        return model;
    }

    public async Task<UsuarioModel?> UpdateAsync(int idUsuario, UsuarioModel model)
    {
        var existing = await context.Usuarios.Find(u => u.IdUsuario == idUsuario).FirstOrDefaultAsync();
        if (existing == null) return null;

        existing.Cpf = model.Cpf;
        existing.Nome = model.Nome;
        existing.Telefone = model.Telefone;
        existing.EndRua = model.EndRua;
        existing.EndBairro = model.EndBairro;
        existing.EndCep = model.EndCep;
        existing.IdCargo = model.IdCargo;

        await context.Usuarios.ReplaceOneAsync(u => u.IdUsuario == idUsuario, existing);
        return existing;
    }

    /// <summary>
    /// Conta quantos atendimentos (como cliente OU profissional) estão ligados a este usuário.
    /// </summary>
    public async Task<int> ContarAtendimentosAsync(int idUsuario) =>
        (int)await context.Atendimentos.CountDocumentsAsync(a =>
            a.FkCliente == idUsuario || a.FkProfissional == idUsuario);

    /// <summary>
    /// Remove o usuário e TODOS os seus atendimentos relacionados (como cliente e como profissional).
    /// Também remove os vínculos de especialidade caso seja profissional.
    /// </summary>
    public async Task<bool> DeleteAsync(int idUsuario)
    {
        var existing = await context.Usuarios.Find(u => u.IdUsuario == idUsuario).FirstOrDefaultAsync();
        if (existing == null) return false;

        await context.Atendimentos.DeleteManyAsync(a =>
            a.FkCliente == idUsuario || a.FkProfissional == idUsuario);

        await context.ProfissionalEspecialidades.DeleteManyAsync(pe => pe.FkProfissional == idUsuario);

        await context.Usuarios.DeleteOneAsync(u => u.IdUsuario == idUsuario);
        return true;
    }
}
