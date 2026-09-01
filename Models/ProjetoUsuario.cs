using Gestão_de_projetos.Models.Infra;

namespace Gestão_de_projetos.Models
{
	public class ProjetoUsuario
	{
		public int IdProjeto { get; set; }

		public string IdUsuario { get; set; }

		public string NivelDeAcesso { get; set; }

		public Projeto Projeto { get; set; }

		public UsuarioDaAplicacao Usuario { get; set; }
	}
}