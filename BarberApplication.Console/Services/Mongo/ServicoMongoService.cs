using MongoDB.Driver;
using BarberApplication.Console.Data.Mongo;
using BarberApplication.Console.Models;

namespace BarberApplication.Console.Services.Mongo;

public class ServicoMongoService(BarbeariaMongoContext context) : IServicoService
{
    public async Task<IEnumerable<ServicoModel>> GetAllAsync() =>
        await context.Servicos.Find(_ => true).ToListAsync();

    public async Task<ServicoModel?> GetByIdAsync(int idServico) =>
        await context.Servicos.Find(s => s.IdServico == idServico).FirstOrDefaultAsync();

    public async Task<ServicoModel> CreateAsync(ServicoModel model)
    {
        model.IdServico = await context.GetNextSequenceAsync("servico");
        await context.Servicos.InsertOneAsync(model);
        return model;
    }

    public async Task<ServicoModel?> UpdateAsync(int idServico, ServicoModel model)
    {
        var existing = await context.Servicos.Find(s => s.IdServico == idServico).FirstOrDefaultAsync();
        if (existing == null) return null;

        existing.Nome = model.Nome;
        existing.FkEspecialidade = model.FkEspecialidade;
        existing.Preco = model.Preco;
        existing.DuracaoEstimada = model.DuracaoEstimada;

        await context.Servicos.ReplaceOneAsync(s => s.IdServico == idServico, existing);
        return existing;
    }

    /// <summary>
    /// Conta quantos atendimentos fazem referência a este serviço.
    /// </summary>
    public async Task<int> ContarAtendimentosAsync(int idServico) =>
        (int)await context.Atendimentos.CountDocumentsAsync(a => a.FkServico == idServico);

    /// <summary>
    /// Remove o serviço e TODOS os atendimentos relacionados.
    /// </summary>
    public async Task<bool> DeleteAsync(int idServico)
    {
        var existing = await context.Servicos.Find(s => s.IdServico == idServico).FirstOrDefaultAsync();
        if (existing == null) return false;

        // Remove atendimentos relacionados primeiro
        await context.Atendimentos.DeleteManyAsync(a => a.FkServico == idServico);

        await context.Servicos.DeleteOneAsync(s => s.IdServico == idServico);
        return true;
    }
}
