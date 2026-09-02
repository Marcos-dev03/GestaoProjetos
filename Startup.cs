using Gestão_de_projetos.BData;
using Gestão_de_projetos.Models.Infra;
using Gestão_de_projetos.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Resend;

namespace Gestão_de_projetos
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		public void ConfigureServices(IServiceCollection services)
		{
			// Resend
			services.AddOptions();

			services.AddHttpClient<ResendClient>();

			services.Configure<ResendClientOptions>(options =>
			{
				options.ApiToken =
					Environment.GetEnvironmentVariable(
						"RESEND_API_KEY");
			});

			services.AddTransient<IResend, ResendClient>();

			// Banco de dados
			services.AddDbContext<BDContext>(options =>
				options.UseNpgsql(
					Configuration.GetConnectionString("BDContext")));

			// Identity
			services.AddIdentity<UsuarioDaAplicacao, IdentityRole>(
				options =>
				{
					options.Password.RequiredLength = 8;
					options.Password.RequireDigit = true;
					options.Password.RequireLowercase = false;
					options.Password.RequireUppercase = true;
					options.Password.RequireNonAlphanumeric = true;

					options.User.RequireUniqueEmail = true;
				})
				.AddEntityFrameworkStores<BDContext>()
				.AddDefaultTokenProviders()
				.AddErrorDescriber<PortugueseIdentityErrorDescriber>();

			// Serviços da aplicação
			services.AddScoped<GeradorNomeUsuario>();
			services.AddScoped<EmailService>();

			// Autorização
			services.AddAuthorization();

			// Configuração do Cookie de autenticação
			services.ConfigureApplicationCookie(options =>
			{
				options.LoginPath = "/Infra/Acessar";
				options.AccessDeniedPath = "/Infra/AcessoNegado";
			});

			// MVC
			services.AddControllersWithViews();
		}

		public void Configure(
			IApplicationBuilder app,
			IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler("/Home/Error");
				app.UseHsts();
			}

			// HTTPS desabilitado por enquanto
			// app.UseHttpsRedirection();

			app.UseStaticFiles();

			app.UseRouting();

			app.UseAuthentication();

			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllerRoute(
					name: "default",
					pattern: "{controller=Infra}/{action=Acessar}/{id?}");
			});
		}

		// Cria as roles necessárias para o sistema.
		// Este método NÃO cria usuários e NÃO transforma
		// nenhum usuário em Admin.
		public static async Task CriarRoles(IHost host)
		{
			using (var scope = host.Services.CreateScope())
			{
				var serviceProvider = scope.ServiceProvider;

				var roleManager =
					serviceProvider.GetRequiredService<
						RoleManager<IdentityRole>>();

				var roles = new[]
				{
					"Admin",
					"Projetos",
					"Propostas",
					"Configuracoes"
				};

				foreach (var role in roles)
				{
					if (await roleManager.RoleExistsAsync(role))
					{
						continue;
					}

					var resultado =
						await roleManager.CreateAsync(
							new IdentityRole(role));

					if (!resultado.Succeeded)
					{
						var erros = string.Join(
							", ",
							resultado.Errors.Select(
								e => e.Description));

						throw new Exception(
							$"Erro ao criar a role '{role}': {erros}");
					}
				}
			}
		}
	}
}