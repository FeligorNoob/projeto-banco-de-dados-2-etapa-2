using BarberApplication.Console.Services;

namespace BarberApplication.Console.Menus;

/// <summary>
/// Classe abstrata base para todos os menus do console.
/// Contém apenas os helpers de UI compartilhados — não armazena services.
/// </summary>
public abstract class ConsoleMenuBase : IConsoleMenu
{
    /// <summary>
    /// Método principal do menu. Deve ser implementado por cada console concreto.
    /// </summary>
    public abstract Task ExibirAsync();

    // =====================================================================
    //  HELPERS DE UI
    // =====================================================================

    /// <summary>
    /// Limpa o console de forma segura (ignora erros caso o console não suporte clear).
    /// </summary>
    protected static void LimparConsole()
    {
        try { System.Console.Clear(); } catch { }
    }

    protected static void Titulo(string texto)
    {
        System.Console.WriteLine();
        System.Console.ForegroundColor = ConsoleColor.Cyan;
        System.Console.WriteLine("  ╔" + new string('═', texto.Length + 4) + "╗");
        System.Console.WriteLine("  ║  " + texto + "  ║");
        System.Console.WriteLine("  ╚" + new string('═', texto.Length + 4) + "╝");
        System.Console.ResetColor();
        System.Console.WriteLine();
    }

    protected static void Separador()
    {
        System.Console.ForegroundColor = ConsoleColor.DarkGray;
        System.Console.WriteLine("  " + new string('─', 65));
        System.Console.ResetColor();
    }

    protected static string LerOpcao()
    {
        System.Console.ForegroundColor = ConsoleColor.Cyan;
        System.Console.Write("  ? ");
        System.Console.ResetColor();
        System.Console.Write("Opção: ");
        return System.Console.ReadLine()?.Trim() ?? "";
    }

    protected static string LerInput(string mensagem)
    {
        System.Console.ForegroundColor = ConsoleColor.DarkGray;
        System.Console.WriteLine("\n  [Digite 0 para Voltar/Cancelar]");
        System.Console.ForegroundColor = ConsoleColor.Cyan;
        System.Console.Write($"  ? ");
        System.Console.ResetColor();
        System.Console.Write($"{mensagem}: ");
        return System.Console.ReadLine()?.Trim() ?? "";
    }
	protected static string LerInputEdicao(string mensagem)
	{
		System.Console.ForegroundColor = ConsoleColor.DarkGray;
		System.Console.WriteLine("\n  [Digite 0 para Pular este campo]");
		System.Console.ForegroundColor = ConsoleColor.Cyan;
		System.Console.Write($"  ? ");
		System.Console.ResetColor();
		System.Console.Write($"{mensagem}: ");
		return System.Console.ReadLine()?.Trim() ?? "";
	}

	protected void Msg(string texto)
    {
        System.Console.ForegroundColor = ConsoleColor.Green;
        System.Console.WriteLine($"\n  ✔ {texto}");
        System.Console.ResetColor();
        Pausar();
    }

    protected void MsgErro(string texto)
    {
        System.Console.ForegroundColor = ConsoleColor.Red;
        System.Console.WriteLine($"\n  ✖ {texto}");
        System.Console.ResetColor();
        Pausar();
    }

    protected static void MsgAviso(string texto)
    {
        System.Console.ForegroundColor = ConsoleColor.Yellow;
        System.Console.WriteLine($"\n  ⚠ {texto}");
        System.Console.ResetColor();
    }

    protected static void Pausar()
    {
        System.Console.ForegroundColor = ConsoleColor.DarkGray;
        System.Console.Write("\n  Pressione qualquer tecla para continuar...");
        System.Console.ResetColor();
        System.Console.ReadKey(true);
    }

    /// <summary>
    /// Exibe aviso de que dados relacionados serão removidos e pede confirmação.
    /// Retorna true se o usuário confirmar (digitar 's').
    /// </summary>
    protected static bool ConfirmarRemocaoComDados(string itemNome, string descricaoDados)
    {
        System.Console.WriteLine();
        System.Console.ForegroundColor = ConsoleColor.Yellow;
        System.Console.WriteLine($"  ⚠ ATENÇÃO: '{itemNome}' possui dados relacionados:");
        System.Console.WriteLine($"  ⚠ {descricaoDados}");
        System.Console.WriteLine("  ⚠ Todos esses dados serão REMOVIDOS permanentemente!");
        System.Console.ResetColor();
        System.Console.ForegroundColor = ConsoleColor.Cyan;
        System.Console.Write("\n  Deseja realmente continuar? (s/N): ");
        System.Console.ResetColor();
        var resp = System.Console.ReadLine()?.Trim().ToLower();
        return resp == "s";
    }

    /// <summary>
    /// Converte o código de status do atendimento para texto legível.
    /// </summary>
    protected static string FormatarStatus(string status) => status switch
    {
        "A" => "Agendado",
        "R" => "Realizado",
        "C" => "Cancelado",
        _ => status
    };

    /// <summary>
    /// Executa uma ação protegida contra erros de conexão com o banco.
    /// </summary>
    protected async Task ExecutarProtegidoAsync(Func<Task> acao)
    {
        try
        {
            await acao();
        }
        catch (MySqlConnector.MySqlException)
        {
            MsgErro("Falha de conexão com o banco de dados. Verifique se o MySQL está rodando.");
        }
        catch (Exception ex)
        {
            MsgErro($"Ocorreu um erro inesperado: {ex.Message}");
        }
    }

    /// <summary>
    /// Exibe a listagem de serviços (compartilhado entre Cliente, Admin e Central de Dados).
    /// Recebe os services por parâmetro, já que a base não os armazena mais.
    /// </summary>
    protected async Task VisualizarServicos(IServicoService servicoService, IEspecialidadeService especialidadeService)
    {
        LimparConsole();
        Titulo("Serviços Disponíveis");
        var servicos = (await servicoService.GetAllAsync()).ToList();
        var especialidades = (await especialidadeService.GetAllAsync()).ToList();

        if (servicos.Count == 0) { Msg("Nenhum serviço cadastrado."); return; }

        System.Console.WriteLine($"  {"ID",-5} | {"Serviço",-25} | {"Preço",-12} | {"Duração",-12} | {"Especialidade",-20}");
        Separador();
        foreach (var s in servicos)
        {
            var espNome = especialidades.FirstOrDefault(e => e.IdEspecialidade == s.FkEspecialidade)?.Nome ?? "-";
            System.Console.WriteLine($"  {s.IdServico,-5} | {s.Nome,-25} | R${s.Preco,-10:F2} | {s.DuracaoEstimada + " min",-12} | {espNome,-20}");
        }
        Pausar();
    }
}
