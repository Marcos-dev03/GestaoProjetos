using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gestão_de_projetos.Controllers
{
	[Authorize]
	public class HomeController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}

		[AllowAnonymous]
		public IActionResult Error()
		{
			return View();
		}

		[AllowAnonymous]
		public IActionResult Privacy()
		{
			return View();
		}
	}
}