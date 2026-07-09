using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using BarberApplication.Console.Data;
using BarberApplication.Console.Data.MySql;
using BarberApplication.Console.Data.Mongo;
using BarberApplication.Console.Menus;

namespace BarberApplication.Console;

public class BarberApp(
    ClienteConsole clienteConsole,
    ProfissionalConsole profissionalConsole,
    AdministradorConsole administradorConsole,
    CentralCrudConsole crudConsole)
    : ConsoleMenuBase
{
    public static async Task RunAsync()
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false)
            .Build();

        using var factory = CriarFactory(configuration);
        if (factory is null)
            return;

        await using var provider = ConfigurarServicos(configuration, factory);
        var app = provider.GetRequiredService<BarberApp>();
        await app.ExibirAsync();
    }

    private static ServiceProvider ConfigurarServicos(IConfiguration configuration, IDatabaseFactory factory)
    {
        var services = new ServiceCollection();

        services.AddSingleton(configuration);
        services.AddSingleton(factory);

        services.AddSingleton(sp => sp.GetRequiredService<IDatabaseFactory>().CreateUsuarioService());
        services.AddSingleton(sp => sp.GetRequiredService<IDatabaseFactory>().CreateCargoService());
        services.AddSingleton(sp => sp.GetRequiredService<IDatabaseFactory>().CreateEspecialidadeService());
        services.AddSingleton(sp => sp.GetRequiredService<IDatabaseFactory>().CreateProfissionalEspecialidadeService());
        services.AddSingleton(sp => sp.GetRequiredService<IDatabaseFactory>().CreateServicoService());
        services.AddSingleton(sp => sp.GetRequiredService<IDatabaseFactory>().CreateAtendimentoService());

        services.AddSingleton<ClienteConsole>();
        services.AddSingleton<ProfissionalConsole>();
        services.AddSingleton<AdministradorConsole>();
        services.AddSingleton<CentralCrudConsole>();
        services.AddSingleton<BarberApp>();

        return services.BuildServiceProvider();
    }

    private static IDatabaseFactory? CriarFactory(IConfiguration configuration)
    {
        while (true)
        {
            try { System.Console.Clear(); } catch { }
            System.Console.WriteLine();
            System.Console.WriteLine("  ╔══════════════════════════════════════════════╗");
            System.Console.WriteLine("  ║  BarberFlow — Seleção de Banco de Dados      ║");
            System.Console.WriteLine("  ╚══════════════════════════════════════════════╝");
            System.Console.WriteLine();
            System.Console.WriteLine("  Qual banco de dados deseja utilizar?");
            System.Console.WriteLine("  1. MySQL   (Entity Framework Core)");
            System.Console.WriteLine("  2. MongoDB (driver oficial)");
            System.Console.WriteLine("  0. Sair");
            System.Console.WriteLine();
            var opcao = LerOpcao();

            try
            {
                switch (opcao)
                {
                    case "1":
                        var mysqlConn = configuration.GetConnectionString("MySql")
                            ?? throw new InvalidOperationException("Connection string 'MySql' não configurada no appsettings.json.");
                        return new MySqlDatabaseFactory(mysqlConn);

                    case "2":
                        var mongoConn = configuration.GetConnectionString("Mongo")
                            ?? throw new InvalidOperationException("Connection string 'Mongo' não configurada no appsettings.json.");
                        var mongoDb = configuration["MongoDatabase"]
                            ?? throw new InvalidOperationException("'MongoDatabase' não configurado no appsettings.json.");
                        return new MongoDatabaseFactory(mongoConn, mongoDb);

                    case "0":
                        return null;

                    default:
                        System.Console.WriteLine("\n  Opção inválida! Pressione qualquer tecla para tentar novamente...");
                        System.Console.ReadKey(true);
                        break;
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("\n  Não foi possível conectar ao banco selecionado:");
                System.Console.WriteLine($"  {ex.Message}");
                System.Console.WriteLine("\n  Verifique o appsettings.json e se o servidor está no ar.");
                System.Console.WriteLine("  Pressione qualquer tecla para voltar à seleção...");
                System.Console.ReadKey(true);
            }
        }
    }

    public override async Task ExibirAsync()
    {
        while (true)
        {
            LimparConsole();
            Titulo("Sistema de Agendamento para Barbearia");
            System.Console.WriteLine("  1. Visualizar como Cliente");
            System.Console.WriteLine("  2. Visualizar como Profissional");
            System.Console.WriteLine("  3. Visualizar como Administrador");
            System.Console.WriteLine("  4. Central de Dados (CRUDs Base)");
            System.Console.WriteLine("  0. Sair");
            Separador();

            var opcao = LerOpcao();
            await ExecutarProtegidoAsync(async () =>
            {
                switch (opcao)
                {
                    case "1": await clienteConsole.ExibirAsync(); break;
                    case "2": await profissionalConsole.ExibirAsync(); break;
                    case "3": await administradorConsole.ExibirAsync(); break;
                    case "4": await crudConsole.ExibirAsync(); break;
                    case "0": return;
                    default: MsgErro("Opção inválida!"); break;
                }
            });

            if (opcao != "0")
                continue;

            Msg("Até logo!");
            return;
        }
    }
}
