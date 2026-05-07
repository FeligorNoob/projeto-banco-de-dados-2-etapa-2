using Microsoft.EntityFrameworkCore;
using BarberApplication.Console.Data;
using BarberApplication.Console.Models;

namespace BarberApplication.Console.Services;

public class ProfissionalEspecialidadeService(BarbeariaContext context) : IProfissionalEspecialidadeService
{
    public async Task<IEnumerable<ProfissionalEspecialidadeModel>> GetAllAsync() => await context.ProfissionalEspecialidades.AsNoTracking().ToListAsync();
    
    public async Task<ProfissionalEspecialidadeModel?> GetByIdAsync(int fkProfissional, int fkEspecialidade) => 
        await context.ProfissionalEspecialidades.FirstOrDefaultAsync(pe => pe.FkProfissional == fkProfissional && pe.FkEspecialidade == fkEspecialidade);
    
    public async Task<ProfissionalEspecialidadeModel> CreateAsync(ProfissionalEspecialidadeModel model)
    {
        try
        {
            context.ProfissionalEspecialidades.Add(model);
            await context.SaveChangesAsync();
            return model;
        }
        catch
        {
            context.Entry(model).State = EntityState.Detached;
            throw;
        }
    }
    
    public async Task<bool> DeleteAsync(int fkProfissional, int fkEspecialidade)
    {
        var model = await context.ProfissionalEspecialidades.FirstOrDefaultAsync(pe => pe.FkProfissional == fkProfissional && pe.FkEspecialidade == fkEspecialidade);
        if (model == null) return false;
        
        context.ProfissionalEspecialidades.Remove(model);
        await context.SaveChangesAsync();
        return true;
    }
}
