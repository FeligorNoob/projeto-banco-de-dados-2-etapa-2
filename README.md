# 💈 BarberFlow — Sistema de Agendamento para Barbearia

Aplicação console em **C# (.NET 10)** com **Entity Framework Core** e banco de dados **MySQL**, desenvolvida como projeto acadêmico de Banco de Dados 2.

---

## 📋 Pré-requisitos

- [MySQL 8.0+](#1-instalar-o-mysql)
- [.NET 10 SDK](#2-instalar-o-net-sdk)
- Git (para clonar o repositório)

---

## 🚀 Passo a Passo para Executar

### 1. Instalar o MySQL

1. Acesse o site oficial: [https://dev.mysql.com/downloads/installer/](https://dev.mysql.com/downloads/installer/)
2. Baixe o **MySQL Installer for Windows** (versão completa ou web)
3. Execute o instalador e selecione o tipo **"Developer Default"** ou **"Server Only"**
4. Durante a configuração:
   - Mantenha a porta padrão ou defina a porta desejada (o projeto usa **3636**)
   - Defina a senha do usuário `root` (o projeto usa **root** como senha padrão)
5. Finalize a instalação e certifique-se de que o serviço MySQL está rodando

> [!TIP]
> Caso use Docker, você pode subir um container MySQL rapidamente:
> ```bash
> docker run -d --name barberflow-mysql -p 3636:3306 -e MYSQL_ROOT_PASSWORD=root mysql:8.0
> ```

---

### 2. Instalar o .NET SDK

1. Acesse: [https://dotnet.microsoft.com/download](https://dotnet.microsoft.com/download)
2. Baixe e instale o **.NET 10 SDK**
3. Verifique a instalação abrindo um terminal:

```bash
dotnet --version
```

O resultado deve mostrar uma versão `10.x.x` ou superior.

---

### 3. Clonar o Repositório

```bash
git clone https://github.com/FeligorNoob/ProjetoBancoDados2.git
cd ProjetoBancoDados2
```

---

### 4. Criar o Banco de Dados

Abra o **MySQL Workbench**, **DBeaver**, ou qualquer cliente SQL e execute o script de criação do banco:

**Opção A — Pelo cliente SQL (Workbench / DBeaver):**

1. Abra o arquivo `BarberApplication.Console/BarberFlow_Script.sql`
2. Execute o script completo (ele cria o banco, tabelas e dados de exemplo)

**Opção B — Pela linha de comando:**

```bash
mysql -u root -p --port=3636 < BarberApplication.Console/BarberFlow_Script.sql
```

Digite a senha `root` quando solicitado.

> [!IMPORTANT]
> O script já inclui o comando `CREATE DATABASE IF NOT EXISTS BarberFlow;`, então não é necessário criar o banco manualmente.

---

### 5. Configurar a Conexão (se necessário)

O arquivo de configuração fica em `BarberApplication.Console/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "server=localhost;port=3636;database=BarberFlow;user=root;password=root"
  }
}
```

Ajuste os valores caso o seu MySQL use uma porta, usuário ou senha diferente:

| Parâmetro  | Valor Padrão | Descrição                  |
|------------|--------------|----------------------------|
| `server`   | `localhost`  | Endereço do servidor MySQL |
| `port`     | `3636`       | Porta do MySQL             |
| `database` | `BarberFlow` | Nome do banco de dados     |
| `user`     | `root`       | Usuário do MySQL           |
| `password` | `root`       | Senha do MySQL             |

---

### 6. Restaurar Dependências e Compilar

```bash
cd BarberApplication.Console
dotnet restore
dotnet build
```

A saída esperada deve mostrar:

```
Compilação com êxito.
    0 Aviso(s)
    0 Erro(s)
```

ou

```
Construir êxito em X,Xs
```

---

### 7. Executar a Aplicação

```bash
dotnet run
```

Você verá o menu principal:

```
  ╔══════════════════════════════════════════════╗
  ║  Sistema de Agendamento para Barbearia       ║
  ╚══════════════════════════════════════════════╝

  1. Visualizar como Cliente
  2. Visualizar como Profissional
  3. Visualizar como Administrador
  0. Sair
```

---

## 🏗️ Estrutura do Projeto

```
BarberApplication.Console/
├── Program.cs                  → Ponto de entrada
├── BarberApp.cs                → Orquestrador principal (menu raiz)
├── appsettings.json            → Configuração de conexão
├── BarberFlow_Script.sql       → Script de criação do banco + dados
│
├── Data/
│   ├── BarbeariaContext.cs     → DbContext (Entity Framework)
│   ├── IDatabaseFactory.cs     → Interface do Factory Method
│   └── MySqlDatabaseFactory.cs → Implementação MySQL
│
├── Models/                     → Entidades mapeadas para o banco
│   ├── UsuarioModel.cs
│   ├── CargoModel.cs
│   ├── AtendimentoModel.cs
│   ├── ServicoModel.cs
│   ├── EspecialidadeModel.cs
│   └── ProfissionalEspecialidadeModel.cs
│
├── Services/                   → Camada de serviço (CRUD)
│   ├── UsuarioService.cs
│   ├── CargoService.cs
│   ├── AtendimentoService.cs
│   ├── ServicoService.cs
│   ├── EspecialidadeService.cs
│   └── ProfissionalEspecialidadeService.cs
│
└── Menus/                      → Interface console por perfil
    ├── IConsoleMenu.cs
    ├── ConsoleMenuBase.cs
    ├── ClienteConsole.cs
    ├── ProfissionalConsole.cs
    └── AdministradorConsole.cs
```

---

## 👥 Perfis de Acesso

| Cargo          | ID  | Funcionalidades                                                 |
|----------------|-----|-----------------------------------------------------------------|
| **Cliente**        | 3   | Agendar serviço, ver/cancelar agendamentos, ver serviços        |
| **Profissional**   | 2   | Ver agenda, marcar atendimentos como realizados, ver especialidades |
| **Administrador**  | 1   | CRUD completo de usuários, serviços, especialidades + relatórios  |

---

## 📊 Dados de Teste

O script SQL já inclui dados de exemplo prontos para uso:

- **1** Administrador (CPF: `99999999999`)
- **10** Profissionais (CPFs: `11111111102` a `11111111111`)
- **39** Clientes (CPFs: `22222222212` a `22222222220` + mocks)
- **3** Especialidades: Cortes de Cabelo, Barba, Colorimetria
- **4** Serviços com preços e duração
- **4** Atendimentos agendados/realizados

> [!TIP]
> Para fazer login como **Cliente**, use por exemplo o CPF: `22222222212`
> Para fazer login como **Profissional**, use por exemplo o CPF: `11111111102`
