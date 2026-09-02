using Gestão_de_projetos.Models.Infra;

namespace Gestão_de_projetos.Models
{
	public class ProjetoUsuario
	{
		public int IdProjeto { get; set; }

		public string IdUsuario { get; set; } = string.Empty;

		public string NivelDeAcesso { get; set; } = string.Empty;

		public Projeto Projeto { get; set; } = null!;

		public UsuarioDaAplicacao Usuario { get; set; } = null!;
	}
}