using BarberApplication.Console.Models;
using BarberApplication.Console.Services;

namespace BarberApplication.Console.Menus;

/// <summary>
/// Menu do console para a área do Profissional.
/// </summary>
public class ProfissionalConsole(
    IUsuarioService usuarioService,
    IEspecialidadeService especialidadeService,
    IProfissionalEspecialidadeService profEspService,
    IAtendimentoService atendimentoService)
    : ConsoleMenuBase
{
    // Cargo 2 = Profissional
    private const int CargoProfissional = 2;

    public override async Task ExibirAsync()
    {
        LimparConsole();
        Titulo("Área do Profissional");

        var cpf = LerInput("Digite seu CPF (11 dígitos)");
        if (cpf == "0") return;

        var usuarios = (await usuarioService.GetAllAsync()).ToList();
        var profissional = usuarios.FirstOrDefault(u => u.Cpf == cpf);

        if (profissional == null) { MsgErro("CPF não encontrado."); return; }
        if (profissional.IdCargo != CargoProfissional) { MsgErro("Este CPF não pertence a um profissional."); return; }

        Msg($"Bem-vindo(a), {profissional.Nome}!");
        await SubMenu(profissional);
    }

    private async Task SubMenu(UsuarioModel profissional)
    {
        while (true)
        {
            LimparConsole();
            Titulo($"Profissional: {profissional.Nome} (CPF: {profissional.Cpf})");
            System.Console.WriteLine("  1. Meus Atendimentos (Agendados)");
            System.Console.WriteLine("  2. Todos os meus Atendimentos");
            System.Console.WriteLine("  3. Marcar Atendimento como Realizado");
            System.Console.WriteLine("  4. Minhas Especialidades");
            System.Console.WriteLine("  0. Voltar");
            Separador();

            var opcao = LerOpcao();
            await ExecutarProtegidoAsync(async () =>
            {
                switch (opcao)
                {
                    case "1": await VerAgendados(profissional); break;
                    case "2": await VerTodos(profissional); break;
                    case "3": await MarcarRealizado(profissional); break;
                    case "4": await VerEspecialidades(profissional); break;
                    case "0": return;
                    default: MsgErro("Opção inválida!"); break;
                }
            });
            if (opcao == "0") return;
        }
    }

    private async Task VerAgendados(UsuarioModel profissional)
    {
        LimparConsole();
        Titulo("Atendimentos Agendados");
        var agendados = (await atendimentoService.GetAllAsync())
            .Where(a => a.FkProfissional == profissional.IdUsuario && a.StatusAtendimento == "A")
            .OrderBy(a => a.DataHora).ToList();

        if (agendados.Count == 0) { Msg("Sem atendimentos agendados."); return; }

        System.Console.WriteLine($"  {"ID",-5} {"Cliente",-20} {"Serviço",-20} {"Data/Hora",-18}");
        Separador();
        foreach (var a in agendados)
            System.Console.WriteLine($"  {a.IdAtendimento,-5} {(a.Cliente?.Nome ?? "-"),-20} {(a.Servico?.Nome ?? "-"),-20} {a.DataHora:dd/MM/yyyy HH:mm}");
        Pausar();
    }

    private async Task VerTodos(UsuarioModel profissional)
    {
        LimparConsole();
        Titulo("Todos os meus Atendimentos");
        var meus = (await atendimentoService.GetAllAsync())
            .Where(a => a.FkProfissional == profissional.IdUsuario)
            .OrderByDescending(a => a.DataHora).ToList();

        if (meus.Count == 0) { Msg("Nenhum atendimento encontrado."); return; }

        System.Console.WriteLine($"  {"ID",-5} {"Cliente",-20} {"Serviço",-20} {"Data/Hora",-18} {"Status",-10}");
        Separador();
        foreach (var a in meus)
            System.Console.WriteLine($"  {a.IdAtendimento,-5} {(a.Cliente?.Nome ?? "-"),-20} {(a.Servico?.Nome ?? "-"),-20} {a.DataHora:dd/MM/yyyy HH:mm}  {FormatarStatus(a.StatusAtendimento),-10}");
        Pausar();
    }

    private async Task MarcarRealizado(UsuarioModel profissional)
    {
        LimparConsole();
        Titulo("Marcar Atendimento como Realizado");
        var agendados = (await atendimentoService.GetAllAsync())
            .Where(a => a.FkProfissional == profissional.IdUsuario && a.StatusAtendimento == "A").ToList();

        if (agendados.Count == 0) { Msg("Sem atendimentos agendados."); return; }

        for (int i = 0; i < agendados.Count; i++)
            System.Console.WriteLine($"  {i + 1}. {agendados[i].Cliente?.Nome ?? "-"} - {agendados[i].Servico?.Nome ?? "-"} em {agendados[i].DataHora:dd/MM/yyyy HH:mm}");

        var inIdx = LerInput("Escolha qual marcar como realizado");
        if (inIdx == "0") return;
        if (!int.TryParse(inIdx, out int idx) || idx < 1 || idx > agendados.Count) { MsgErro("Opção inválida!"); return; }

        await atendimentoService.AtualizarStatusAsync(agendados[idx - 1].IdAtendimento, "R");
        Msg("Atendimento marcado como realizado!");
    }

    private async Task VerEspecialidades(UsuarioModel profissional)
    {
        LimparConsole();
        Titulo($"Especialidades de {profissional.Nome}");
        var profEsps = (await profEspService.GetAllAsync()).ToList();
        var especialidades = (await especialidadeService.GetAllAsync()).ToList();

        var minhasEspIds = profEsps.Where(pe => pe.FkProfissional == profissional.IdUsuario)
                                    .Select(pe => pe.FkEspecialidade).ToHashSet();
        var minhas = especialidades.Where(e => minhasEspIds.Contains(e.IdEspecialidade)).ToList();

        if (minhas.Count == 0) { Msg("Nenhuma especialidade vinculada."); return; }
        foreach (var e in minhas)
            System.Console.WriteLine($"  - {e.Nome}");
        Pausar();
    }
}
