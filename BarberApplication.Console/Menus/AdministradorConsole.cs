using BarberApplication.Console.Models;
using BarberApplication.Console.Services;

namespace BarberApplication.Console.Menus;

public class AdministradorConsole(
    IUsuarioService UsuarioService,
    IEspecialidadeService EspecialidadeService,
    IProfissionalEspecialidadeService ProfEspService,
    IServicoService ServicoService,
    IAtendimentoService AtendimentoService)
    : ConsoleMenuBase
{
    public override async Task ExibirAsync()
    {
        while (true)
        {
            LimparConsole();
            Titulo("Opções de Administrador");
            System.Console.WriteLine("  ──── Consultas ────");
            System.Console.WriteLine("  1.  Visualizar Todos os Clientes");
            System.Console.WriteLine("  2.  Visualizar Todos os Profissionais");
            System.Console.WriteLine("  3.  Visualizar Especialidades de um Profissional");
            System.Console.WriteLine("  4.  Visualizar Serviços");
            System.Console.WriteLine("  5.  Visualizar Todos os Atendimentos");
            System.Console.WriteLine("  6.  Visualizar Especialidades");
            System.Console.WriteLine();
            System.Console.WriteLine("  ──── Cadastros / Vínculos ────");
            System.Console.WriteLine("  7.  Criar novo Usuário (Cliente)");
            System.Console.WriteLine("  8.  Criar novo Usuário (Profissional)");
            System.Console.WriteLine("  9.  Criar novo Serviço");
            System.Console.WriteLine("  10. Criar nova Especialidade");
            System.Console.WriteLine("  11. Vincular Especialidade a Profissional");
            System.Console.WriteLine("  12. Desvincular Especialidade de Profissional");
            System.Console.WriteLine();
            System.Console.WriteLine("  ──── Remoções ────");
            System.Console.WriteLine("  13. Remover um Usuário");
            System.Console.WriteLine("  14. Remover um Serviço");
            System.Console.WriteLine("  15. Remover um Atendimento");
            System.Console.WriteLine("  16. Remover uma Especialidade");
            System.Console.WriteLine();
            System.Console.WriteLine("  ──── Relatórios ────");
            System.Console.WriteLine("  17. Agenda Geral Detalhada");
            System.Console.WriteLine("  18. Faturamento Total por Barbeiro");
            System.Console.WriteLine("  19. Serviços mais procurados por Bairro");
            System.Console.WriteLine();
            System.Console.WriteLine("  0.  Voltar");
            Separador();

            var opcao = LerOpcao();
            await ExecutarProtegidoAsync(async () =>
            {
                switch (opcao)
                {
                    case "1":  await VerClientes(); break;
                    case "2":  await VerProfissionais(); break;
                    case "3":  await VerEspecialidadesDeProf(); break;
                    case "4":  await VisualizarServicos(ServicoService, EspecialidadeService); break;
                    case "5":  await VerAtendimentos(); break;
                    case "6":  await VerEspecialidades(); break;
                    case "7":  await CriarUsuario(3); break;
                    case "8":  await CriarUsuario(2); break;
                    case "9":  await CriarServico(); break;
                    case "10": await CriarEspecialidade(); break;
                    case "11": await VincularEspecialidade(); break;
                    case "12": await DesvincularEspecialidade(); break;
                    case "13": await RemoverUsuario(); break;
                    case "14": await RemoverServico(); break;
                    case "15": await RemoverAtendimento(); break;
                    case "16": await RemoverEspecialidade(); break;
                    case "17": await RelatorioAgendaGeral(); break;
                    case "18": await RelatorioFaturamento(); break;
                    case "19": await RelatorioServicosPorBairro(); break;
                    case "0":  return;
                    default:   MsgErro("Opção inválida!"); break;
                }
            });
            if (opcao == "0") return;
        }
    }

    // ──── Consultas ────

    private async Task VerClientes()
    {
        LimparConsole();
        Titulo("Todos os Clientes");
        var clientes = (await UsuarioService.GetAllAsync()).Where(u => u.IdCargo == 3).ToList();
        if (clientes.Count == 0) { Msg("Nenhum cliente cadastrado."); return; }
        System.Console.WriteLine($"  {"ID",-5} {"Nome",-30} {"CPF",-15} {"Telefone",-15}");
        Separador();
        foreach (var c in clientes)
            System.Console.WriteLine($"  {c.IdUsuario,-5} {c.Nome,-30} {c.Cpf,-15} {(c.Telefone ?? "-"),-15}");
        Pausar();
    }

    private async Task VerProfissionais()
    {
        LimparConsole();
        Titulo("Todos os Profissionais");
        var profs = (await UsuarioService.GetAllAsync()).Where(u => u.IdCargo == 2).ToList();
        if (profs.Count == 0) { Msg("Nenhum profissional cadastrado."); return; }
        System.Console.WriteLine($"  {"ID",-5} {"Nome",-30} {"CPF",-15} {"Telefone",-15}");
        Separador();
        foreach (var p in profs)
            System.Console.WriteLine($"  {p.IdUsuario,-5} {p.Nome,-30} {p.Cpf,-15} {(p.Telefone ?? "-"),-15}");
        Pausar();
    }

    private async Task VerEspecialidadesDeProf()
    {
        LimparConsole();
        Titulo("Especialidades de um Profissional");
        var profs = (await UsuarioService.GetAllAsync()).Where(u => u.IdCargo == 2).ToList();
        if (profs.Count == 0) { Msg("Nenhum profissional cadastrado."); return; }
        for (int i = 0; i < profs.Count; i++)
            System.Console.WriteLine($"  {i + 1}. {profs[i].Nome}");
        var inp = LerInput("Escolha o profissional");
        if (inp == "0") return;
        if (!int.TryParse(inp, out int idx) || idx < 1 || idx > profs.Count) { MsgErro("Opção inválida!"); return; }
        var prof = profs[idx - 1];
        var profEsps = (await ProfEspService.GetAllAsync()).ToList();
        var esps = (await EspecialidadeService.GetAllAsync()).ToList();
        var espIds = profEsps.Where(pe => pe.FkProfissional == prof.IdUsuario).Select(pe => pe.FkEspecialidade).ToHashSet();
        var espDoProf = esps.Where(e => espIds.Contains(e.IdEspecialidade)).ToList();
        Titulo($"Especialidades de {prof.Nome}");
        if (espDoProf.Count == 0) { Msg("Nenhuma especialidade vinculada."); return; }
        foreach (var e in espDoProf)
            System.Console.WriteLine($"  - {e.Nome}");
        Pausar();
    }

    private async Task VerAtendimentos()
    {
        LimparConsole();
        Titulo("Todos os Atendimentos");
        var atendimentos = (await AtendimentoService.GetAllAsync()).ToList();
        if (atendimentos.Count == 0) { Msg("Nenhum atendimento encontrado."); return; }
        System.Console.WriteLine($"  {"ID",-5} {"Cliente",-20} {"Profissional",-20} {"Serviço",-20} {"Data/Hora",-18} {"Status"}");
        Separador();
        foreach (var a in atendimentos)
            System.Console.WriteLine($"  {a.IdAtendimento,-5} {(a.Cliente?.Nome ?? "-"),-20} {(a.Profissional?.Nome ?? "-"),-20} {(a.Servico?.Nome ?? "-"),-20} {a.DataHora:dd/MM/yyyy HH:mm}  {FormatarStatus(a.StatusAtendimento)}");
        Pausar();
    }

    private async Task VerEspecialidades()
    {
        LimparConsole();
        Titulo("Especialidades");
        var esps = (await EspecialidadeService.GetAllAsync()).ToList();
        if (esps.Count == 0) { Msg("Nenhuma especialidade cadastrada."); return; }
        System.Console.WriteLine($"  {"ID",-5} {"Nome"}");
        Separador();
        foreach (var e in esps)
            System.Console.WriteLine($"  {e.IdEspecialidade,-5} {e.Nome}");
        Pausar();
    }

    // ──── Cadastros / Vínculos ────

    private async Task CriarUsuario(int cargo)
    {
        var nomeCargo = cargo == 2 ? "Profissional" : "Cliente";
        LimparConsole();
        Titulo($"Criar novo {nomeCargo}");
        var cpf = LerInput("CPF (11 dígitos)"); if (cpf == "0") return;
        var nome = LerInput("Nome completo"); if (nome == "0") return;
        var tel = LerInput("Telefone (opcional, Enter para pular)"); if (tel == "0") return;
        var rua = LerInput("Rua (opcional, Enter para pular)"); if (rua == "0") return;
        var bairro = LerInput("Bairro (opcional, Enter para pular)"); if (bairro == "0") return;
        var cep = LerInput("CEP (opcional, Enter para pular)"); if (cep == "0") return;
        try
        {
            var usuario = await UsuarioService.CreateAsync(new UsuarioModel
            {
                Cpf = cpf, Nome = nome, IdCargo = cargo,
                Telefone = string.IsNullOrWhiteSpace(tel) ? null : tel,
                EndRua = string.IsNullOrWhiteSpace(rua) ? null : rua,
                EndBairro = string.IsNullOrWhiteSpace(bairro) ? null : bairro,
                EndCep = string.IsNullOrWhiteSpace(cep) ? null : cep,
            });
            Msg($"{nomeCargo} '{usuario.Nome}' criado com sucesso!");
        }
        catch (ArgumentException ex) { MsgErro(ex.Message); }
    }

    private async Task CriarServico()
    {
        LimparConsole();
        Titulo("Criar novo Serviço");
        var nome = LerInput("Nome do serviço"); if (nome == "0") return;
        var esps = (await EspecialidadeService.GetAllAsync()).ToList();
        int? fkEsp = null;
        if (esps.Count > 0)
        {
            System.Console.WriteLine("  Especialidades disponíveis:");
            for (int i = 0; i < esps.Count; i++)
                System.Console.WriteLine($"    {i + 1}. {esps[i].Nome}");
            var inEsp = LerInput("Escolha a especialidade (ou Enter para nenhuma)");
            if (inEsp == "0") return;
            if (!string.IsNullOrWhiteSpace(inEsp) && int.TryParse(inEsp, out int idxE) && idxE >= 1 && idxE <= esps.Count)
                fkEsp = esps[idxE - 1].IdEspecialidade;
        }
        var inPreco = LerInput("Preço (ex: 50.00)"); if (inPreco == "0") return;
        if (!decimal.TryParse(inPreco.Replace('.', ','), out decimal preco)) { MsgErro("Preço inválido!"); return; }
        var inDur = LerInput("Duração estimada (minutos)"); if (inDur == "0") return;
        if (!int.TryParse(inDur, out int dur)) { MsgErro("Duração inválida!"); return; }
        await ServicoService.CreateAsync(new ServicoModel { Nome = nome, FkEspecialidade = fkEsp, Preco = preco, DuracaoEstimada = dur });
        Msg("Serviço criado com sucesso!");
    }

    private async Task CriarEspecialidade()
    {
        LimparConsole();
        Titulo("Criar nova Especialidade");
        var nome = LerInput("Nome da especialidade"); if (nome == "0") return;
        await EspecialidadeService.CreateAsync(new EspecialidadeModel { Nome = nome });
        Msg("Especialidade criada com sucesso!");
    }

    private async Task VincularEspecialidade()
    {
        LimparConsole();
        Titulo("Vincular Especialidade a Profissional");
        var profs = (await UsuarioService.GetAllAsync()).Where(u => u.IdCargo == 2).ToList();
        if (profs.Count == 0) { MsgErro("Nenhum profissional cadastrado."); return; }
        for (int i = 0; i < profs.Count; i++) System.Console.WriteLine($"  {i + 1}. {profs[i].Nome}");
        var inP = LerInput("Escolha o profissional"); if (inP == "0") return;
        if (!int.TryParse(inP, out int idxP) || idxP < 1 || idxP > profs.Count) { MsgErro("Opção inválida!"); return; }
        var prof = profs[idxP - 1];
        var esps = (await EspecialidadeService.GetAllAsync()).ToList();
        if (esps.Count == 0) { MsgErro("Nenhuma especialidade cadastrada."); return; }
        var vinculadas = (await ProfEspService.GetAllAsync()).Where(pe => pe.FkProfissional == prof.IdUsuario).Select(pe => pe.FkEspecialidade).ToHashSet();
        var disponiveis = esps.Where(e => !vinculadas.Contains(e.IdEspecialidade)).ToList();
        if (disponiveis.Count == 0) { MsgErro("Este profissional já possui todas as especialidades."); return; }
        Titulo($"Especialidades NÃO vinculadas a {prof.Nome}");
        for (int i = 0; i < disponiveis.Count; i++) System.Console.WriteLine($"  {i + 1}. {disponiveis[i].Nome}");
        var inE = LerInput("Escolha a especialidade para VINCULAR"); if (inE == "0") return;
        if (!int.TryParse(inE, out int idxE) || idxE < 1 || idxE > disponiveis.Count) { MsgErro("Opção inválida!"); return; }
        var esp = disponiveis[idxE - 1];
        await ProfEspService.CreateAsync(new ProfissionalEspecialidadeModel { FkProfissional = prof.IdUsuario, FkEspecialidade = esp.IdEspecialidade });
        Msg($"Especialidade '{esp.Nome}' vinculada ao profissional '{prof.Nome}'!");
    }

    private async Task DesvincularEspecialidade()
    {
        LimparConsole();
        Titulo("Desvincular Especialidade de Profissional");
        var profs = (await UsuarioService.GetAllAsync()).Where(u => u.IdCargo == 2).ToList();
        if (profs.Count == 0) { MsgErro("Nenhum profissional cadastrado."); return; }
        for (int i = 0; i < profs.Count; i++) System.Console.WriteLine($"  {i + 1}. {profs[i].Nome}");
        var inP = LerInput("Escolha o profissional"); if (inP == "0") return;
        if (!int.TryParse(inP, out int idxP) || idxP < 1 || idxP > profs.Count) { MsgErro("Opção inválida!"); return; }
        var prof = profs[idxP - 1];
        var todos = (await ProfEspService.GetAllAsync()).ToList();
        var esps = (await EspecialidadeService.GetAllAsync()).ToList();
        var mineIds = todos.Where(pe => pe.FkProfissional == prof.IdUsuario).Select(pe => pe.FkEspecialidade).ToHashSet();
        var minhas = esps.Where(e => mineIds.Contains(e.IdEspecialidade)).ToList();
        if (minhas.Count == 0) { MsgErro("Este profissional não possui nenhuma especialidade vinculada."); return; }
        Titulo($"Especialidades vinculadas a {prof.Nome}");
        for (int i = 0; i < minhas.Count; i++) System.Console.WriteLine($"  {i + 1}. {minhas[i].Nome}");
        var inE = LerInput("Escolha a especialidade para DESVINCULAR"); if (inE == "0") return;
        if (!int.TryParse(inE, out int idxE) || idxE < 1 || idxE > minhas.Count) { MsgErro("Opção inválida!"); return; }
        var esp = minhas[idxE - 1];
        var ok = await ProfEspService.DeleteAsync(prof.IdUsuario, esp.IdEspecialidade);
        if (ok) Msg($"Especialidade '{esp.Nome}' removida do profissional '{prof.Nome}'!"); else MsgErro("Erro ao desvincular.");
    }

    // ──── Remoções ────

    private async Task RemoverUsuario()
    {
        LimparConsole();
        Titulo("Remover Usuário");
        var usuarios = (await UsuarioService.GetAllAsync()).ToList();
        if (usuarios.Count == 0) { Msg("Nenhum usuário cadastrado."); return; }
        for (int i = 0; i < usuarios.Count; i++)
            System.Console.WriteLine($"  {i + 1}. [{(usuarios[i].IdCargo == 2 ? "Profissional" : "Cliente")}] {usuarios[i].Nome} (CPF: {usuarios[i].Cpf})");
        var inp = LerInput("Escolha o usuário para remover"); if (inp == "0") return;
        if (!int.TryParse(inp, out int idx) || idx < 1 || idx > usuarios.Count) { MsgErro("Opção inválida!"); return; }
        var u = usuarios[idx - 1];
        int qtdAtend = await UsuarioService.ContarAtendimentosAsync(u.IdUsuario);
        if (qtdAtend > 0)
        {
            if (!ConfirmarRemocaoComDados(u.Nome, $"{qtdAtend} atendimento(s) relacionado(s) serão removidos permanentemente."))
            { MsgAviso("Remoção cancelada."); Pausar(); return; }
        }
        var ok = await UsuarioService.DeleteAsync(u.IdUsuario);
        if (ok) Msg("Usuário e dados relacionados removidos com sucesso!"); else MsgErro("Erro ao remover usuário.");
    }

    private async Task RemoverServico()
    {
        LimparConsole();
        Titulo("Remover Serviço");
        var servicos = (await ServicoService.GetAllAsync()).ToList();
        if (servicos.Count == 0) { Msg("Nenhum serviço cadastrado."); return; }
        for (int i = 0; i < servicos.Count; i++)
            System.Console.WriteLine($"  {i + 1}. {servicos[i].Nome} - R${servicos[i].Preco:F2}");
        var inp = LerInput("Escolha o serviço para remover"); if (inp == "0") return;
        if (!int.TryParse(inp, out int idx) || idx < 1 || idx > servicos.Count) { MsgErro("Opção inválida!"); return; }
        var s = servicos[idx - 1];
        int qtdAtend = await ServicoService.ContarAtendimentosAsync(s.IdServico);
        if (qtdAtend > 0)
        {
            if (!ConfirmarRemocaoComDados(s.Nome, $"{qtdAtend} atendimento(s) relacionado(s) serão removidos permanentemente."))
            { MsgAviso("Remoção cancelada."); Pausar(); return; }
        }
        var ok = await ServicoService.DeleteAsync(s.IdServico);
        if (ok) Msg("Serviço e dados relacionados removidos com sucesso!"); else MsgErro("Erro ao remover serviço.");
    }

    private async Task RemoverAtendimento()
    {
        LimparConsole();
        Titulo("Remover Atendimento");
        var atendimentos = (await AtendimentoService.GetAllAsync()).ToList();
        if (atendimentos.Count == 0) { Msg("Nenhum atendimento encontrado."); return; }
        for (int i = 0; i < atendimentos.Count; i++)
        {
            var a = atendimentos[i];
            System.Console.WriteLine($"  {i + 1}. {a.Cliente?.Nome ?? "-"} | {a.Servico?.Nome ?? "-"} | {a.DataHora:dd/MM/yyyy HH:mm} | {FormatarStatus(a.StatusAtendimento)}");
        }
        var inp = LerInput("Escolha o atendimento para remover"); if (inp == "0") return;
        if (!int.TryParse(inp, out int idx) || idx < 1 || idx > atendimentos.Count) { MsgErro("Opção inválida!"); return; }
        var ok = await AtendimentoService.DeleteAsync(atendimentos[idx - 1].IdAtendimento);
        if (ok) Msg("Atendimento removido com sucesso!"); else MsgErro("Erro ao remover atendimento.");
    }

    private async Task RemoverEspecialidade()
    {
        LimparConsole();
        Titulo("Remover Especialidade");
        var esps = (await EspecialidadeService.GetAllAsync()).ToList();
        if (esps.Count == 0) { Msg("Nenhuma especialidade cadastrada."); return; }
        for (int i = 0; i < esps.Count; i++)
            System.Console.WriteLine($"  {i + 1}. {esps[i].Nome}");
        var inp = LerInput("Escolha a especialidade para remover"); if (inp == "0") return;
        if (!int.TryParse(inp, out int idx) || idx < 1 || idx > esps.Count) { MsgErro("Opção inválida!"); return; }
        var esp = esps[idx - 1];
        var (vinculos, servicos) = await EspecialidadeService.ContarDependenciasAsync(esp.IdEspecialidade);
        if (vinculos > 0 || servicos > 0)
        {
            var desc = $"{vinculos} vínculo(s) com profissional(is) serão removidos. {servicos} serviço(s) perderão a especialidade associada.";
            if (!ConfirmarRemocaoComDados(esp.Nome, desc))
            { MsgAviso("Remoção cancelada."); Pausar(); return; }
        }
        var ok = await EspecialidadeService.DeleteAsync(esp.IdEspecialidade);
        if (ok) Msg("Especialidade e vínculos removidos com sucesso!"); else MsgErro("Erro ao remover especialidade.");
    }

    // ──── Relatórios ────

    private async Task RelatorioAgendaGeral()
    {
        LimparConsole();
        Titulo("Relatório 1: Agenda Geral Detalhada");
        var atendimentos = (await AtendimentoService.GetAllAsync()).Where(a => a.StatusAtendimento != "C").OrderByDescending(a => a.DataHora).ToList();
        if (atendimentos.Count == 0) { Msg("Nenhum atendimento na agenda."); return; }
        System.Console.WriteLine($"  {"Cliente",-20} {"Barbeiro",-20} {"Serviço",-20} {"Preço",-10} {"Data",-18}");
        Separador();
        foreach (var a in atendimentos)
            System.Console.WriteLine($"  {(a.Cliente?.Nome ?? "N/D"),-20} {(a.Profissional?.Nome ?? "N/D"),-20} {(a.Servico?.Nome ?? "N/D"),-20} {(a.Servico != null ? $"R${a.Servico.Preco:F2}" : "-"),-10} {a.DataHora:dd/MM/yy HH:mm}");
        Pausar();
    }

    private async Task RelatorioFaturamento()
    {
        LimparConsole();
        Titulo("Relatório 2: Faturamento Total por Barbeiro");
        var realizados = (await AtendimentoService.GetAllAsync()).Where(a => a.StatusAtendimento == "R" && a.Servico != null).ToList();
        if (realizados.Count == 0) { Msg("Nenhum faturamento registrado (nenhum atendimento 'Realizado')."); return; }
        var fat = realizados.GroupBy(a => a.Profissional?.Nome ?? "Desconhecido")
            .Select(g => new { Barbeiro = g.Key, Total = g.Sum(a => a.Servico!.Preco), Qtd = g.Count() })
            .OrderByDescending(x => x.Total).ToList();
        System.Console.WriteLine($"  {"Barbeiro",-30} {"Total Faturado",-20} {"Qtd Serviços"}");
        Separador();
        foreach (var f in fat)
            System.Console.WriteLine($"  {f.Barbeiro,-30} R${f.Total,-18:F2} {f.Qtd}");
        Pausar();
    }

    private async Task RelatorioServicosPorBairro()
    {
        LimparConsole();
        Titulo("Relatório 3: Serviços mais procurados por Bairro");
        var comBairro = (await AtendimentoService.GetAllAsync())
            .Where(a => a.Cliente != null && !string.IsNullOrWhiteSpace(a.Cliente.EndBairro) && a.Servico != null).ToList();

        if (comBairro.Count == 0) { Msg("Nenhum dado com cruzamento de Bairro encontrado."); return; }

        // Group by Bairro to get totals per Bairro and details per service
        var bairros = comBairro
            .GroupBy(a => a.Cliente!.EndBairro!.Trim())
            .OrderBy(gb => gb.Key)
            .ToList();

        System.Console.WriteLine($"  {"Bairro",-20} | {"Serviço",-25} | {"Qtd",-5} | {"Total Faturado",-15}");
        Separador();

        foreach (var bairroGroup in bairros)
        {
            var totalBairroQtd = bairroGroup.Count();
            var totalBairroFat = bairroGroup.Sum(a => a.Servico!.Preco);

            System.Console.WriteLine($"  {bairroGroup.Key,-20} | {"",-25} | {totalBairroQtd,-5} | R${totalBairroFat,-13:F2}");

            var servicosNoBairro = bairroGroup
                .GroupBy(a => a.Servico!.Nome)
                .Select(gs => new
                {
                    Servico = gs.Key,
                    Qtd = gs.Count(),
                    TotalFat = gs.Sum(a => a.Servico!.Preco)
                })
                .OrderByDescending(s => s.Qtd)
                .ToList();

            foreach (var s in servicosNoBairro)
                System.Console.WriteLine($"  {"",-20} | {s.Servico,-25} | {s.Qtd,-5} | R${s.TotalFat,-13:F2}");
        }

        Pausar();
    }
}
