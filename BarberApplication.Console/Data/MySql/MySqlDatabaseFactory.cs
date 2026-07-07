using Microsoft.EntityFrameworkCore;
using BarberApplication.Console.Services;
using BarberApplication.Console.Services.MySql;

namespace BarberApplication.Console.Data.MySql;

/// <summary>
/// Factory Method para MySQL usando Entity Framework Core + Pomelo.
/// Cria o BarbeariaContext e instancia todos os services com ele.
/// </summary>
public class MySqlDatabaseFactory : IDatabaseFactory
{
    private readonly BarbeariaContext _context;

    public MySqlDatabaseFactory(string connectionString)
    {
        var optionsBuilder = new DbContextOptionsBuilder<BarbeariaContext>();
        optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
        _context = new BarbeariaContext(optionsBuilder.Options);
    }

    public IUsuarioService CreateUsuarioService() => new UsuarioService(_context);
    public ICargoService CreateCargoService() => new CargoService(_context);
    public IEspecialidadeService CreateEspecialidadeService() => new EspecialidadeService(_context);
    public IProfissionalEspecialidadeService CreateProfissionalEspecialidadeService() => new ProfissionalEspecialidadeService(_context);
    public IServicoService CreateServicoService() => new ServicoService(_context);
    public IAtendimentoService CreateAtendimentoService() => new AtendimentoService(_context);

    public void Dispose()
    {
        _context.Dispose();
        GC.SuppressFinalize(this);
    }
}
