using Microsoft.AspNetCore.Identity;

namespace Gestão_de_projetos.Models.Infra
{
	public class UsuarioDaAplicacao : IdentityUser
	{
		public string Nome { get; set; }

		public string NomeCadastrado { get; set; }
	}
}