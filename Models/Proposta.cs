using System.ComponentModel.DataAnnotations;

namespace Gestão_de_projetos.Models
{
	public class Proposta
	{
		[Key]
		public int IdProposta { get; set; }

		[Required]
		public string Descricao { get; set; } = string.Empty;

		[Required]
		public int TipoProposta { get; set; }

		[Required]
		public decimal Valor { get; set; }

		[Required]
		public int IdProjeto { get; set; }

		public Projeto Projeto { get; set; } = null!;
	}
}