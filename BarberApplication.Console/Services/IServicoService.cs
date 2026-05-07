using BarberApplication.Console.Models;

namespace BarberApplication.Console.Services;

public interface IServicoService
{
    Task<IEnumerable<ServicoModel>> GetAllAsync();
    Task<ServicoModel?> GetByIdAsync(int idServico);
    Task<ServicoModel> CreateAsync(ServicoModel model);
    Task<ServicoModel?> UpdateAsync(int idServico, ServicoModel model);
    Task<int> ContarAtendimentosAsync(int idServico);
    Task<bool> DeleteAsync(int idServico);
}
