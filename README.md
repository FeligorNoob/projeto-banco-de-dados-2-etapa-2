# 💈 BarberFlow — Sistema de Agendamento para Barbearia

Aplicação console em **C# (.NET 10)** desenvolvida como projeto acadêmico de Banco de Dados 2. A aplicação suporta **dois bancos de dados** e permite escolher qual usar **na inicialização**:

- **MySQL** (relacional) via **Entity Framework Core** + Pomelo;
- **MongoDB** (NoSQL) via o **driver oficial** MongoDB.Driver.

Ambos operam sobre as mesmas funcionalidades e interfaces — a troca é feita por um menu de seleção quando o programa inicia. Você só precisa configurar (e ter no ar) o banco que pretende usar.

> **Repositório:** https://github.com/FeligorNoob/projeto-banco-de-dados-2-etapa-2.git

---

## 📋 O que você vai precisar instalar

Antes de tudo, certifique-se de que as ferramentas abaixo estão instaladas na máquina. Cada uma tem seu link de download e instruções:

| Ferramenta | Para que serve | Download |
|------------|---------------|----------|
| **Git** | Clonar o repositório | https://git-scm.com/downloads |
| **MySQL 8.0+** | Banco de dados relacional (opção 1) | https://dev.mysql.com/downloads/installer/ |
| **MongoDB 6.0+** | Banco de dados NoSQL (opção 2) | https://www.mongodb.com/try/download/community |
| **mongosh** | Cliente de linha de comando do MongoDB (para rodar o seed) | https://www.mongodb.com/try/download/shell |
| **.NET 10 SDK** | Compilar e executar o projeto C# | https://dotnet.microsoft.com/download/dotnet/10.0 |

> 💡 Você **não precisa** instalar os dois bancos. Instale apenas o que for usar (MySQL **ou** MongoDB). Se quiser testar os dois, instale ambos.

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

### Passo 2 — Instalar o Banco de Dados (escolha o seu)

> Faça **apenas** o passo do banco que você vai usar: **2A** para MySQL **ou** **2B** para MongoDB. Se quiser testar os dois, faça ambos.

#### Passo 2A — Instalar o MySQL

1. Acesse https://dev.mysql.com/downloads/installer/
2. Baixe o **MySQL Installer for Windows** (o arquivo maior, "Full")
3. Execute o instalador e escolha o tipo **"Server Only"** (ou "Developer Default")
4. Na etapa de configuração do servidor:
   - **Porta:** mantenha `3306` (padrão) — **anote essa porta, você vai precisar dela**
   - **Authentication Method:** escolha "Use Strong Password Encryption"
   - **Root Password:** defina uma senha que você não vai esquecer (ex: `root`) — **anote essa senha**
5. Finalize a instalação

> **Verificação:** Abra o **MySQL Command Line Client** (instalado junto com o MySQL) e tente fazer login com a senha que você definiu. Se abrir sem erros, está funcionando. ✅

#### Passo 2B — Instalar o MongoDB

O MongoDB precisa de **duas** ferramentas: o **servidor** (MongoDB Community Server) e o **cliente de linha de comando** (mongosh), usado para rodar o seed no Passo 5B. Os instaladores recentes **não** trazem mais o `mongosh` embutido, por isso ele é baixado à parte.

**1. Instalar o MongoDB Community Server**

1. Acesse https://www.mongodb.com/try/download/community
2. Selecione:
   - **Version:** a última disponível (6.0 ou superior)
   - **Platform:** Windows
   - **Package:** `msi`
3. Clique em **Download** e execute o instalador
4. Escolha o tipo de instalação **"Complete"**
5. Na tela **"Service Configuration"**:
   - Mantenha marcado **"Install MongoD as a Service"** — isso faz o MongoDB **iniciar sozinho junto com o Windows** (recomendado)
   - Mantenha **"Run service as Network Service user"** e o nome do serviço como `MongoDB`
   - **Porta:** o padrão é `27017` — **anote, você vai precisar dela**
6. (Opcional) Deixe marcado **"Install MongoDB Compass"** se quiser uma interface gráfica para visualizar os dados
7. Conclua a instalação clicando em **Next / Install**

**2. Instalar o mongosh (MongoDB Shell)**

1. Acesse https://www.mongodb.com/try/download/shell
2. Em **Package**, selecione `msi` (Platform: Windows) e clique em **Download**
3. Execute o instalador e siga com as opções padrão (**Next → Install**)

> **Verificação:** Abra um novo **Prompt de Comando** ou **PowerShell** e execute:
>
> ```
> mongosh --version
> ```
>
> Se aparecer um número de versão (ex: `2.x.x`), o cliente está instalado. Para confirmar que o **servidor** está no ar, execute `mongosh` — se conectar e abrir o prompt `test>`, está funcionando. Digite `exit` para sair. ✅
>
> Se der erro de conexão, verifique se o serviço está rodando em **Painel de Controle → Serviços → "MongoDB"** (ou execute `net start MongoDB` no Prompt de Comando como administrador).

---

### Passo 3 — Instalar o .NET 10 SDK

