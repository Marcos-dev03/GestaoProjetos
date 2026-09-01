using Gestão_de_projetos.Models;
using Gestão_de_projetos.Models.Infra;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Gestão_de_projetos.BData
{
	public class BDContext : IdentityDbContext<UsuarioDaAplicacao>
	{
		public BDContext(DbContextOptions<BDContext> options)
			: base(options)
		{
		}

		public DbSet<Projeto> Projetos { get; set; }

		public DbSet<ProjetoUsuario> ProjetoUsuarios { get; set; }

		public DbSet<Proposta> Propostas { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<ProjetoUsuario>()
				.HasKey(pu => new
				{
					pu.IdProjeto,
					pu.IdUsuario
				});

			modelBuilder.Entity<ProjetoUsuario>()
				.HasOne(pu => pu.Projeto)
				.WithMany(p => p.ProjetoUsuarios)
				.HasForeignKey(pu => pu.IdProjeto);

			modelBuilder.Entity<ProjetoUsuario>()
				.HasOne(pu => pu.Usuario)
				.WithMany()
				.HasForeignKey(pu => pu.IdUsuario);

			modelBuilder.Entity<Proposta>()
				.HasOne(p => p.Projeto)
				.WithMany(p => p.Propostas)
				.HasForeignKey(p => p.IdProjeto);

			modelBuilder.Entity<Proposta>()
				.Property(p => p.Valor)
				.HasPrecision(18, 2);
		}
	}
}