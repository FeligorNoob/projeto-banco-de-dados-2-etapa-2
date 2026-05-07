using BarberApplication.Console.Models;
using BarberApplication.Console.Services;
using System;

namespace BarberApplication.Console.Menus;

public class CentralCrudConsole : ConsoleMenuBase
{
	public CentralCrudConsole(
		IUsuarioService usuarioService,
		ICargoService cargoService,
		IEspecialidadeService especialidadeService,
		IProfissionalEspecialidadeService profEspService,
		IServicoService servicoService,
		IAtendimentoService atendimentoService)
		: base(usuarioService, cargoService, especialidadeService, profEspService, servicoService, atendimentoService)
	{
	}

	public override async Task ExibirAsync()
	{
		while (true)
		{
			LimparConsole();
			Titulo("Central de Dados (CRUD Base)");
			System.Console.WriteLine("  Módulos Disponíveis:");
			System.Console.WriteLine("  1.  CRUD Usuários");
			System.Console.WriteLine("  2.  CRUD Especialidades");
			System.Console.WriteLine("  3.  CRUD Serviços");
			System.Console.WriteLine("  4.  CRUD Atendimentos");
			System.Console.WriteLine("  5.  CRUD Cargos");
			System.Console.WriteLine("  6.  CRUD Profissional x Especialidade");
			System.Console.WriteLine("  7.  Visualizar Relatórios Avançados (Admin)");
			System.Console.WriteLine();
			System.Console.WriteLine("  0.  Voltar");
			Separador();

			var opcao = LerOpcao();
			await ExecutarProtegidoAsync(async () =>
			{
				switch (opcao)
				{
					case "1": await SubMenuUsuarios(); break;
					case "2": await SubMenuEspecialidades(); break;
					case "3": await SubMenuServicos(); break;
					case "4": await SubMenuAtendimentos(); break;
					case "5": await SubMenuCargos(); break;
					case "6": await SubMenuProfEsp(); break;
					case "7": await VisualizarRelatorios(); break;
					case "0": return;
					default: MsgErro("Opção inválida!"); break;
				}
			});
			if (opcao == "0") return;
		}
	}

	private async Task SubMenuUsuarios()
	{
		while (true)
		{
			LimparConsole();
			Titulo("Gestão de Usuários");
			System.Console.WriteLine("  1. Criar");
			System.Console.WriteLine("  2. Consultar");
			System.Console.WriteLine("  3. Editar");
			System.Console.WriteLine("  4. Remover");
			System.Console.WriteLine("  0. Voltar");
			Separador();

			var opcao = LerOpcao();
			switch (opcao)
			{
				case "1": await CriarUsuario(); break;
				case "2": await VerUsuarios(); break;
				case "3": await EditarUsuario(); break;
				case "4": await RemoverUsuario(); break;
				case "0": return;
				default: MsgErro("Opção inválida!"); break;
			}
		}
	}

	private async Task CriarUsuario()
	{
		LimparConsole();
		Titulo("Criar novo Usuário");

		var cpf = LerInput("CPF (11 dígitos)");
		if (cpf == "0") return;

		var nome = LerInput("Nome completo");
		if (nome == "0") return;

		var tel = LerInput("Telefone (opcional, Enter para pular)");
		if (tel == "0") return;

		var cargos = await CargoService.GetAllAsync();
		System.Console.WriteLine();
		System.Console.WriteLine("  Cargos disponíveis:");
		foreach (var c in cargos)
			System.Console.WriteLine($"  [{c.IdCargo}] {c.Nome}");

		var idCargoStr = LerInput("ID do Cargo");
		if (idCargoStr == "0") return;

		if (!int.TryParse(idCargoStr, out int idCargo)) return;

		try
		{
			await UsuarioService.CreateAsync(new UsuarioModel
			{
				Cpf = cpf,
				Nome = nome,
				Telefone = string.IsNullOrWhiteSpace(tel) ? null : tel,
				IdCargo = idCargo
			});
			Msg($"Usuário criado com sucesso!");
		}
		catch (ArgumentException ex)
		{
			MsgErro(ex.Message);
		}
	}

