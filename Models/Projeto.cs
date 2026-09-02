using System.ComponentModel.DataAnnotations;

namespace Gestão_de_projetos.Models
{
	public class Projeto
	{
		[Key]
		public int IdProjeto { get; set; }

		[Required]
		public string Nome { get; set; } = string.Empty;

		[Required]
		public string Descricao { get; set; } = string.Empty;

		[Required]
		public DateTime DataInicio { get; set; }

		[Required]
		public DateTime DataFim { get; set; }

		public ICollection<Proposta> Propostas { get; set; } = new List<Proposta>();

		public ICollection<ProjetoUsuario> ProjetoUsuarios { get; set; } = new List<ProjetoUsuario>();
	}
}