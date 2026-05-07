using BarberApplication.Console.Models;

namespace BarberApplication.Console.Services;

public interface IProfissionalEspecialidadeService
{
    Task<IEnumerable<ProfissionalEspecialidadeModel>> GetAllAsync();
    Task<ProfissionalEspecialidadeModel?> GetByIdAsync(int fkProfissional, int fkEspecialidade);
    Task<ProfissionalEspecialidadeModel> CreateAsync(ProfissionalEspecialidadeModel model);
    Task<bool> DeleteAsync(int fkProfissional, int fkEspecialidade);
}