	private async Task VerUsuarios()
	{
		LimparConsole();
		Titulo("Todos os Usuários");

		var usuarios = (await UsuarioService.GetAllAsync()).ToList();

		if (usuarios.Count == 0) { Msg("Nenhum usuário cadastrado."); return; }

		System.Console.WriteLine($"  {"ID",-5} | {"Nome",-30} | {"CPF",-15} | {"Cargo",-20}");
		Separador();
		foreach (var c in usuarios)
			System.Console.WriteLine($"  {c.IdUsuario,-5} | {c.Nome,-30} | {c.Cpf,-15} | {(c.Cargo?.Nome ?? c.IdCargo.ToString()),-20}");
		Pausar();
	}

	private async Task EditarUsuario()
	{
		LimparConsole();
		Titulo("Editar Usuário");
		var usuarios = (await UsuarioService.GetAllAsync()).OrderBy(u => u.IdUsuario).ToList();
		if (usuarios.Count == 0) { Msg("Nenhum usuário cadastrado."); return; }

		foreach (var c in usuarios)
			System.Console.WriteLine($"  [{c.IdUsuario}] {c.Nome} (CPF: {c.Cpf})");

		var inId = LerInput("Digite o ID do usuário para editar");
		if (inId == "0") return;

		if (!int.TryParse(inId, out int idUsuario)) return;

		var usuario = await UsuarioService.GetByIdAsync(idUsuario);
		if (usuario == null) { MsgErro("Não encontrado!"); return; }

		var nome = LerInputEdicao($"Novo Nome [{usuario.Nome}]");
		if (nome != "0" && !string.IsNullOrWhiteSpace(nome)) usuario.Nome = nome;

		var tel = LerInputEdicao($"Novo Telefone [{usuario.Telefone}]");
		if (tel != "0" && !string.IsNullOrWhiteSpace(tel)) usuario.Telefone = tel;

		await UsuarioService.UpdateAsync(idUsuario, usuario);
		Msg("Usuário editado com sucesso!");
	}

	private async Task RemoverUsuario()
	{
		LimparConsole();
		Titulo("Remover Usuário");
		var usuarios = (await UsuarioService.GetAllAsync()).OrderBy(u => u.IdUsuario).ToList();
		if (usuarios.Count == 0) { Msg("Nenhum usuário cadastrado."); return; }

		foreach (var c in usuarios)
			System.Console.WriteLine($"  [{c.IdUsuario}] {c.Nome} (CPF: {c.Cpf})");

		var inId = LerInput("Digite o ID do usuário para remover");
		if (inId == "0") return;

		if (!int.TryParse(inId, out int idUsuario)) return;

		try 
		{
			var ok = await UsuarioService.DeleteAsync(idUsuario);
			if (ok) Msg("Usuário removido com sucesso!"); else MsgErro("Erro ao remover usuário (pode haver dependências).");
		}
		catch (Exception)
		{
			MsgErro("Usuário possui dependências como Agendamentos ou Vínculos no banco de dados e não pode ser removido!");
		}
	}

	private async Task SubMenuEspecialidades()
	{
		 while (true)
		{
			LimparConsole();
			Titulo("Gestão de Especialidades");
			System.Console.WriteLine("  1. Criar");
			System.Console.WriteLine("  2. Consultar");
			System.Console.WriteLine("  3. Editar");
			System.Console.WriteLine("  4. Remover");
			System.Console.WriteLine("  0. Voltar");
			Separador();

			var opcao = LerOpcao();
			switch (opcao)
			{
				case "1": await CriarEspecialidade(); break;
				case "2": await VerEspecialidades(); break;
				case "3": await EditarEspecialidade(); break;
				case "4": await RemoverEspecialidade(); break;
				case "0": return;
				default: MsgErro("Opção inválida!"); break;
			}
		}
	}

	private async Task CriarEspecialidade()
	{
		LimparConsole();
		Titulo("Criar Especialidade");
		var nome = LerInput("Nome da Especialidade");
		if (string.IsNullOrWhiteSpace(nome) || nome == "0") return;
		await EspecialidadeService.CreateAsync(new EspecialidadeModel { Nome = nome });
		Msg("Criado!");
	}

	private async Task VerEspecialidades()
	{
		LimparConsole();
		Titulo("Especialidades");
		var esps = (await EspecialidadeService.GetAllAsync()).ToList();
		if (esps.Count == 0) { Msg("Nenhuma especialidade cadastrada."); return; }
		
		System.Console.WriteLine($"  {"ID",-5} | {"Nome",-40}");
		Separador();
		foreach(var e in esps) System.Console.WriteLine($"  {e.IdEspecialidade,-5} | {e.Nome,-40}");
		Pausar();
	}

