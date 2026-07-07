using MongoDB.Driver;
using BarberApplication.Console.Data.Mongo;
using BarberApplication.Console.Models;

namespace BarberApplication.Console.Services.Mongo;

public class CargoMongoService(BarbeariaMongoContext context) : ICargoService
{
    public async Task<IEnumerable<CargoModel>> GetAllAsync() =>
        await context.Cargos.Find(_ => true).ToListAsync();

    public async Task<CargoModel?> GetByIdAsync(int idCargo) =>
        await context.Cargos.Find(c => c.IdCargo == idCargo).FirstOrDefaultAsync();

    public async Task<CargoModel> CreateAsync(CargoModel model)
    {
        model.IdCargo = await context.GetNextSequenceAsync("cargo");
        await context.Cargos.InsertOneAsync(model);
        return model;
    }

    public async Task<CargoModel?> UpdateAsync(int idCargo, CargoModel model)
    {
        var existing = await context.Cargos.Find(c => c.IdCargo == idCargo).FirstOrDefaultAsync();
        if (existing == null) return null;

        existing.Nome = model.Nome;

        await context.Cargos.ReplaceOneAsync(c => c.IdCargo == idCargo, existing);
        return existing;
    }

    public async Task<bool> DeleteAsync(int idCargo)
    {
        var result = await context.Cargos.DeleteOneAsync(c => c.IdCargo == idCargo);
        return result.DeletedCount > 0;
    }
}
