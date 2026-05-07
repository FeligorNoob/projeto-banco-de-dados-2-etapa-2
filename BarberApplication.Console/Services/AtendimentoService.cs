using Microsoft.EntityFrameworkCore;
using BarberApplication.Console.Data;
using BarberApplication.Console.Models;

namespace BarberApplication.Console.Services;

public class AtendimentoService(BarbeariaContext context) : IAtendimentoService
{
    public async Task<IEnumerable<AtendimentoModel>> GetAllAsync() => 
        await context.Atendimentos
            .Include(a => a.Cliente)
            .Include(a => a.Profissional)
            .Include(a => a.Servico)
            .ToListAsync();
    
    public async Task<AtendimentoModel?> GetByIdAsync(int idAtendimento) => 
        await context.Atendimentos
            .Include(a => a.Cliente)
            .Include(a => a.Profissional)
            .Include(a => a.Servico)
            .FirstOrDefaultAsync(a => a.IdAtendimento == idAtendimento);
    
    public async Task<AtendimentoModel> CreateAsync(AtendimentoModel model)
    {
        context.Atendimentos.Add(model);
        await context.SaveChangesAsync();
        return model;
    }
    
    public async Task<AtendimentoModel?> UpdateAsync(int idAtendimento, AtendimentoModel model)
    {
        var existing = await context.Atendimentos.FindAsync(idAtendimento);
        if (existing == null) return null;
        
        existing.FkProfissional = model.FkProfissional;
        existing.FkCliente = model.FkCliente;
        existing.FkServico = model.FkServico;
        existing.DataHora = model.DataHora;
        existing.StatusAtendimento = model.StatusAtendimento;
        
        await context.SaveChangesAsync();
        return existing;
    }
    
    public async Task<bool> DeleteAsync(int idAtendimento)
    {
        var model = await context.Atendimentos.FindAsync(idAtendimento);
        if (model == null) return false;
        
        context.Atendimentos.Remove(model);
        await context.SaveChangesAsync();
        return true;
    }

    public async Task AtualizarStatusAsync(int id, string novoStatus)
    {
        var atend = await context.Atendimentos.FindAsync(id);
        if (atend != null)
        {
            atend.StatusAtendimento = novoStatus;
            await context.SaveChangesAsync();
        }
    }
}