	private async Task EditarEspecialidade()
	{
		LimparConsole();
		Titulo("Editar Especialidade");
		var esps = (await EspecialidadeService.GetAllAsync()).OrderBy(e => e.IdEspecialidade).ToList();
		foreach(var e in esps) System.Console.WriteLine($"  [{e.IdEspecialidade}] {e.Nome}");

		var inId = LerInput("ID da especialidade");
		if (inId == "0" || !int.TryParse(inId, out int id)) return;
		var esp = await EspecialidadeService.GetByIdAsync(id);
		if (esp == null) return;
		var nome = LerInputEdicao($"Novo Nome [{esp.Nome}]");
		if (nome != "0" && !string.IsNullOrWhiteSpace(nome)) {
			esp.Nome = nome;
			await EspecialidadeService.UpdateAsync(id, esp);
			Msg("Atualizado!");
		}
	}

	private async Task RemoverEspecialidade()
	{
		LimparConsole();
		Titulo("Remover Especialidade");
		var esps = (await EspecialidadeService.GetAllAsync()).OrderBy(e => e.IdEspecialidade).ToList();
		foreach(var e in esps) System.Console.WriteLine($"  [{e.IdEspecialidade}] {e.Nome}");

		var inId = LerInput("ID da especialidade para remover");
		if (inId == "0" || !int.TryParse(inId, out int id)) return;
		try {
			await EspecialidadeService.DeleteAsync(id);
			Msg("Removido!");
		} catch { MsgErro("Impossível remover pois está amarrado a profissionais/serviços!"); }
	}

	private async Task SubMenuServicos()
	{
		 while (true)
		{
			LimparConsole();
			Titulo("Gestão de Serviços");
			System.Console.WriteLine("  1. Criar");
			System.Console.WriteLine("  2. Consultar");
			System.Console.WriteLine("  3. Editar");
			System.Console.WriteLine("  4. Remover");
			System.Console.WriteLine("  0. Voltar");
			Separador();

			var opcao = LerOpcao();
			switch (opcao)
			{
				case "1": await CriarServico(); break;
				case "2": await VisualizarServicos(); break;
				case "3": await EditarServico(); break;
				case "4": await RemoverServico(); break;
				case "0": return;
				default: MsgErro("Opção inválida!"); break;
			}
		}
	}

	private async Task CriarServico()
	{
		LimparConsole();
		Titulo("Criar Serviço");
		var nome = LerInput("Nome do serviço");
		if (nome == "0") return;

		var especialidades = await EspecialidadeService.GetAllAsync();
		System.Console.WriteLine();
		System.Console.WriteLine("  Especialidades disponíveis:");
		foreach(var e in especialidades) System.Console.WriteLine($"  [{e.IdEspecialidade}] {e.Nome}");
		var inEsp = LerInput("ID da Especialidade (opcional, Enter para pular)");
		int? fkEspecialidade = null;
		if (inEsp == "0") return;
		if (!string.IsNullOrWhiteSpace(inEsp) && int.TryParse(inEsp, out int espId)) fkEspecialidade = espId;

		var inPreco = LerInput("Preço (ex: 50.00)");
		if (inPreco == "0") return;
		if (!decimal.TryParse(inPreco.Replace('.', ','), out decimal preco)) { MsgErro("Preço inválido!"); return; }

		var inDuracao = LerInput("Duração (min)");
		if (inDuracao == "0" || !int.TryParse(inDuracao, out int duracao)) return;

		await ServicoService.CreateAsync(new ServicoModel { Nome = nome, FkEspecialidade = fkEspecialidade, Preco = preco, DuracaoEstimada = duracao });
		Msg("Serviço Criado!");
	}

