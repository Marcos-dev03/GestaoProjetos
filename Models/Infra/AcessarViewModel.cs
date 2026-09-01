using System.ComponentModel.DataAnnotations;

namespace Gestão_de_projetos.Models.Infra
{
	public class AcessarViewModel
	{
		[Required]
		[Display(Name = "Nome do Usuário")]
		public string NomeUsuario { get; set; }

		[Required]
		[DataType(DataType.Password)]
		public string Senha { get; set; }

		[Display(Name = "Lembrar de mim?")]
		public bool LembrarDeMim { get; set; }
	}
}