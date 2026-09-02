using System.ComponentModel.DataAnnotations;

namespace Gestão_de_projetos.Models.Infra
{
	public class RegistrarNovoUsuarioViewModel
	{
		[Required(ErrorMessage = "Informe o nome.")]
		[Display(Name = "Nome")]
		public string Nome { get; set; } = string.Empty;

		[Required(ErrorMessage = "Informe o e-mail.")]
		[EmailAddress(ErrorMessage = "Informe um e-mail válido.")]
		[Display(Name = "E-mail")]
		public string Email { get; set; } = string.Empty;

		[Required(ErrorMessage = "Informe a senha.")]
		[DataType(DataType.Password)]
		[Display(Name = "Senha")]
		public string Senha { get; set; } = string.Empty;

		[Required(ErrorMessage = "Confirme a senha.")]
		[DataType(DataType.Password)]
		[Compare("Senha", ErrorMessage = "As senhas não coincidem.")]
		[Display(Name = "Confirmar Senha")]
		public string ConfirmarSenha { get; set; } = string.Empty;

		public bool AcessoProjetos { get; set; }

		public bool AcessoPropostas { get; set; }

		public bool AcessoConfiguracoes { get; set; }
	}
}