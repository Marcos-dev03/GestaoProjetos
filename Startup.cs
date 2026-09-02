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
			services.AddOptions();

			services.AddHttpClient<ResendClient>();

			services.Configure<ResendClientOptions>(options =>
			{
				options.ApiToken = Configuration["Email:ApiKey"];
			});

			services.AddTransient<IResend, ResendClient>();
			services.AddDbContext<BDContext>(options =>
				options.UseNpgsql(
					Configuration.GetConnectionString("BDContext")));

			services.AddIdentity<UsuarioDaAplicacao, IdentityRole>(options =>
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

			services.AddScoped<GeradorNomeUsuario>();
			services.AddScoped<EmailService>();

			services.AddAuthorization();

			services.ConfigureApplicationCookie(options =>
			{
				options.LoginPath = "/Infra/Acessar";
				options.AccessDeniedPath = "/Infra/AcessoNegado";
			});

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

			//app.UseHttpsRedirection();

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

		public static async Task CriarAdmin(IHost host)
		{
			using (var scope = host.Services.CreateScope())
			{
				var serviceProvider = scope.ServiceProvider;

				var roleManager =
					serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

				var userManager =
					serviceProvider.GetRequiredService<UserManager<UsuarioDaAplicacao>>();

				const string nomeRole = "Admin";

				if (!await roleManager.RoleExistsAsync(nomeRole))
				{
					await roleManager.CreateAsync(
						new IdentityRole(nomeRole));
				}

				var usuarios =
					await userManager.Users.ToListAsync();

				if (!usuarios.Any())
				{
					return;
				}

				foreach (var usuario in usuarios)
				{
					if (await userManager.IsInRoleAsync(
						usuario,
						nomeRole))
					{
						return;
					}
				}

				var primeiroUsuario = usuarios.First();

				await userManager.AddToRoleAsync(
					primeiroUsuario,
					nomeRole);
			}
		}
	}
}