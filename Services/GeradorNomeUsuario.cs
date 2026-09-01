using Gestão_de_projetos.Models.Infra;
using Microsoft.AspNetCore.Identity;
using System.Globalization;
using System.Text;

namespace Gestão_de_projetos.Services
{
	public class GeradorNomeUsuario
	{
		private readonly UserManager<UsuarioDaAplicacao> _userManager;

		public GeradorNomeUsuario(
			UserManager<UsuarioDaAplicacao> userManager)
		{
			_userManager = userManager;
		}

		public async Task<string> GerarAsync(string nomeCompleto)
		{
			string[] partes = nomeCompleto
				.Trim()
				.Split(' ', StringSplitOptions.RemoveEmptyEntries);

			if (partes.Length == 0)
			{
				throw new ArgumentException("Nome inválido.");
			}

			string primeiroNome = RemoverAcentos(partes[0]);

			// Exemplo:
			// Marcos Fernando
			// Marcos.Fernando
			string nomeUsuario = primeiroNome;

			if (partes.Length >= 2)
			{
				string segundoNome = RemoverAcentos(partes[1]);

				nomeUsuario = $"{primeiroNome}.{segundoNome}";
			}

			if (!await ExisteAsync(nomeUsuario))
			{
				return nomeUsuario;
			}

			// Exemplo:
			// Marcos Fernando Costa
			// Marcos.Fernando.Costa
			for (int i = 2; i < partes.Length; i++)
			{
				string parte = RemoverAcentos(partes[i]);

				string tentativa = $"{nomeUsuario}.{parte}";

				if (!await ExisteAsync(tentativa))
				{
					return tentativa;
				}
			}

			// Somente se todas as combinações de nomes já existirem,
			// usamos número como último recurso.
			int numero = 2;

			while (await ExisteAsync($"{nomeUsuario}{numero}"))
			{
				numero++;
			}

			return $"{nomeUsuario}{numero}";
		}

		private async Task<bool> ExisteAsync(string nomeUsuario)
		{
			var usuario = await _userManager.FindByNameAsync(nomeUsuario);

			return usuario != null;
		}

		private static string RemoverAcentos(string texto)
		{
			string normalizado = texto.Normalize(NormalizationForm.FormD);

			StringBuilder resultado = new StringBuilder();

			foreach (char caractere in normalizado)
			{
				UnicodeCategory categoria =
					CharUnicodeInfo.GetUnicodeCategory(caractere);

				if (categoria != UnicodeCategory.NonSpacingMark)
				{
					resultado.Append(caractere);
				}
			}

			return resultado
				.ToString()
				.Normalize(NormalizationForm.FormC);
		}
	}
}