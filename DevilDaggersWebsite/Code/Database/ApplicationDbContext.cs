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

		public DbSet<CustomEntry> CustomEntries { get; set; }
		public DbSet<CustomLeaderboard> CustomLeaderboards { get; set; }
		public DbSet<CustomLeaderboardCategory> CustomLeaderboardCategories { get; set; }
	}
}