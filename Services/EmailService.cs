using System.Net;
using System.Net.Mail;

namespace Gestão_de_projetos.Services
{
	public class EmailService
	{
		private readonly IConfiguration _configuration;
		private readonly ILogger<EmailService> _logger;

		public EmailService(
			IConfiguration configuration,
			ILogger<EmailService> logger)
		{
			_configuration = configuration;
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

				_logger.LogInformation("Carregando configurações da seção 'Email'.");

				var emailConfig = _configuration.GetSection("Email");

				string remetente = emailConfig["Remetente"]
					?? throw new InvalidOperationException(
						"O remetente do e-mail não foi configurado.");

				string senha = emailConfig["Senha"]
					?? throw new InvalidOperationException(
						"A senha do e-mail não foi configurada.");

				string servidor = emailConfig["Servidor"]
					?? throw new InvalidOperationException(
						"O servidor do e-mail não foi configurado.");

				int porta = int.Parse(
					emailConfig["Porta"]
					?? throw new InvalidOperationException(
						"A porta do e-mail não foi configurada.")
				);

				_logger.LogInformation(
					"Configuração carregada. Servidor: {Servidor}, Porta: {Porta}, Remetente: {Remetente}",
					servidor,
					porta,
					remetente);

				_logger.LogInformation("Criando cliente SMTP.");

				using var smtp = new SmtpClient(servidor, porta)
				{

					EnableSsl = true,
					Timeout = 3000,
					Credentials = new NetworkCredential(remetente, senha)
				};

				_logger.LogInformation(
					"Cliente SMTP configurado. SSL habilitado: {Ssl}",
					smtp.EnableSsl);

				_logger.LogInformation("Criando objeto MailMessage.");

				using var mail = new MailMessage();

				mail.From = new MailAddress(
					remetente,
					"Gestão+");

				_logger.LogInformation(
					"Remetente definido: {Remetente}",
					remetente);

				mail.To.Add(destinatario);

				_logger.LogInformation(
					"Destinatário definido: {Destinatario}",
					destinatario);

				mail.Subject = assunto;

				_logger.LogInformation(
					"Assunto definido: {Assunto}",
					assunto);

				mail.Body = mensagem;

				mail.IsBodyHtml = true;

				_logger.LogInformation(
					"Corpo do e-mail configurado. HTML: {Html}",
					mail.IsBodyHtml);

				_logger.LogInformation(
					"Enviando e-mail através do SMTP {Servidor}:{Porta}...",
					servidor,
					porta);

				_logger.LogInformation("ANTES do SendMailAsync");

				await smtp.SendMailAsync(mail)
					.WaitAsync(TimeSpan.FromSeconds(15));

				_logger.LogInformation("DEPOIS do SendMailAsync");

				_logger.LogInformation(
					"E-mail enviado com sucesso para {Destinatario}.",
					destinatario);
			}
			catch (FormatException ex)
			{
				_logger.LogError(
					ex,
					"Erro de formato na configuração do e-mail. Verifique principalmente a porta SMTP.");

				throw;
			}
			catch (SmtpException ex)
			{
				_logger.LogError(
					ex,
					"Erro SMTP ao tentar enviar e-mail para {Destinatario}. Status: {Status}",
					destinatario,
					ex.StatusCode);

				throw;
			}
			catch (Exception ex)
			{
				_logger.LogError(
					ex,
					"Erro inesperado ao enviar e-mail para {Destinatario}.",
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