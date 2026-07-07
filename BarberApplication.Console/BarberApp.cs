using Microsoft.Extensions.Configuration;
using BarberApplication.Console.Data;
using BarberApplication.Console.Data.MySql;
using BarberApplication.Console.Data.Mongo;
using BarberApplication.Console.Services;
using BarberApplication.Console.Menus;

namespace BarberApplication.Console;

/// <summary>
/// Classe principal que orquestra a aplicação console da barbearia.
/// Utiliza o padrão Factory Method (IDatabaseFactory) para instanciar os services,
/// permitindo trocar facilmente de banco de dados (ex: MySQL → MongoDB).
/// </summary>
public class BarberApp : ConsoleMenuBase
{
    private readonly IConsoleMenu _clienteConsole;
    private readonly IConsoleMenu _profissionalConsole;
    private readonly IConsoleMenu _administradorConsole;
    private readonly IConsoleMenu _crudConsole;

    private BarberApp(
        IUsuarioService usuarioService,
        ICargoService cargoService,
        IEspecialidadeService especialidadeService,
        IProfissionalEspecialidadeService profEspService,
        IServicoService servicoService,
        IAtendimentoService atendimentoService)
        : base(usuarioService, cargoService, especialidadeService, profEspService, servicoService, atendimentoService)
    {
        _clienteConsole = new ClienteConsole(usuarioService, cargoService, especialidadeService, profEspService, servicoService, atendimentoService);
        _profissionalConsole = new ProfissionalConsole(usuarioService, cargoService, especialidadeService, profEspService, servicoService, atendimentoService);
        _administradorConsole = new AdministradorConsole(usuarioService, cargoService, especialidadeService, profEspService, servicoService, atendimentoService);
        _crudConsole = new CentralCrudConsole(usuarioService, cargoService, especialidadeService, profEspService, servicoService, atendimentoService);
    }

    /// <summary>
    /// Cria e executa a aplicação console.
    /// Usa o Factory Method para instanciar os services de acesso ao banco.
    /// O banco (MySQL ou MongoDB) é escolhido pelo usuário na inicialização.
    /// </summary>
    public static async Task RunAsync()
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false)
            .Build();

        // ===== FACTORY METHOD =====
        // Seleção do banco em tempo de execução. As duas conexões ficam no appsettings.json.
        using IDatabaseFactory? factory = CriarFactory(configuration);
        if (factory is null) return; // usuário optou por sair

        var app = new BarberApp(
            factory.CreateUsuarioService(),
            factory.CreateCargoService(),
            factory.CreateEspecialidadeService(),
            factory.CreateProfissionalEspecialidadeService(),
            factory.CreateServicoService(),
            factory.CreateAtendimentoService()
        );

        await app.ExibirAsync();
    }

    /// <summary>
    /// Exibe o menu de seleção de banco e cria a IDatabaseFactory correspondente.
    /// Repete a pergunta se a conexão falhar. Retorna null se o usuário optar por sair.
    /// </summary>
    private static IDatabaseFactory? CriarFactory(IConfiguration configuration)
    {
        while (true)
        {
            try { System.Console.Clear(); } catch { }
            System.Console.WriteLine();
            System.Console.WriteLine("  ╔══════════════════════════════════════════════╗");
            System.Console.WriteLine("  ║  BarberFlow — Seleção de Banco de Dados       ║");
            System.Console.WriteLine("  ╚══════════════════════════════════════════════╝");
            System.Console.WriteLine();
            System.Console.WriteLine("  Qual banco de dados deseja utilizar?");
            System.Console.WriteLine("  1. MySQL   (Entity Framework Core)");
            System.Console.WriteLine("  2. MongoDB (driver oficial)");
            System.Console.WriteLine("  0. Sair");
            System.Console.WriteLine();
            System.Console.Write("  Opção: ");
            var opcao = System.Console.ReadLine()?.Trim();

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

    /// <summary>
    /// Loop principal do menu da aplicação.
    /// </summary>
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
                    case "1": await _clienteConsole.ExibirAsync(); break;
                    case "2": await _profissionalConsole.ExibirAsync(); break;
                    case "3": await _administradorConsole.ExibirAsync(); break;
                    case "4": await _crudConsole.ExibirAsync(); break;
                    case "0": return;
                    default: MsgErro("Opção inválida!"); break;
                }
            });
            if (opcao == "0") { Msg("Até logo!"); return; }
        }
    }
}
