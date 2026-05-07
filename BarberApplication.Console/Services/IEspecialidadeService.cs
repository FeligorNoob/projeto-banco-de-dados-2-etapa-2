using BarberApplication.Console.Models;

namespace BarberApplication.Console.Services;

public interface IEspecialidadeService
{
    Task<IEnumerable<EspecialidadeModel>> GetAllAsync();
    Task<EspecialidadeModel?> GetByIdAsync(int idEspecialidade);
    Task<EspecialidadeModel> CreateAsync(EspecialidadeModel model);
    Task<EspecialidadeModel?> UpdateAsync(int idEspecialidade, EspecialidadeModel model);
    Task<(int vinculos, int servicos)> ContarDependenciasAsync(int idEspecialidade);
    Task<bool> DeleteAsync(int idEspecialidade);
}