	private async Task EditarServico()
	{
		LimparConsole();
		Titulo("Editar Serviço");
		var servicos = (await ServicoService.GetAllAsync()).OrderBy(s => s.IdServico).ToList();
		foreach(var s in servicos) System.Console.WriteLine($"  [{s.IdServico}] {s.Nome} - R${s.Preco:F2}");

		var inId = LerInput("ID do Serviço");
		if (inId == "0" || !int.TryParse(inId, out int id)) return;
		var serv = await ServicoService.GetByIdAsync(id);
		if (serv == null) return;

		var n = LerInputEdicao($"Novo nome [{serv.Nome}]");
		if (n != "0" && !string.IsNullOrWhiteSpace(n)) serv.Nome = n;

		var especialidades = await EspecialidadeService.GetAllAsync();
		System.Console.WriteLine();
		System.Console.WriteLine("  Especialidades disponíveis:");
		foreach(var e in especialidades) System.Console.WriteLine($"  [{e.IdEspecialidade}] {e.Nome}");
		
		var currentEsp = serv.FkEspecialidade.HasValue ? serv.FkEspecialidade.ToString() : "Nenhuma";
		var inEsp = LerInputEdicao($"Novo ID Especialidade [{currentEsp}] (Enter para manter)");
		if (inEsp != "0" && !string.IsNullOrWhiteSpace(inEsp)) {
			if (int.TryParse(inEsp, out int espId)) serv.FkEspecialidade = espId;
		}

		await ServicoService.UpdateAsync(id, serv);
		Msg("Serviço Atualizado!");
	}

	private async Task RemoverServico()
	{
		LimparConsole();
		Titulo("Remover Serviço");
		var servicos = (await ServicoService.GetAllAsync()).OrderBy(s => s.IdServico).ToList();
		foreach(var s in servicos) System.Console.WriteLine($"  [{s.IdServico}] {s.Nome} - R${s.Preco:F2}");

		var inId = LerInput("ID do Serviço");
		if (inId == "0" || !int.TryParse(inId, out int id)) return;

		try {
			await ServicoService.DeleteAsync(id);
			Msg("Removido com sucesso!");
		} catch { MsgErro("Falha: Serviço pode estar ligado a um atendimento."); }
	}

	private async Task SubMenuAtendimentos()
	{
		 while (true)
		{
			LimparConsole();
			Titulo("Gestão de Atendimentos");
			System.Console.WriteLine("  1. Criar");
			System.Console.WriteLine("  2. Consultar");
			System.Console.WriteLine("  3. Editar");
			System.Console.WriteLine("  4. Remover");
			System.Console.WriteLine("  0. Voltar");
			Separador();

			var opcao = LerOpcao();
			switch (opcao)
			{
				case "1": await CriarAtendimento(); break;
				case "2": await VerAtendimentos(); break;
				case "3": await EditarAtendimento(); break;
				case "4": await RemoverAtendimento(); break;
				case "0": return;
				default: MsgErro("Opção inválida!"); break;
			}
		}
	}

	private async Task CriarAtendimento()
	{
		LimparConsole();
		Titulo("Criar Atendimento");

		var profissionais = (await UsuarioService.GetAllAsync()).Where(u => u.IdCargo == 2).ToList();
		System.Console.WriteLine();
		System.Console.WriteLine("  Profissionais disponíveis:");
		foreach(var p in profissionais) System.Console.WriteLine($"  [{p.IdUsuario}] {p.Nome}");
		var inProf = LerInput("ID do Profissional"); if(inProf == "0" || !int.TryParse(inProf, out int fkProf)) return;

		var clientes = (await UsuarioService.GetAllAsync()).Where(u => u.IdCargo == 3).ToList();
		System.Console.WriteLine();
		System.Console.WriteLine("  Clientes disponíveis:");
		foreach(var c in clientes) System.Console.WriteLine($"  [{c.IdUsuario}] {c.Nome}");
		var inCli = LerInput("ID do Cliente"); if(inCli == "0" || !int.TryParse(inCli, out int fkCli)) return;

		var servicos = await ServicoService.GetAllAsync();
		System.Console.WriteLine();
		System.Console.WriteLine("  Serviços disponíveis:");
		foreach(var s in servicos) System.Console.WriteLine($"  [{s.IdServico}] {s.Nome} - R${s.Preco:F2}");
		var inServ = LerInput("ID do Serviço"); if(inServ == "0" || !int.TryParse(inServ, out int fkServ)) return;

		var inData = LerInput("Data e Hora (dd/MM/yyyy HH:mm)"); 
		if(inData == "0" || !DateTime.TryParseExact(inData, "dd/MM/yyyy HH:mm", null, System.Globalization.DateTimeStyles.None, out DateTime dataHora)) return;
		var status = LerInput("Status (A, R, C)"); if(status == "0" || string.IsNullOrWhiteSpace(status)) return;

		try {
			await AtendimentoService.CreateAsync(new AtendimentoModel {
				FkProfissional = fkProf,
				FkCliente = fkCli,
				FkServico = fkServ,
				DataHora = dataHora,
				StatusAtendimento = status.ToUpper()
			});
			Msg("Atendimento criado!");
		} catch { MsgErro("Erro ao criar atendimento. Verifique os dados inseridos."); }
	}

