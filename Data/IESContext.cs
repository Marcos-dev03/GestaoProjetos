using Microsoft.EntityFrameworkCore;

namespace Gestão_de_projetos.Data
{
	public class IESContext : DbContext
	{
		public IESContext(DbContextOptions<IESContext> options)
			: base(options)
		{
		}
	}
}