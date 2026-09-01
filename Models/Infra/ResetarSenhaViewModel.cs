using System.ComponentModel.DataAnnotations;

namespace Gestão_de_projetos.Models.Infra
{
	public class ResetarSenhaViewModel
	{
		public string Email { get; set; }

		public string Token { get; set; }

		[Required]
		[DataType(DataType.Password)]
		[Display(Name = "Nova senha")]
		public string NovaSenha { get; set; }

		[Required]
		[DataType(DataType.Password)]
		[Compare("NovaSenha")]
		[Display(Name = "Confirmar nova senha")]
		public string ConfirmarNovaSenha { get; set; }
	}
}