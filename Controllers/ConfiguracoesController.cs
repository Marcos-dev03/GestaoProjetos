using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gestão_de_projetos.Controllers
{
	[Authorize(Roles = "Admin,Configuracoes")]
	public class ConfiguracoesController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}