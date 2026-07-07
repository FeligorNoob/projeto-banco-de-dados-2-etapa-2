using MongoDB.Driver;
using BarberApplication.Console.Data.Mongo;
using BarberApplication.Console.Models;

namespace BarberApplication.Console.Services.Mongo;

public class ProfissionalEspecialidadeMongoService(BarbeariaMongoContext context) : IProfissionalEspecialidadeService
{
    public async Task<IEnumerable<ProfissionalEspecialidadeModel>> GetAllAsync() =>
        await context.ProfissionalEspecialidades.Find(_ => true).ToListAsync();

    public async Task<ProfissionalEspecialidadeModel?> GetByIdAsync(int fkProfissional, int fkEspecialidade) =>
        await context.ProfissionalEspecialidades
            .Find(pe => pe.FkProfissional == fkProfissional && pe.FkEspecialidade == fkEspecialidade)
            .FirstOrDefaultAsync();

    public async Task<ProfissionalEspecialidadeModel> CreateAsync(ProfissionalEspecialidadeModel model)
    {
        // Respeita a unicidade da chave composta (equivalente à PK composta no MySQL).
        bool jaExiste = await context.ProfissionalEspecialidades
            .Find(pe => pe.FkProfissional == model.FkProfissional && pe.FkEspecialidade == model.FkEspecialidade)
            .AnyAsync();
        if (jaExiste)
            throw new ArgumentException("Este profissional já está vinculado a esta especialidade.");

        await context.ProfissionalEspecialidades.InsertOneAsync(model);
        return model;
    }

    public async Task<bool> DeleteAsync(int fkProfissional, int fkEspecialidade)
    {
        var result = await context.ProfissionalEspecialidades.DeleteOneAsync(
            pe => pe.FkProfissional == fkProfissional && pe.FkEspecialidade == fkEspecialidade);
        return result.DeletedCount > 0;
    }
}
