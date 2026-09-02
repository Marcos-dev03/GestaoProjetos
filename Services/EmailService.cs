using Resend;

namespace Gestão_de_projetos.Services
{
	public class EmailService
	{
		private readonly IResend _resend;
		private readonly ILogger<EmailService> _logger;

		public EmailService(
			IResend resend,
			ILogger<EmailService> logger)
		{
			_resend = resend;
			_logger = logger;

			_logger.LogInformation("EmailService inicializado.");
		}

		public async Task EnviarAsync(
			string destinatario,
			string assunto,
			string mensagem)
		{
			_logger.LogInformation(
				"Iniciando envio de e-mail para {Destinatario}. Assunto: {Assunto}",
				destinatario,
				assunto);

			try
			{
				var email = new EmailMessage
				{
					From = "Gestao Projetos <onboarding@resend.dev>",
					Subject = assunto,
					HtmlBody = mensagem
				};

				email.To.Add(destinatario);

				_logger.LogInformation(
					"Enviando e-mail através da API do Resend...");

				var resposta = await _resend.EmailSendAsync(email);

				_logger.LogInformation(
					"E-mail enviado com sucesso. ID do Resend: {EmailId}",
					resposta.Content);
			}
			catch (Exception ex)
			{
				_logger.LogError(
					ex,
					"Erro ao enviar e-mail para {Destinatario}.",
					destinatario);

				throw;
			}
			finally
			{
				_logger.LogInformation(
					"Processo de envio de e-mail finalizado para {Destinatario}.",
					destinatario);
			}
		}
	}
}