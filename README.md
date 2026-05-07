# 💈 BarberFlow — Sistema de Agendamento para Barbearia

Aplicação console em **C# (.NET 10)** com **Entity Framework Core** e banco de dados **MySQL**, desenvolvida como projeto acadêmico de Banco de Dados 2.

> **Repositório:** https://github.com/FeligorNoob/projeto-banco-de-dados-2.git

---

## 📋 O que você vai precisar instalar

Antes de tudo, certifique-se de que as ferramentas abaixo estão instaladas na máquina. Cada uma tem seu link de download e instruções:

| Ferramenta | Para que serve | Download |
|------------|---------------|----------|
| **Git** | Clonar o repositório | https://git-scm.com/downloads |
| **MySQL 8.0+** | Banco de dados da aplicação | https://dev.mysql.com/downloads/installer/ |
| **.NET 10 SDK** | Compilar e executar o projeto C# | https://dotnet.microsoft.com/download/dotnet/10.0 |

---

## 🚀 Passo a Passo Completo

### Passo 1 — Instalar o Git

1. Acesse https://git-scm.com/downloads e baixe o instalador para Windows
2. Execute o instalador e clique em **Next** em todas as telas (as opções padrão funcionam bem)
3. Ao final, abra o **Prompt de Comando** (cmd) ou **PowerShell** e execute:

```
git --version
```

Se aparecer algo como `git version 2.x.x`, a instalação foi bem-sucedida. ✅

---

### Passo 2 — Instalar o MySQL

1. Acesse https://dev.mysql.com/downloads/installer/
2. Baixe o **MySQL Installer for Windows** (o arquivo maior, "Full")
3. Execute o instalador e escolha o tipo **"Server Only"** (ou "Developer Default")
4. Na etapa de configuração do servidor:
   - **Porta:** mantenha `3306` (padrão) — **anote essa porta, você vai precisar dela**
   - **Authentication Method:** escolha "Use Strong Password Encryption"
   - **Root Password:** defina uma senha que você não vai esquecer (ex: `root`) — **anote essa senha**
5. Finalize a instalação

> **Verificação:** Abra o **MySQL Command Line Client** (instalado junto com o MySQL) e tente fazer login com a senha que você definiu. Se abrir sem erros, está funcionando. ✅

---

### Passo 3 — Instalar o .NET 9 SDK

1. Acesse https://dotnet.microsoft.com/download/dotnet/9.0
2. Na seção **SDK**, clique em **"Download"** para Windows x64
3. Execute o instalador
4. Ao final, abra um novo **Prompt de Comando** e execute:

```
dotnet --version
```

Se aparecer algo como `10.0.x`, a instalação foi bem-sucedida. ✅

---

### Passo 4 — Clonar o Repositório

Abra o **Prompt de Comando** ou **PowerShell**, navegue até uma pasta de sua preferência (ex: `Documentos`) e execute:

```
git clone https://github.com/FeligorNoob/projeto-banco-de-dados-2.git
```

Isso vai criar uma pasta chamada `projeto-banco-de-dados-2` com todos os arquivos do projeto.

Em seguida, entre na pasta do projeto:

```
cd projeto-banco-de-dados-2
```

---

### Passo 5 — Importar o Banco de Dados

O banco de dados já está pronto para uso no arquivo de dump localizado em:

```
dump-BarberFlow-202605070336.sql
```

Este arquivo contém toda a estrutura das tabelas e os dados de exemplo já inseridos (usuários, serviços, especialidades e atendimentos).

**Para importar, execute o comando abaixo no Prompt de Comando** (substituindo `SUA_SENHA` pela senha do MySQL que você definiu no Passo 2):

```
mysql -u root -p < dump-BarberFlow-202605070336.sql
```

Quando solicitado, digite a senha do MySQL.

> ⚠️ **Importante:** O dump **não cria o banco de dados automaticamente** — você precisa criá-lo antes. Execute os dois comandos a seguir em ordem:

**1. Criar o banco de dados:**
```
mysql -u root -p -e "CREATE DATABASE IF NOT EXISTS BarberFlow;"
```

**2. Importar o dump:**
```
mysql -u root -p BarberFlow < dump-BarberFlow-202605070336.sql
```

