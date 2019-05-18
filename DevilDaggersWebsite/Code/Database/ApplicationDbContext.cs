using DevilDaggersWebsite.Code.Database.CustomLeaderboards;
using Microsoft.EntityFrameworkCore;

namespace DevilDaggersWebsite.Code.Database
{
	public class ApplicationDbContext : DbContext
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
			: base(options)
		{
		}

		public DbSet<CustomLeaderboard> CustomLeaderboards { get; set; }
		public DbSet<CustomEntry> CustomEntries { get; set; }
	}
}