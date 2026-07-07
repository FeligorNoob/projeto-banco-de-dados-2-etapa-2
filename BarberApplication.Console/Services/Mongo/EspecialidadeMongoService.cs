using MongoDB.Driver;
using BarberApplication.Console.Data.Mongo;
using BarberApplication.Console.Models;

namespace BarberApplication.Console.Services.Mongo;

public class EspecialidadeMongoService(BarbeariaMongoContext context) : IEspecialidadeService
{
    public async Task<IEnumerable<EspecialidadeModel>> GetAllAsync() =>
        await context.Especialidades.Find(_ => true).ToListAsync();

    public async Task<EspecialidadeModel?> GetByIdAsync(int idEspecialidade) =>
        await context.Especialidades.Find(e => e.IdEspecialidade == idEspecialidade).FirstOrDefaultAsync();

    public async Task<EspecialidadeModel> CreateAsync(EspecialidadeModel model)
    {
        model.IdEspecialidade = await context.GetNextSequenceAsync("especialidade");
        await context.Especialidades.InsertOneAsync(model);
        return model;
    }

    public async Task<EspecialidadeModel?> UpdateAsync(int idEspecialidade, EspecialidadeModel model)
    {
        var existing = await context.Especialidades.Find(e => e.IdEspecialidade == idEspecialidade).FirstOrDefaultAsync();
        if (existing == null) return null;

        existing.Nome = model.Nome;

        await context.Especialidades.ReplaceOneAsync(e => e.IdEspecialidade == idEspecialidade, existing);
        return existing;
    }

    /// <summary>
    /// Conta quantos vínculos (profissional_especialidade) e serviços dependem desta especialidade.
    /// </summary>
    public async Task<(int vinculos, int servicos)> ContarDependenciasAsync(int idEspecialidade)
    {
        int vinculos = (int)await context.ProfissionalEspecialidades
            .CountDocumentsAsync(pe => pe.FkEspecialidade == idEspecialidade);
        int servicos = (int)await context.Servicos
            .CountDocumentsAsync(s => s.FkEspecialidade == idEspecialidade);
        return (vinculos, servicos);
    }

    /// <summary>
    /// Remove a especialidade, desvinculando profissionais e limpando FkEspecialidade dos serviços.
    /// NÃO remove os serviços — apenas desassocia a especialidade deles.
    /// </summary>
    public async Task<bool> DeleteAsync(int idEspecialidade)
    {
        var existing = await context.Especialidades.Find(e => e.IdEspecialidade == idEspecialidade).FirstOrDefaultAsync();
        if (existing == null) return false;

        // Remove vínculos profissional_especialidade
        await context.ProfissionalEspecialidades.DeleteManyAsync(pe => pe.FkEspecialidade == idEspecialidade);

        // Desassocia especialidade dos serviços (seta null)
        await context.Servicos.UpdateManyAsync(
            s => s.FkEspecialidade == idEspecialidade,
            Builders<ServicoModel>.Update.Set(s => s.FkEspecialidade, (int?)null));

        await context.Especialidades.DeleteOneAsync(e => e.IdEspecialidade == idEspecialidade);
        return true;
    }
}
