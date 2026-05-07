using Microsoft.EntityFrameworkCore;
using BarberApplication.Console.Data;
using BarberApplication.Console.Models;

namespace BarberApplication.Console.Services;

public class EspecialidadeService(BarbeariaContext context) : IEspecialidadeService
{
    public async Task<IEnumerable<EspecialidadeModel>> GetAllAsync() => await context.Especialidades.ToListAsync();

    public async Task<EspecialidadeModel?> GetByIdAsync(int idEspecialidade) => await context.Especialidades.FindAsync(idEspecialidade);

    public async Task<EspecialidadeModel> CreateAsync(EspecialidadeModel model)
    {
        context.Especialidades.Add(model);
        await context.SaveChangesAsync();
        return model;
    }

    public async Task<EspecialidadeModel?> UpdateAsync(int idEspecialidade, EspecialidadeModel model)
    {
        var existing = await context.Especialidades.FindAsync(idEspecialidade);
        if (existing == null) return null;

        existing.Nome = model.Nome;

        await context.SaveChangesAsync();
        return existing;
    }

    /// <summary>
    /// Conta quantos vínculos (profissional_especialidade) e serviços dependem desta especialidade.
    /// </summary>
    public async Task<(int vinculos, int servicos)> ContarDependenciasAsync(int idEspecialidade)
    {
        int vinculos = await context.ProfissionalEspecialidades
            .CountAsync(pe => pe.FkEspecialidade == idEspecialidade);
        int servicos = await context.Servicos
            .CountAsync(s => s.FkEspecialidade == idEspecialidade);
        return (vinculos, servicos);
    }

    /// <summary>
    /// Remove a especialidade, desvinculando profissionais e limpando FkEspecialidade dos serviços.
    /// NÃO remove os serviços — apenas desassocia a especialidade deles.
    /// </summary>
    public async Task<bool> DeleteAsync(int idEspecialidade)
    {
        var model = await context.Especialidades.FindAsync(idEspecialidade);
        if (model == null) return false;

        // Remove vínculos profissional_especialidade
        var vinculos = await context.ProfissionalEspecialidades
            .Where(pe => pe.FkEspecialidade == idEspecialidade)
            .ToListAsync();
        context.ProfissionalEspecialidades.RemoveRange(vinculos);

        // Desassocia especialidade dos serviços (seta null)
        var servicos = await context.Servicos
            .Where(s => s.FkEspecialidade == idEspecialidade)
            .ToListAsync();
        foreach (var s in servicos)
            s.FkEspecialidade = null;

        context.Especialidades.Remove(model);
        await context.SaveChangesAsync();
        return true;
    }
}