	private async Task VerAtendimentos()
	{
		LimparConsole();
		Titulo("Todos os Atendimentos");
		var atendimentos = (await AtendimentoService.GetAllAsync()).OrderBy(a => a.IdAtendimento).ToList();
		if (atendimentos.Count == 0) { Msg("Nenhum atendimento cadastrado."); return; }

		System.Console.WriteLine($"  {"ID",-5} | {"Data e Hora",-16} | {"Profissional",-20} | {"Cliente",-20} | {"Serviço",-20} | {"Status",-6}");
		Separador();
		foreach (var a in atendimentos)
			System.Console.WriteLine($"  {a.IdAtendimento,-5} | {a.DataHora,-16:dd/MM/yy HH:mm} | {(a.Profissional?.Nome ?? "N/D"),-20} | {(a.Cliente?.Nome ?? "N/D"),-20} | {(a.Servico?.Nome ?? "N/D"),-20} | {a.StatusAtendimento,-6}");
		Pausar();
	}

	private async Task EditarAtendimento()
	{
		LimparConsole();
		Titulo("Editar Atendimento");
		var atendimentos = (await AtendimentoService.GetAllAsync()).OrderBy(a => a.IdAtendimento).ToList();
		foreach (var a in atendimentos)
			System.Console.WriteLine($"  [{a.IdAtendimento}] {a.DataHora:dd/MM/yy HH:mm} | Prof: {a.Profissional?.Nome ?? "N/D"} | Cli: {a.Cliente?.Nome ?? "N/D"} | Serv: {a.Servico?.Nome ?? "N/D"} | Sts: {a.StatusAtendimento}");

		var inId = LerInput("ID do Atendimento");
		if (inId == "0" || !int.TryParse(inId, out int id)) return;
		var atend = await AtendimentoService.GetByIdAsync(id);
		if (atend == null) return;

		var status = LerInputEdicao($"Novo Status [{atend.StatusAtendimento}] (A/R/C)");
		if (status != "0" && !string.IsNullOrWhiteSpace(status)) atend.StatusAtendimento = status.ToUpper();

		try {
			await AtendimentoService.AtualizarStatusAsync(id, atend.StatusAtendimento);
			Msg("Atualizado!");
		} catch { MsgErro("Erro ao atualizar!"); }
	}

	private async Task RemoverAtendimento()
	{
		LimparConsole();
		Titulo("Remover Atendimento");
		var atendimentos = (await AtendimentoService.GetAllAsync()).OrderBy(a => a.IdAtendimento).ToList();
		foreach (var a in atendimentos)
			System.Console.WriteLine($"  [{a.IdAtendimento}] {a.DataHora:dd/MM/yy HH:mm} | Prof: {a.Profissional?.Nome ?? "N/D"} | Cli: {a.Cliente?.Nome ?? "N/D"} | Serv: {a.Servico?.Nome ?? "N/D"} | Sts: {a.StatusAtendimento}");

		var inId = LerInput("ID do Atendimento");
		if (inId == "0" || !int.TryParse(inId, out int id)) return;

		try {
			await AtendimentoService.DeleteAsync(id);
			Msg("Removido com sucesso!");
		} catch { MsgErro("Falha ao remover."); }
	}

	private async Task SubMenuCargos()
	{
		 while (true)
		{
			LimparConsole();
			Titulo("Gestão de Cargos");
			System.Console.WriteLine("  1. Criar");
			System.Console.WriteLine("  2. Consultar");
			System.Console.WriteLine("  3. Editar");
			System.Console.WriteLine("  4. Remover");
			System.Console.WriteLine("  0. Voltar");
			Separador();

			var opcao = LerOpcao();
			switch (opcao)
			{
				case "1": await CriarCargo(); break;
				case "2": await VerCargos(); break;
				case "3": await EditarCargo(); break;
				case "4": await RemoverCargo(); break;
				case "0": return;
				default: MsgErro("Opção inválida!"); break;
			}
		}
	}

