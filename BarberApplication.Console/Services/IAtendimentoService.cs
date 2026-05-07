using BarberApplication.Console.Models;

namespace BarberApplication.Console.Services;

public interface IAtendimentoService
{
    Task<IEnumerable<AtendimentoModel>> GetAllAsync();
    Task<AtendimentoModel?> GetByIdAsync(int idAtendimento);
    Task<AtendimentoModel> CreateAsync(AtendimentoModel model);
    Task<AtendimentoModel?> UpdateAsync(int idAtendimento, AtendimentoModel model);
    Task<bool> DeleteAsync(int idAtendimento);
    Task AtualizarStatusAsync(int id, string novoStatus);
}