> **Alternativa via MySQL Workbench (interface gráfica):**
> 1. Abra o **MySQL Workbench** (instalado junto com o MySQL)
> 2. Conecte-se ao servidor local
> 3. Crie um schema chamado `BarberFlow` (clique com botão direito em "Schemas" → "Create Schema")
> 4. Vá em **Server → Data Import**
> 5. Escolha "Import from Self-Contained File" e selecione o arquivo `dump-BarberFlow-202605070336.sql`
> 6. Em "Default Target Schema", selecione `BarberFlow`
> 7. Clique em **"Start Import"**

---

### Passo 6 — Configurar a Conexão com o Banco

Abra o arquivo `BarberApplication.Console/appsettings.json` com qualquer editor de texto (Bloco de Notas, VS Code, etc.):

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "server=localhost;port=3306;database=BarberFlow;user=root;password=root"
  }
}
```

**Ajuste os valores conforme sua instalação:**

| Parâmetro  | Valor Padrão | O que mudar                          |
|------------|--------------|--------------------------------------|
| `server`   | `localhost`  | Manter se o MySQL está na mesma máquina |
| `port`     | `3306`       | Mudar se escolheu outra porta no Passo 2 |
| `database` | `BarberFlow` | Manter                               |
| `user`     | `root`       | Manter (ou o usuário que você criou) |
| `password` | `root`       | **Alterar para a sua senha do MySQL** |

> ⚠️ **Se não alterar a senha correta aqui, a aplicação não vai conseguir conectar ao banco.**

---

### Passo 7 — Compilar o Projeto

Ainda no Prompt de Comando, entre na pasta do projeto C# e restaure as dependências:

```
cd BarberApplication.Console
dotnet restore
```

Em seguida, compile:

```
dotnet build
```

A saída esperada deve conter:

```
Build succeeded.
    0 Warning(s)
    0 Error(s)
```

Se aparecer erros, verifique se o .NET 9 SDK está instalado corretamente (Passo 3).

---

### Passo 8 — Executar a Aplicação

```
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
  4. Central de Dados (CRUDs Base)
  0. Sair
```

---

## 👥 Perfis de Acesso

| Perfil | Como acessar | Funcionalidades |
|--------|-------------|-----------------|
| **Cliente** | CPF `22222222212` | Agendar serviço, ver/cancelar agendamentos, ver serviços |
| **Profissional** | CPF `11111111102` | Ver agenda, marcar atendimentos como realizados, ver especialidades |
| **Administrador** | Menu 3 (sem login) | Consultas, cadastros, remoções, relatórios |
| **Central de Dados** | Menu 4 (sem login) | CRUD completo de todas as entidades |

---

## 📊 Dados de Exemplo (já incluídos no dump)

O arquivo de dump já contém os seguintes dados prontos para uso:

- **1** Administrador (CPF: `99999999999`)
- **10** Profissionais (CPFs: `11111111102` até `11111111111`)
- **20+** Clientes (CPFs: `22222222212` até `22222222220` + mocks)
- **3** Especialidades: Cortes de Cabelo, Barba, Colorimetria
- **4** Serviços: Corte Social (R$50), Corte Degradê (R$55), Colorir Cabelo (R$75), Aparar Barba (R$40)
- **15** Atendimentos com status variados (Agendado, Realizado, Cancelado)

---

## ❓ Problemas Comuns

### "Unable to connect to any of the specified MySQL hosts"
- Verifique se o serviço do MySQL está rodando (Painel de Controle → Serviços → procure "MySQL")
- Confirme a porta e senha no `appsettings.json`

### "dotnet: command not found" / "'dotnet' não é reconhecido"
- O .NET SDK não foi instalado ou a máquina precisa ser reiniciada após a instalação
- Feche e abra novamente o Prompt de Comando após instalar

### "Access denied for user 'root'"
- A senha no `appsettings.json` está incorreta
- Corrija o campo `password` com a senha definida durante a instalação do MySQL

### O banco foi importado mas a aplicação não encontra as tabelas
- Verifique se o banco `BarberFlow` foi criado antes da importação (Passo 5)
- Confirme o nome do banco no `appsettings.json` (campo `database=BarberFlow`)