	private async Task CriarCargo()
	{
		LimparConsole();
		Titulo("Criar Cargo");
		var nome = LerInput("Nome do Cargo");
		if (string.IsNullOrWhiteSpace(nome) || nome == "0") return;

		try {
			await CargoService.CreateAsync(new CargoModel { Nome = nome });
			Msg("Criado!");
		} catch { MsgErro("Erro ao criar Cargo."); }
	}

	private async Task VerCargos()
	{
		LimparConsole();
		Titulo("Cargos");
		var cargos = (await CargoService.GetAllAsync()).ToList();
		if (cargos.Count == 0) { Msg("Nenhum cargo cadastrado."); return; }
		
		System.Console.WriteLine($"  {"ID",-5} | {"Nome",-40}");
		Separador();
		foreach(var c in cargos) System.Console.WriteLine($"  {c.IdCargo,-5} | {c.Nome,-40}");
		Pausar();
	}

	private async Task EditarCargo()
	{
		LimparConsole();
		Titulo("Editar Cargo");
		var cargos = await CargoService.GetAllAsync();
		foreach(var c in cargos) System.Console.WriteLine($"  [{c.IdCargo}] {c.Nome}");

		var inId = LerInput("ID do Cargo");
		if (inId == "0" || !int.TryParse(inId, out int id)) return;
		var cargo = await CargoService.GetByIdAsync(id);
		if (cargo == null) return;

		var nome = LerInputEdicao($"Novo Nome [{cargo.Nome}]");
		if (nome != "0" && !string.IsNullOrWhiteSpace(nome)) {
			cargo.Nome = nome;
			await CargoService.UpdateAsync(id, cargo);
			Msg("Atualizado!");
		}
	}

	private async Task RemoverCargo()
	{
		LimparConsole();
		Titulo("Remover Cargo");
		var cargos = await CargoService.GetAllAsync();
		foreach(var c in cargos) System.Console.WriteLine($"  [{c.IdCargo}] {c.Nome}");

		var inId = LerInput("ID do Cargo para remover");
		if (inId == "0" || !int.TryParse(inId, out int id)) return;
		try {
			await CargoService.DeleteAsync(id);
			Msg("Removido!");
		} catch { MsgErro("Impossível remover pois existem usuários com este cargo!"); }
	}

	private async Task SubMenuProfEsp()
	{
		 while (true)
		{
			LimparConsole();
			Titulo("Gestão de Profissional X Especialidade");
			System.Console.WriteLine("  1. Vincular");
			System.Console.WriteLine("  2. Consultar Todos");
			System.Console.WriteLine("  3. Remover Vínculo");
			System.Console.WriteLine("  0. Voltar");
			Separador();

			var opcao = LerOpcao();
			switch (opcao)
			{
				case "1": await VincularProfEsp(); break;
				case "2": await VerProfEsp(); break;
				case "3": await RemoverProfEsp(); break;
				case "0": return;
				default: MsgErro("Opção inválida!"); break;
			}
		}
	}

	private async Task VincularProfEsp()
	{
		LimparConsole();
		Titulo("Vincular Especialidade a Profissional");

		var profissionais = (await UsuarioService.GetAllAsync()).Where(u => u.IdCargo == 2).ToList();
		System.Console.WriteLine();
		System.Console.WriteLine("  Profissionais disponíveis:");
		foreach(var p in profissionais) System.Console.WriteLine($"  [{p.IdUsuario}] {p.Nome}");
		var inProf = LerInput("ID do Profissional"); if(inProf == "0" || !int.TryParse(inProf, out int fkProf)) return;

		var especialidades = await EspecialidadeService.GetAllAsync();
		System.Console.WriteLine();
		System.Console.WriteLine("  Especialidades disponíveis:");
		foreach(var e in especialidades) System.Console.WriteLine($"  [{e.IdEspecialidade}] {e.Nome}");
		var inEsp = LerInput("ID da Especialidade"); if(inEsp == "0" || !int.TryParse(inEsp, out int fkEsp)) return;

		var existing = await ProfEspService.GetByIdAsync(fkProf, fkEsp);
		if (existing != null)
		{
			MsgErro("Este profissional já possui esta especialidade vinculada!");
			return;
		}

		try {
			await ProfEspService.CreateAsync(new ProfissionalEspecialidadeModel { FkProfissional = fkProf, FkEspecialidade = fkEsp });
			Msg("Vinculado!");
		} catch (Exception ex) { 
			MsgErro($"Erro ao vincular: {ex.Message} {(ex.InnerException != null ? " -> " + ex.InnerException.Message : "")}"); 
		}
	}

