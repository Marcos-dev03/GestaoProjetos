using Resend;

namespace Gestão_de_projetos.Services
{
	public class EmailService
	{
		private readonly IResend _resend;
		private readonly ILogger<EmailService> _logger;
		private readonly IConfiguration _configuration;

		public EmailService(
			IResend resend,
			ILogger<EmailService> logger,
			IConfiguration configuration)
		{
			_resend = resend;
			_logger = logger;
			_configuration = configuration;

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
				var remetente = _configuration["Email:Remetente"];

				if (string.IsNullOrWhiteSpace(remetente))
				{
					throw new InvalidOperationException(
						"O remetente do e-mail não foi configurado.");
				}

				_logger.LogInformation(
					"Preparando envio através do Resend. Remetente: {Remetente}",
					remetente);

				var email = new EmailMessage
				{
					From = remetente,
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