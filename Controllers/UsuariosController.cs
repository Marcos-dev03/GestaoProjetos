using Gestão_de_projetos.Models.Infra;
using Gestão_de_projetos.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Gestão_de_projetos.Controllers
{
	[Authorize(Roles = "Admin")]
	public class UsuariosController : Controller
	{
		private readonly UserManager<UsuarioDaAplicacao> _userManager;
		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly GeradorNomeUsuario _geradorNomeUsuario;

		public UsuariosController(
			UserManager<UsuarioDaAplicacao> userManager,
			RoleManager<IdentityRole> roleManager,
			GeradorNomeUsuario geradorNomeUsuario)
		{
			_userManager = userManager;
			_roleManager = roleManager;
			_geradorNomeUsuario = geradorNomeUsuario;
		}

		public async Task<IActionResult> Index()
		{
			var usuarios = await _userManager.Users.ToListAsync();

			return View(usuarios);
		}

		public async Task<IActionResult> Details(string id)
		{
			if (string.IsNullOrWhiteSpace(id))
			{
				return NotFound();
			}

			var usuario =
				await _userManager.FindByIdAsync(id);

			if (usuario == null)
			{
				return NotFound();
			}

			return View(usuario);
		}

		[HttpGet]
		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(
			RegistrarNovoUsuarioViewModel model)
		{
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

			var usuario = new UsuarioDaAplicacao
			{
				UserName = nomeUsuario,
				Email = model.Email.Trim(),
				Nome = model.Nome.Trim(),
				NomeCadastrado = model.Nome.Trim()
			};

			var resultado =
				await _userManager.CreateAsync(
					usuario,
					model.Senha);

			if (!resultado.Succeeded)
			{
				foreach (var erro in resultado.Errors)
				{
					ModelState.AddModelError(
						string.Empty,
						erro.Description);
				}

				return View(model);
			}

			var permissoes = new List<string>();

			if (model.AcessoProjetos)
			{
				permissoes.Add("Projetos");
			}

			if (model.AcessoPropostas)
			{
				permissoes.Add("Propostas");
			}

			if (model.AcessoConfiguracoes)
			{
				permissoes.Add("Configuracoes");
			}

			if (permissoes.Any())
			{
				var resultadoRoles =
					await _userManager.AddToRolesAsync(
						usuario,
						permissoes);

				if (!resultadoRoles.Succeeded)
				{
					foreach (var erro in resultadoRoles.Errors)
					{
						ModelState.AddModelError(
							string.Empty,
							erro.Description);
					}

					await _userManager.DeleteAsync(usuario);

					return View(model);
				}
			}

			return RedirectToAction(nameof(Index));
		}

		[HttpGet]
		public async Task<IActionResult> Edit(string id)
		{
			if (string.IsNullOrWhiteSpace(id))
			{
				return NotFound();
			}

			var usuario =
				await _userManager.FindByIdAsync(id);

			if (usuario == null)
			{
				return NotFound();
			}

			var roles =
				await _userManager.GetRolesAsync(usuario);

			var model = new EditarUsuarioViewModel
			{
				Id = usuario.Id,
				Nome = usuario.Nome,
				Email = usuario.Email
	?? throw new InvalidOperationException(
		"O e-mail do usuário não pode ser nulo."),
				AcessoProjetos =
					roles.Contains("Projetos"),

				AcessoPropostas =
					roles.Contains("Propostas"),

				AcessoConfiguracoes =
					roles.Contains("Configuracoes")
			};

			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(
			EditarUsuarioViewModel model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}

			var usuario =
				await _userManager.FindByIdAsync(model.Id);

			if (usuario == null)
			{
				return NotFound();
			}

			var outroUsuario =
				await _userManager.FindByEmailAsync(
					model.Email.Trim());

			if (outroUsuario != null &&
				outroUsuario.Id != usuario.Id)
			{
				ModelState.AddModelError(
					"Email",
					"Este e-mail já está sendo utilizado.");

				return View(model);
			}

			usuario.Nome = model.Nome.Trim();
			usuario.NomeCadastrado = model.Nome.Trim();
			usuario.Email = model.Email.Trim();

			var resultado =
				await _userManager.UpdateAsync(usuario);

			if (!resultado.Succeeded)
			{
				foreach (var erro in resultado.Errors)
				{
					ModelState.AddModelError(
						string.Empty,
						erro.Description);
				}

				return View(model);
			}

			var rolesAtuais =
				await _userManager.GetRolesAsync(usuario);

			var rolesSistema = new[]
			{
				"Projetos",
				"Propostas",
				"Configuracoes"
			};

			var rolesParaRemover =
				rolesAtuais
					.Where(r => rolesSistema.Contains(r))
					.ToList();

			if (rolesParaRemover.Any())
			{
				await _userManager.RemoveFromRolesAsync(
					usuario,
					rolesParaRemover);
			}

			var novasRoles = new List<string>();

			if (model.AcessoProjetos)
			{
				novasRoles.Add("Projetos");
			}

			if (model.AcessoPropostas)
			{
				novasRoles.Add("Propostas");
			}

			if (model.AcessoConfiguracoes)
			{
				novasRoles.Add("Configuracoes");
			}

			if (novasRoles.Any())
			{
				await _userManager.AddToRolesAsync(
					usuario,
					novasRoles);
			}

			return RedirectToAction(nameof(Index));
		}

		[HttpGet]
		public async Task<IActionResult> Delete(string id)
		{
			if (string.IsNullOrWhiteSpace(id))
			{
				return NotFound();
			}

			var usuario =
				await _userManager.FindByIdAsync(id);

			if (usuario == null)
			{
				return NotFound();
			}

			return View(usuario);
		}

		[HttpPost]
		[ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(
			string id)
		{
			if (string.IsNullOrWhiteSpace(id))
			{
				return NotFound();
			}

			var usuario =
				await _userManager.FindByIdAsync(id);

			if (usuario == null)
			{
				return NotFound();
			}

			if (usuario.Id == _userManager.GetUserId(User))
			{
				ModelState.AddModelError(
					string.Empty,
					"Você não pode excluir sua própria conta.");

				return View("Delete", usuario);
			}

			var resultado =
				await _userManager.DeleteAsync(usuario);

			if (!resultado.Succeeded)
			{
				foreach (var erro in resultado.Errors)
				{
					ModelState.AddModelError(
						string.Empty,
						erro.Description);
				}

				return View("Delete", usuario);
			}

			return RedirectToAction(nameof(Index));
		}
	}
}