using Gestão_de_projetos.BData;
using Gestão_de_projetos.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Authorize(Roles = "Admin,Propostas")]
public class PropostaController : Controller
{
	private readonly BDContext _context;

	public PropostaController(BDContext context)
	{
		_context = context;
	}

	public async Task<IActionResult> Index()
	{
		return View(await _context.Propostas.ToListAsync());
	}

	public async Task<IActionResult> Details(int? id)
	{
		if (id == null)
		{
			return NotFound();
		}

		var proposta = await _context.Propostas
			.FirstOrDefaultAsync(p => p.IdProposta == id);

		if (proposta == null)
		{
			return NotFound();
		}

		return View(proposta);
	}

	public IActionResult Create()
	{
		return View();
	}

	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Create(
		[Bind("IdProposta,Descricao,TipoProposta,Valor,IdProjeto")]
		Proposta proposta)
	{
		if (ModelState.IsValid)
		{
			_context.Propostas.Add(proposta);

			await _context.SaveChangesAsync();

			return RedirectToAction(nameof(Index));
		}

		return View(proposta);
	}

	public async Task<IActionResult> Edit(int? id)
	{
		if (id == null)
		{
			return NotFound();
		}

		var proposta = await _context.Propostas.FindAsync(id);

		if (proposta == null)
		{
			return NotFound();
		}

		return View(proposta);
	}

	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Edit(
		int id,
		[Bind("IdProposta,Descricao,TipoProposta,Valor,IdProjeto")]
		Proposta proposta)
	{
		if (id != proposta.IdProposta)
		{
			return NotFound();
		}

		if (ModelState.IsValid)
		{
			try
			{
				_context.Propostas.Update(proposta);

				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!PropostaExists(proposta.IdProposta))
				{
					return NotFound();
				}

				throw;
			}

			return RedirectToAction(nameof(Index));
		}

		return View(proposta);
	}

	public async Task<IActionResult> Delete(int? id)
	{
		if (id == null)
		{
			return NotFound();
		}

		var proposta = await _context.Propostas
			.FirstOrDefaultAsync(p => p.IdProposta == id);

		if (proposta == null)
		{
			return NotFound();
		}

		return View(proposta);
	}

	[HttpPost, ActionName("Delete")]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> DeleteConfirmed(int id)
	{
		var proposta = await _context.Propostas.FindAsync(id);

		if (proposta != null)
		{
			_context.Propostas.Remove(proposta);

			await _context.SaveChangesAsync();
		}

		return RedirectToAction(nameof(Index));
	}

	private bool PropostaExists(int id)
	{
		return _context.Propostas.Any(p => p.IdProposta == id);
	}
}