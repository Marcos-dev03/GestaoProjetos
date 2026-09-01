using System.ComponentModel.DataAnnotations;

namespace Gestão_de_projetos.Models.Infra
{
	public class EsqueciSenhaViewModel
	{
		[Required(ErrorMessage = "Informe seu e-mail.")]
		[EmailAddress(ErrorMessage = "Informe um e-mail válido.")]
		public string Email { get; set; }
	}
}