using System.ComponentModel.DataAnnotations;

namespace Gestão_de_projetos.Models.Infra
{
	public class AcessarViewModel
	{
		[Required]
		[Display(Name = "Nome do Usuário")]
		public string NomeUsuario { get; set; } = string.Empty;

		[Required]
		[DataType(DataType.Password)]
		public string Senha { get; set; } = string.Empty;

		[Display(Name = "Lembrar de mim?")]
		public bool LembrarDeMim { get; set; }
	}
}