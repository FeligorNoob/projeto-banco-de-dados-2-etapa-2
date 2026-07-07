using MongoDB.Driver;
using BarberApplication.Console.Services;
using BarberApplication.Console.Services.Mongo;

namespace BarberApplication.Console.Data.Mongo;

/// <summary>
/// Factory Method para MongoDB usando o driver oficial MongoDB.Driver.
/// Cria o BarbeariaMongoContext e instancia todos os services com ele.
/// Espelha a MySqlDatabaseFactory, permitindo trocar de banco sem alterar a UI.
/// </summary>
public class MongoDatabaseFactory : IDatabaseFactory
{
    private readonly BarbeariaMongoContext _context;

    public MongoDatabaseFactory(string connectionString, string databaseName)
    {
        MongoMappings.Register();

        var client = new MongoClient(connectionString);
        var database = client.GetDatabase(databaseName);
        _context = new BarbeariaMongoContext(database);

        // Garante os índices (também valida a conexão logo na inicialização).
        _context.EnsureIndexesAsync().GetAwaiter().GetResult();
    }

    public IUsuarioService CreateUsuarioService() => new UsuarioMongoService(_context);
    public ICargoService CreateCargoService() => new CargoMongoService(_context);
    public IEspecialidadeService CreateEspecialidadeService() => new EspecialidadeMongoService(_context);
    public IProfissionalEspecialidadeService CreateProfissionalEspecialidadeService() => new ProfissionalEspecialidadeMongoService(_context);
    public IServicoService CreateServicoService() => new ServicoMongoService(_context);
    public IAtendimentoService CreateAtendimentoService() => new AtendimentoMongoService(_context);

    public void Dispose()
    {
        // O MongoClient gerencia o pool de conexões internamente; nada a liberar explicitamente.
        GC.SuppressFinalize(this);
    }
}
