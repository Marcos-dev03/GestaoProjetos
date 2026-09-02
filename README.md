# Gestão+

Sistema web para gerenciamento de projetos e propostas, feito em ASP.NET Core MVC.

# Sobre o projeto

O Gestão+ nasceu para organizar o fluxo de projetos e propostas de uma equipe: cadastro de usuários, controle de acesso por função e recuperação de senha por e-mail, tudo em um só lugar.

# Funcionalidades

- Cadastro, login e logout de usuários
- Recuperação de senha por e-mail
- Gerenciamento de usuários, projetos e propostas
- Controle de permissões por função
- Autenticação via ASP.NET Core Identity
- Dados persistidos em PostgreSQL

# Controle de acesso

Funções disponíveis:

- **Admin** — gerenciamento completo do sistema
- **Projetos** — gerenciamento de projetos
- **Propostas** — gerenciamento de propostas
- **Configurações** — acesso às configurações

Quem se cadastra pelo site recebe automaticamente as permissões de Projetos, Propostas e Configurações. A função Admin não é atribuída sozinha — precisa ser concedida manualmente.

# Tecnologias

- C# / .NET 8
- ASP.NET Core MVC
- Entity Framework Core
- ASP.NET Core Identity
- PostgreSQL / Npgsql
- Resend, para envio de e-mails
- HTML, CSS, JavaScript
- Git, GitHub, Render

# Estrutura

```text
Gestão de projetos/
│
├── BData/
│   └── BDContext.cs
│
├── Controllers/
│   ├── InfraController.cs
│   ├── ProjetoController.cs
│   ├── PropostaController.cs
│   └── UsuariosController.cs
│
├── Models/
│   └── ...
│
├── Services/
│   ├── EmailService.cs
│   └── GeradorNomeUsuario.cs
│
├── Views/
│   └── ...
│
├── Migrations/
│   └── ...
│
├── Properties/
│   └── launchSettings.json
│
├── Program.cs
├── Startup.cs
├── appsettings.json
└── Gestão de projetos.csproj
```

# Rodando localmente

Você vai precisar do .NET 8 SDK, PostgreSQL instalado e uma conta no Resend (ou outro provedor de e-mail) configurada.

```bash
git clone https://github.com/Marcos-dev03/GestaoProjetos
cd GestaoProjetos
```

Configure a connection string do PostgreSQL e as demais variáveis utilizando `appsettings.json`, User Secrets ou variáveis de ambiente.

Não versione senhas, chaves de API ou outras credenciais no repositório.

```bash
dotnet ef database update
```

E suba a aplicação:

```bash
dotnet run
```

Por padrão, a aplicação utiliza a porta definida em `Properties/launchSettings.json`.

## Autor

Desenvolvido por **Marcos Oliveira**.

Estudante de Desenvolvimento de Sistemas, com foco em desenvolvimento web utilizando C# e .NET.

**GitHub:** [Marcos-dev03](https://github.com/Marcos-dev03)

# Licença

MIT.