	private async Task VerProfEsp()
	{
		LimparConsole();
		Titulo("Vínculos Profissional x Especialidade");
		var vinculos = (await ProfEspService.GetAllAsync()).ToList();
		if (vinculos.Count == 0) { Msg("Nenhum vínculo cadastrado."); return; }

		var profissionais = (await UsuarioService.GetAllAsync()).Where(u => u.IdCargo == 2).ToList();
		var especialidades = (await EspecialidadeService.GetAllAsync()).ToList();

		System.Console.WriteLine($"  {"Profissional",-30} | {"Especialidade",-30}");
		Separador();
		foreach (var v in vinculos)
		{
			var profNome = profissionais.FirstOrDefault(p => p.IdUsuario == v.FkProfissional)?.Nome ?? $"ID: {v.FkProfissional}";
			var espNome = especialidades.FirstOrDefault(e => e.IdEspecialidade == v.FkEspecialidade)?.Nome ?? $"ID: {v.FkEspecialidade}";
			System.Console.WriteLine($"  {profNome,-30} | {espNome,-30}");
		}
		Pausar();
	}

	private async Task RemoverProfEsp()
	{
		LimparConsole();
		Titulo("Remover Vínculo");
		var vinculos = (await ProfEspService.GetAllAsync()).ToList();
		if (vinculos.Count == 0) { Msg("Nenhum vínculo cadastrado."); return; }

		var profissionais = (await UsuarioService.GetAllAsync()).Where(u => u.IdCargo == 2).ToList();
		var especialidades = (await EspecialidadeService.GetAllAsync()).ToList();

		System.Console.WriteLine($"  {"ID P.",-6} | {"Profissional",-25} | {"ID E.",-6} | {"Especialidade",-25}");
		Separador();
		foreach (var v in vinculos)
		{
			var profNome = profissionais.FirstOrDefault(p => p.IdUsuario == v.FkProfissional)?.Nome ?? "N/D";
			var espNome = especialidades.FirstOrDefault(e => e.IdEspecialidade == v.FkEspecialidade)?.Nome ?? "N/D";
			System.Console.WriteLine($"  {v.FkProfissional,-6} | {profNome,-25} | {v.FkEspecialidade,-6} | {espNome,-25}");
		}

		System.Console.WriteLine();
		var inProf = LerInput("ID do Profissional"); if(inProf == "0" || !int.TryParse(inProf, out int fkProf)) return;
		var inEsp = LerInput("ID da Especialidade"); if(inEsp == "0" || !int.TryParse(inEsp, out int fkEsp)) return;

		try {
			var ok = await ProfEspService.DeleteAsync(fkProf, fkEsp);
			if (ok) Msg("Removido!");
			else MsgErro("Não foi possível encontrar o vínculo para remover. Verifique os IDs informados.");
		} catch (Exception ex) { 
			MsgErro($"Falha ao remover: {ex.Message} {(ex.InnerException != null ? " -> " + ex.InnerException.Message : "")}"); 
		}
	}

	private async Task VisualizarRelatorios()
	{
		while(true)
		{
			LimparConsole();
			Titulo("Relatórios Base Avançados (Atendimentos Cruzados)");
			System.Console.WriteLine("  ──── Relatórios ────");
			System.Console.WriteLine("  17. Agenda Geral Detalhada");
			System.Console.WriteLine("  18. Faturamento Total por Barbeiro");
			System.Console.WriteLine("  19. Serviços mais procurados por Bairro");
			System.Console.WriteLine();
			System.Console.WriteLine("  0.  Voltar");
			Separador();

			var opcao = LerOpcao();
			switch (opcao)
			{
				case "17": await RelatorioAgendaGeral(); break;
				case "18": await RelatorioFaturamento(); break;
				case "19": await RelatorioServicosPorBairro(); break;
				case "0": return;
				default: MsgErro("Opção inválida!"); break;
			}
		}
	}

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
