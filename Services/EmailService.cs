using System.Net;
using System.Net.Mail;

namespace Gestão_de_projetos.Services
{
	public class EmailService
	{
		private readonly IConfiguration _configuration;

		public EmailService(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		public async Task EnviarAsync(
			string destinatario,
			string assunto,
			string mensagem)
		{
			var emailConfig = _configuration.GetSection("Email");

			string remetente = emailConfig["Remetente"] ?? throw new InvalidOperationException("O remetente do e-mail não foi configurado."); ;
			string senha = emailConfig["Senha"] ?? throw new InvalidOperationException("A senha do e-mail não foi configurada.");
			string servidor = emailConfig["Servidor"] ?? throw new InvalidOperationException("O servidor do e-mail não foi configurado.");
			int porta = int.Parse(emailConfig["Porta"] ?? throw new InvalidOperationException("A porta do e-mail não foi configurada."));

			using var smtp = new SmtpClient(servidor, porta);

			smtp.EnableSsl = true;
			smtp.Credentials = new NetworkCredential(
				remetente,
				senha);

			using var mail = new MailMessage();

			mail.From = new MailAddress(
				remetente,
				"Gestão+");

			mail.To.Add(destinatario);

			mail.Subject = assunto;

			mail.Body = mensagem;

			mail.IsBodyHtml = true;

			await smtp.SendMailAsync(mail);
		}
	}
}