using Gestão_de_projetos.Models.Infra;
using Microsoft.AspNetCore.Identity;

namespace Gestão_de_projetos
{
	public static class Permissoes
	{
		public static async Task InicializarAsync(
			IServiceProvider serviceProvider)
		{
			var roleManager =
				serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

			string[] roles =
			{
				"Admin",
				"Projetos",
				"Propostas",
				"Configuracoes"
			};

			foreach (var role in roles)
			{
				if (!await roleManager.RoleExistsAsync(role))
				{
					await roleManager.CreateAsync(
						new IdentityRole(role));
				}
			}
		}
	}
}