using Microsoft.EntityFrameworkCore;
using BarberApplication.Console.Data.MySql;
using BarberApplication.Console.Models;
using BarberApplication.Console.Services;

namespace BarberApplication.Console.Services.MySql;

public class ServicoService(BarbeariaContext context) : IServicoService
{
    public async Task<IEnumerable<ServicoModel>> GetAllAsync() => await context.Servicos.ToListAsync();

    public async Task<ServicoModel?> GetByIdAsync(int idServico) => await context.Servicos.FindAsync(idServico);

    public async Task<ServicoModel> CreateAsync(ServicoModel model)
    {
        context.Servicos.Add(model);
        await context.SaveChangesAsync();
        return model;
    }

    public async Task<ServicoModel?> UpdateAsync(int idServico, ServicoModel model)
    {
        var existing = await context.Servicos.FindAsync(idServico);
        if (existing == null) return null;

        existing.Nome = model.Nome;
        existing.FkEspecialidade = model.FkEspecialidade;
        existing.Preco = model.Preco;
        existing.DuracaoEstimada = model.DuracaoEstimada;

        await context.SaveChangesAsync();
        return existing;
    }

    /// <summary>
    /// Conta quantos atendimentos fazem referência a este serviço.
    /// </summary>
    public async Task<int> ContarAtendimentosAsync(int idServico) =>
        await context.Atendimentos.CountAsync(a => a.FkServico == idServico);

    /// <summary>
    /// Remove o serviço e TODOS os atendimentos relacionados.
    /// </summary>
    public async Task<bool> DeleteAsync(int idServico)
    {
        var model = await context.Servicos.FindAsync(idServico);
        if (model == null) return false;

        // Remove atendimentos relacionados primeiro
        var atendimentos = await context.Atendimentos
            .Where(a => a.FkServico == idServico)
            .ToListAsync();
        context.Atendimentos.RemoveRange(atendimentos);

        context.Servicos.Remove(model);
        await context.SaveChangesAsync();
        return true;
    }
}
