using Gestão_de_projetos.Models.Infra;
using Gestão_de_projetos.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Gestão_de_projetos.Controllers
{
	[Authorize]
	public class InfraController : Controller
	{
		private readonly UserManager<UsuarioDaAplicacao> _userManager;
		private readonly SignInManager<UsuarioDaAplicacao> _signInManager;
		private readonly GeradorNomeUsuario _geradorNomeUsuario;
		private readonly EmailService _emailService;
		private readonly ILogger<InfraController> _logger;

		public InfraController(
			UserManager<UsuarioDaAplicacao> userManager,
			SignInManager<UsuarioDaAplicacao> signInManager,
			GeradorNomeUsuario geradorNomeUsuario,
			EmailService emailService,
			ILogger<InfraController> logger)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_geradorNomeUsuario = geradorNomeUsuario;
			_emailService = emailService;
			_logger = logger;
		}

		[HttpGet]
		[AllowAnonymous]
		public async Task<IActionResult> Acessar(string returnUrl = null)
		{
			await _signInManager.SignOutAsync();

			await HttpContext.SignOutAsync(
				IdentityConstants.ExternalScheme);

			ViewData["ReturnUrl"] = returnUrl;

			return View();
		}

		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Acessar(
	AcessarViewModel model,
	string returnUrl = null)
		{
			ViewData["ReturnUrl"] = returnUrl;

			if (!ModelState.IsValid)
			{
				return View(model);
			}

			var nomeUsuario = model.NomeUsuario?.Trim();

			if (string.IsNullOrWhiteSpace(nomeUsuario))
			{
				ModelState.AddModelError(
					"NomeUsuario",
					"Informe o nome de usuário.");

				return View(model);
			}

			var user = await _userManager.FindByNameAsync(nomeUsuario);

			if (user == null)
			{
				ModelState.AddModelError(
					string.Empty,
					"Usuário ou senha inválidos.");

				return View(model);
			}

			var result = await _signInManager.PasswordSignInAsync(
				user,
				model.Senha,
				model.LembrarDeMim,
				lockoutOnFailure: false);

			if (result.Succeeded)
			{
				_logger.LogInformation(
					"Usuário {NomeUsuario} acessou o sistema.",
					user.UserName);

				return RedirectToLocal(returnUrl);
			}

			if (result.IsLockedOut)
			{
				ModelState.AddModelError(
					string.Empty,
					"Esta conta está temporariamente bloqueada.");

				return View(model);
			}

			ModelState.AddModelError(
				string.Empty,
				"Usuário ou senha inválidos.");

			return View(model);
		}

		[HttpGet]
		[AllowAnonymous]
		public IActionResult RegistrarNovoUsuario(
			string returnUrl = null)
		{
			ViewData["ReturnUrl"] = returnUrl;

			return View();
		}

		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> RegistrarNovoUsuario(
			RegistrarNovoUsuarioViewModel model,
			string returnUrl = null)
		{
			ViewData["ReturnUrl"] = returnUrl;

			if (!ModelState.IsValid)
			{
				return View(model);
			}

			var emailExistente =
				await _userManager.FindByEmailAsync(
					model.Email.Trim());

			if (emailExistente != null)
			{
				ModelState.AddModelError(
					"Email",
					"Este e-mail já está sendo utilizado.");

				return View(model);
			}

			string nomeUsuario =
				await _geradorNomeUsuario.GerarAsync(
					model.Nome.Trim());

			var user = new UsuarioDaAplicacao
			{
				Nome = model.Nome.Trim(),
				NomeCadastrado = model.Nome.Trim(),
				UserName = nomeUsuario,
				Email = model.Email.Trim()
			};

			var result =
				await _userManager.CreateAsync(
					user,
					model.Senha);

			if (result.Succeeded)
			{
				var permissoes = new List<string>
	{
		"Projetos",
		"Propostas",
		"Configuracoes"
	};

				var resultadoRoles =
					await _userManager.AddToRolesAsync(
						user,
						permissoes);

				if (!resultadoRoles.Succeeded)
				{
					foreach (var erro in resultadoRoles.Errors)
					{
						ModelState.AddModelError(
							string.Empty,
							erro.Description);
					}

					await _userManager.DeleteAsync(user);

					return View(model);
				}

				_logger.LogInformation(
					"Usuário {NomeUsuario} criou uma nova conta.",
					nomeUsuario);

				await _signInManager.SignInAsync(
					user,
					isPersistent: false);

				return RedirectToLocal(returnUrl);
			}

			AddErrors(result);

			return View(model);
		}

		[HttpGet]
		[AllowAnonymous]
		public IActionResult EsqueciSenha()
		{
			return View();
		}

		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> EsqueciSenha(
			EsqueciSenhaViewModel model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}

			var user =
				await _userManager.FindByEmailAsync(
					model.Email.Trim());

			if (user == null)
			{
				ModelState.AddModelError(
					"Email",
					"Este e-mail não está cadastrado no sistema.");

				return View(model);
			}

			var token =
				await _userManager.GeneratePasswordResetTokenAsync(
					user);

			var resetUrl = Url.Action(
				nameof(ResetarSenha),
				"Infra",
				new
				{
					email = user.Email,
					token = token
				},
				Request.Scheme);

			var mensagem = $@"
<!DOCTYPE html>
<html lang='pt-br'>
<head>
	<meta charset='UTF-8'>
	<title>Redefinição de senha</title>
</head>

<body style='
	font-family: Arial, sans-serif;
	background-color: #05051a;
	color: #ffffff;
	padding: 40px;
'>

	<div style='
		max-width: 500px;
		margin: 0 auto;
		background: #0d0d28;
		padding: 35px;
		border-radius: 15px;
	'>

		<h2 style='color: #ffffff;'>
			Gestão+
		</h2>

		<h3 style='color: #ffffff;'>
			Redefinição de senha
		</h3>

		<p>
			Olá, <strong>{user.NomeCadastrado}</strong>.
		</p>

		<p>
			Recebemos uma solicitação para redefinir
			a senha da sua conta.
		</p>

		<p>
			Clique no botão abaixo para criar uma nova senha:
		</p>

		<p>
			<a href='{resetUrl}'
			   style='
					display:inline-block;
					padding:12px 20px;
					background:#584cff;
					color:white;
					text-decoration:none;
					border-radius:6px;
			   '>
				Redefinir minha senha
			</a>
		</p>

		<p>
			Se você não solicitou a redefinição de senha,
			pode ignorar este e-mail.
		</p>

		<p>
			<strong>Gestão+</strong>
		</p>

	</div>

</body>
</html>
";

			try
			{
				await _emailService.EnviarAsync(
					user.Email,
					"Redefinição de senha - Gestão+",
					mensagem);
			}
			catch (Exception ex)
			{
				_logger.LogError(
					ex,
					"Erro ao enviar e-mail de recuperação.");

				ModelState.AddModelError(
					string.Empty,
					"Não foi possível enviar o e-mail de recuperação. Tente novamente.");

				return View(model);
			}

			return RedirectToAction(
				nameof(EmailEnviado));
		}

		[HttpGet]
		[AllowAnonymous]
		public IActionResult EmailEnviado()
		{
			return View();
		}

		[HttpGet]
		[AllowAnonymous]
		public IActionResult AcessoNegado()
		{
			return View();
		}

		[HttpGet]
		[AllowAnonymous]
		public IActionResult ResetarSenha(
			string email,
			string token)
		{
			if (string.IsNullOrEmpty(email) ||
				string.IsNullOrEmpty(token))
			{
				return RedirectToAction(
					nameof(Acessar));
			}

			var model = new ResetarSenhaViewModel
			{
				Email = email,
				Token = token
			};

			return View(model);
		}

		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> ResetarSenha(
			ResetarSenhaViewModel model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}

			var user =
				await _userManager.FindByEmailAsync(
					model.Email);

			if (user == null)
			{
				ModelState.AddModelError(
					string.Empty,
					"Não foi possível redefinir a senha.");

				return View(model);
			}

			var result =
				await _userManager.ResetPasswordAsync(
					user,
					model.Token,
					model.NovaSenha);

			if (result.Succeeded)
			{
				_logger.LogInformation(
					"Senha redefinida com sucesso para {Email}.",
					user.Email);

				return RedirectToAction(
					nameof(Acessar));
			}

			AddErrors(result);

			return View(model);
		}

		[HttpGet]
		public async Task<IActionResult> Sair()
		{
			await _signInManager.SignOutAsync();

			_logger.LogInformation(
				"Usuário saiu do sistema.");

			return RedirectToAction(
				nameof(HomeController.Index),
				"Home");
		}

		private IActionResult RedirectToLocal(
			string returnUrl)
		{
			if (Url.IsLocalUrl(returnUrl))
			{
				return Redirect(returnUrl);
			}

			return RedirectToAction(
				"Index",
				"Home");
		}

		private void AddErrors(
			IdentityResult result)
		{
			foreach (var error in result.Errors)
			{
				ModelState.AddModelError(
					string.Empty,
					error.Description);
			}
		}
	}
}