1. Acesse https://dotnet.microsoft.com/download/dotnet/10.0
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
git clone https://github.com/FeligorNoob/projeto-banco-de-dados-2-etapa-2.git
```

Isso vai criar uma pasta chamada `projeto-banco-de-dados-2-etapa-2` com todos os arquivos do projeto.

Em seguida, entre na pasta do projeto:

```
cd projeto-banco-de-dados-2-etapa-2
```

---

### Passo 5 — Importar os Dados (escolha o seu banco)

> Faça **apenas** o passo correspondente ao banco que você vai usar: **5A** para MySQL **ou** **5B** para MongoDB.

#### Passo 5A — Importar os Dados no MySQL

O banco de dados já está pronto para uso no arquivo de dump localizado em:

```
dump-BarberFlow-202605070336.sql
```

Este arquivo contém toda a estrutura das tabelas e os dados de exemplo já inseridos (usuários, serviços, especialidades e atendimentos).

**Para importar, execute o comando abaixo no Prompt de Comando** (substituindo `SUA_SENHA` pela senha do MySQL que você definiu no Passo 2A):

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

#### Passo 5B — Popular os Dados no MongoDB

O MongoDB não usa o dump `.sql`. Em vez disso, o projeto traz um **script de seed equivalente** (com os mesmos dados de exemplo) em:

```
mongo-seed-BarberFlow.js
```

Com o **serviço do MongoDB no ar** e o **mongosh** instalado, rode na raiz do projeto:

```
mongosh "mongodb://localhost:27017" mongo-seed-BarberFlow.js
```

O script **recria** o banco `BarberFlow` do zero (dropa as coleções e insere os dados), cria os índices necessários (CPF único e chave composta de profissional × especialidade) e configura os contadores de id. Pode ser executado quantas vezes quiser para restaurar os dados. Ao final, ele imprime a contagem de documentos de cada coleção.

> 💡 O banco e as coleções são criados automaticamente pelo script — você **não** precisa criá-los antes (diferente do MySQL).

---

### Passo 6 — Configurar as Conexões

Abra o arquivo `BarberApplication.Console/appsettings.json` com qualquer editor de texto (Bloco de Notas, VS Code, etc.). Ele guarda as **duas** conexões — você ajusta a do banco que for usar:

```json
{
  "ConnectionStrings": {
    "MySql": "server=localhost;port=3306;database=BarberFlow;user=root;password=root",
    "Mongo": "mongodb://localhost:27017"
  },
  "MongoDatabase": "BarberFlow"
}
```

**Conexão MySQL** (`ConnectionStrings:MySql`) — ajuste conforme sua instalação:

| Parâmetro  | Valor Padrão | O que mudar                          |
|------------|--------------|--------------------------------------|
| `server`   | `localhost`  | Manter se o MySQL está na mesma máquina |
| `port`     | `3306`       | Mudar se escolheu outra porta no Passo 2A |
| `database` | `BarberFlow` | Manter                               |
| `user`     | `root`       | Manter (ou o usuário que você criou) |
| `password` | `root`       | **Alterar para a sua senha do MySQL** |

**Conexão MongoDB** (`ConnectionStrings:Mongo` + `MongoDatabase`):

| Parâmetro        | Valor Padrão                | O que mudar                                        |
|------------------|-----------------------------|----------------------------------------------------|
| `Mongo`          | `mongodb://localhost:27017` | Mudar host/porta se o MongoDB não estiver local ou usar outra porta |
| `MongoDatabase`  | `BarberFlow`                | Manter (deve bater com o banco criado pelo seed)   |

> ⚠️ **Configure corretamente a conexão do banco que você escolher** — caso contrário a aplicação não conseguirá conectar. Você só precisa acertar a conexão do banco que vai usar.

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

Se aparecer erros, verifique se o .NET 10 SDK está instalado corretamente (Passo 3).

---

### Passo 8 — Executar a Aplicação

```
dotnet run
```

**Primeiro, você escolhe o banco de dados** que será usado nesta execução:

```
  ╔══════════════════════════════════════════════╗
  ║  BarberFlow — Seleção de Banco de Dados       ║
  ╚══════════════════════════════════════════════╝

  Qual banco de dados deseja utilizar?
  1. MySQL   (Entity Framework Core)
  2. MongoDB (driver oficial)
  0. Sair
```

Digite `1` para **MySQL** ou `2` para **MongoDB** (o banco escolhido precisa estar no ar e configurado conforme o Passo 6). Se a conexão falhar, o programa avisa e volta para esta tela.

Em seguida, aparece o menu principal:

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

> ℹ️ As funcionalidades são **idênticas** nos dois bancos. A escolha vale apenas para a execução atual — para trocar de banco, saia e rode `dotnet run` novamente.

---

## 👥 Perfis de Acesso

| Perfil | Como acessar | Funcionalidades |
|--------|-------------|-----------------|
| **Cliente** | CPF `22222222212` | Agendar serviço, ver/cancelar agendamentos, ver serviços |
| **Profissional** | CPF `11111111102` | Ver agenda, marcar atendimentos como realizados, ver especialidades |
| **Administrador** | Menu 3 (sem login) | Consultas, cadastros, remoções, relatórios |
| **Central de Dados** | Menu 4 (sem login) | CRUD completo de todas as entidades |

---

## 📊 Dados de Exemplo (MySQL e MongoDB)

Tanto o dump do MySQL (`dump-BarberFlow-202605070336.sql`) quanto o seed do MongoDB (`mongo-seed-BarberFlow.js`) trazem **os mesmos** dados prontos para uso:

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
- Verifique se o banco `BarberFlow` foi criado antes da importação (Passo 5A)
- Confirme o nome do banco no `appsettings.json` (campo `database=BarberFlow`)

### (MongoDB) "A timeout occurred" / "No connection could be made" ao escolher a opção 2
- Verifique se o serviço do MongoDB está rodando (Painel de Controle → Serviços → procure "MongoDB")
- Confirme o host/porta em `ConnectionStrings:Mongo` no `appsettings.json` (padrão `mongodb://localhost:27017`)

### (MongoDB) A aplicação abre mas não aparece nenhum dado
- Rode o seed antes de usar: `mongosh "mongodb://localhost:27017" mongo-seed-BarberFlow.js` (Passo 5B)
- Confirme que `MongoDatabase` no `appsettings.json` é `BarberFlow` (mesmo banco criado pelo seed)
