using BarberApplication.Console.Models;

namespace BarberApplication.Console.Services;

public interface ICargoService
{
    Task<IEnumerable<CargoModel>> GetAllAsync();
    Task<CargoModel?> GetByIdAsync(int idCargo);
    Task<CargoModel> CreateAsync(CargoModel model);
    Task<CargoModel?> UpdateAsync(int idCargo, CargoModel model);
    Task<bool> DeleteAsync(int idCargo);
}
