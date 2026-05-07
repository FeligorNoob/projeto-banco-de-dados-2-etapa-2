using BarberApplication.Console.Models;

namespace BarberApplication.Console.Services;

public interface IUsuarioService
{
    Task<IEnumerable<UsuarioModel>> GetAllAsync();
    Task<UsuarioModel?> GetByIdAsync(int idUsuario);
    Task<UsuarioModel> CreateAsync(UsuarioModel model);
    Task<UsuarioModel?> UpdateAsync(int idUsuario, UsuarioModel model);
    Task<int> ContarAtendimentosAsync(int idUsuario);
    Task<bool> DeleteAsync(int idUsuario);
}
