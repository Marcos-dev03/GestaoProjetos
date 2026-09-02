using System.ComponentModel.DataAnnotations;

namespace Gestão_de_projetos.Models.Infra
{
	public class EditarUsuarioViewModel
	{
		public string Id { get; set; } = string.Empty;

		[Required(ErrorMessage = "Informe o nome.")]
		[Display(Name = "Nome")]
		public string Nome { get; set; } = string.Empty;

		[Required(ErrorMessage = "Informe o e-mail.")]
		[EmailAddress(ErrorMessage = "Informe um e-mail válido.")]
		[Display(Name = "E-mail")]
		public string Email { get; set; } = string.Empty;
		[Display(Name = "Projetos")]
		public bool AcessoProjetos { get; set; }

		[Display(Name = "Propostas")]
		public bool AcessoPropostas { get; set; }

		[Display(Name = "Configurações")]
		public bool AcessoConfiguracoes { get; set; }
	}
}