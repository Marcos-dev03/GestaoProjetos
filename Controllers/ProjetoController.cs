using Gestão_de_projetos.BData;
using Gestão_de_projetos.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Authorize(Roles = "Admin,Projetos")]
public class ProjetoController : Controller
{
	private readonly BDContext _context;

	public ProjetoController(BDContext context)
	{
		_context = context;
	}

	public async Task<IActionResult> Index()
	{
		return View(await _context.Projetos.ToListAsync());
	}

	public async Task<IActionResult> Details(int? id)
	{
		if (id == null)
		{
			return NotFound();
		}

		var projeto = await _context.Projetos
			.FirstOrDefaultAsync(p => p.IdProjeto == id);

		if (projeto == null)
		{
			return NotFound();
		}

		return View(projeto);
	}

	public IActionResult Create()
	{
		return View();
	}

	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Create(
		[Bind("IdProjeto,Nome,Descricao,DataInicio,DataFim,Propostas,Usuarios")]
		Projeto projeto)
	{
		if (ModelState.IsValid)
		{
			_context.Projetos.Add(projeto);

			await _context.SaveChangesAsync();

			return RedirectToAction(nameof(Index));
		}

		return View(projeto);
	}

	public async Task<IActionResult> Edit(int? id)
	{
		if (id == null)
		{
			return NotFound();
		}

		var projeto = await _context.Projetos.FindAsync(id);

		if (projeto == null)
		{
			return NotFound();
		}

		return View(projeto);
	}

	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Edit(
		int id,
		[Bind("IdProjeto,Nome,Descricao,DataInicio,DataFim")]
		Projeto projeto)
	{
		if (id != projeto.IdProjeto)
		{
			return NotFound();
		}

		if (ModelState.IsValid)
		{
			try
			{
				_context.Projetos.Update(projeto);

				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!ProjetoExists(projeto.IdProjeto))
				{
					return NotFound();
				}

				throw;
			}

			return RedirectToAction(nameof(Index));
		}

		return View(projeto);
	}

	public async Task<IActionResult> Delete(int? id)
	{
		if (id == null)
		{
			return NotFound();
		}

		var projeto = await _context.Projetos
			.FirstOrDefaultAsync(p => p.IdProjeto == id);

		if (projeto == null)
		{
			return NotFound();
		}

		return View(projeto);
	}

	[HttpPost, ActionName("Delete")]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> DeleteConfirmed(int id)
	{
		var projeto = await _context.Projetos.FindAsync(id);

		if (projeto != null)
		{
			_context.Projetos.Remove(projeto);

			await _context.SaveChangesAsync();
		}

		return RedirectToAction(nameof(Index));
	}

	private bool ProjetoExists(int id)
	{
		return _context.Projetos.Any(p => p.IdProjeto == id);
	}
}