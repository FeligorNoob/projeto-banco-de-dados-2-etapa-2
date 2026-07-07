using MongoDB.Driver;
using BarberApplication.Console.Data.Mongo;
using BarberApplication.Console.Models;

namespace BarberApplication.Console.Services.Mongo;

public class AtendimentoMongoService(BarbeariaMongoContext context) : IAtendimentoService
{
    public async Task<IEnumerable<AtendimentoModel>> GetAllAsync()
    {
        var atendimentos = await context.Atendimentos.Find(_ => true).ToListAsync();
        await PopularRelacionamentosAsync(atendimentos);
        return atendimentos;
    }

    public async Task<AtendimentoModel?> GetByIdAsync(int idAtendimento)
    {
        var atendimento = await context.Atendimentos.Find(a => a.IdAtendimento == idAtendimento).FirstOrDefaultAsync();
        if (atendimento != null)
            await PopularRelacionamentosAsync([atendimento]);
        return atendimento;
    }

    public async Task<AtendimentoModel> CreateAsync(AtendimentoModel model)
    {
        model.IdAtendimento = await context.GetNextSequenceAsync("atendimento");
        await context.Atendimentos.InsertOneAsync(model);
        return model;
    }

    public async Task<AtendimentoModel?> UpdateAsync(int idAtendimento, AtendimentoModel model)
    {
        var existing = await context.Atendimentos.Find(a => a.IdAtendimento == idAtendimento).FirstOrDefaultAsync();
        if (existing == null) return null;

        existing.FkProfissional = model.FkProfissional;
        existing.FkCliente = model.FkCliente;
        existing.FkServico = model.FkServico;
        existing.DataHora = model.DataHora;
        existing.StatusAtendimento = model.StatusAtendimento;

        await context.Atendimentos.ReplaceOneAsync(a => a.IdAtendimento == idAtendimento, existing);
        return existing;
    }

    public async Task<bool> DeleteAsync(int idAtendimento)
    {
        var result = await context.Atendimentos.DeleteOneAsync(a => a.IdAtendimento == idAtendimento);
        return result.DeletedCount > 0;
    }

    public async Task AtualizarStatusAsync(int id, string novoStatus)
    {
        await context.Atendimentos.UpdateOneAsync(
            a => a.IdAtendimento == id,
            Builders<AtendimentoModel>.Update.Set(a => a.StatusAtendimento, novoStatus));
    }

    /// <summary>
    /// Preenche as navegações Cliente, Profissional e Servico (equivalente aos Include do EF).
    /// </summary>
    private async Task PopularRelacionamentosAsync(List<AtendimentoModel> atendimentos)
    {
        if (atendimentos.Count == 0) return;

        var usuarios = await context.Usuarios.Find(_ => true).ToListAsync();
        var servicos = await context.Servicos.Find(_ => true).ToListAsync();

        foreach (var a in atendimentos)
        {
            a.Profissional = usuarios.FirstOrDefault(u => u.IdUsuario == a.FkProfissional);
            a.Cliente = usuarios.FirstOrDefault(u => u.IdUsuario == a.FkCliente);
            a.Servico = servicos.FirstOrDefault(s => s.IdServico == a.FkServico);
        }
    }
}
