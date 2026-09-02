using Microsoft.AspNetCore.Identity;

namespace Gestão_de_projetos.Models.Infra
{
	public class UsuarioDaAplicacao : IdentityUser
	{
		public string Nome { get; set; } = string.Empty;

		public string NomeCadastrado { get; set; } = string.Empty;
	}
}