using Microsoft.EntityFrameworkCore;
using BarberApplication.Console.Data.MySql;
using BarberApplication.Console.Models;
using BarberApplication.Console.Services;

namespace BarberApplication.Console.Services.MySql;

public class CargoService(BarbeariaContext context) : ICargoService
{
    public async Task<IEnumerable<CargoModel>> GetAllAsync() => await context.Cargos.ToListAsync();
    
    public async Task<CargoModel?> GetByIdAsync(int idCargo) => await context.Cargos.FindAsync(idCargo);
    
    public async Task<CargoModel> CreateAsync(CargoModel model)
    {
        context.Cargos.Add(model);
        await context.SaveChangesAsync();
        return model;
    }
    
    public async Task<CargoModel?> UpdateAsync(int idCargo, CargoModel model)
    {
        var existing = await context.Cargos.FindAsync(idCargo);
        if (existing == null) return null;
        
        existing.Nome = model.Nome;
        
        await context.SaveChangesAsync();
        return existing;
    }
    
    public async Task<bool> DeleteAsync(int idCargo)
    {
        var model = await context.Cargos.FindAsync(idCargo);
        if (model == null) return false;
        
        context.Cargos.Remove(model);
        await context.SaveChangesAsync();
        return true;
    }
}
