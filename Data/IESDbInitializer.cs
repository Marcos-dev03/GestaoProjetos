using Gestão_de_projetos.BData;
using Gestão_de_projetos.Models;
using Microsoft.EntityFrameworkCore;

namespace Gestão_de_projetos.Data
{
	public class IESDbInitializer
	{
		public static void Initialize(BDContext context)
		{
			context.Database.Migrate();

			if (context.Propostas.Any())
			{
				return;
			}

			var projeto = context.Projetos.FirstOrDefault();

			if (projeto == null)
			{
				projeto = new Projeto
				{
					Nome = "Projeto Inicial",
					Descricao = "Projeto criado automaticamente pelo sistema",
					DataInicio = DateTime.Now,
					DataFim = DateTime.Now.AddDays(30)
				};

				context.Projetos.Add(projeto);
				context.SaveChanges();
			}

			var propostas = new Proposta[]
			{
				new Proposta
				{
					Descricao = "Proposta 1",
					TipoProposta = 1,
					IdProjeto = projeto.IdProjeto
				},

				new Proposta
				{
					Descricao = "Proposta 2",
					TipoProposta = 2,
					IdProjeto = projeto.IdProjeto
				},

				new Proposta
				{
					Descricao = "Proposta 3",
					TipoProposta = 3,
					IdProjeto = projeto.IdProjeto
				}
			};

			context.Propostas.AddRange(propostas);

			context.SaveChanges();
		}
	}
}