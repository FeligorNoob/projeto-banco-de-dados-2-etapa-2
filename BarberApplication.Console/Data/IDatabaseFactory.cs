using BarberApplication.Console.Services;

namespace BarberApplication.Console.Data;

/// <summary>
/// Factory Method para criação dos services de acesso ao banco de dados.
/// Para trocar de banco (ex: MongoDB), basta criar uma nova implementação desta interface.
/// </summary>
public interface IDatabaseFactory : IDisposable
{
    IUsuarioService CreateUsuarioService();
    ICargoService CreateCargoService();
    IEspecialidadeService CreateEspecialidadeService();
    IProfissionalEspecialidadeService CreateProfissionalEspecialidadeService();
    IServicoService CreateServicoService();
    IAtendimentoService CreateAtendimentoService();
}
