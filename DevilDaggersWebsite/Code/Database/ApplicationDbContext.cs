using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DevilDaggersWebsite.Code.Database
{
	public class ApplicationDbContext : IdentityDbContext
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
			: base(options)
		{
		}

		public DbSet<AssetMod> AssetMods { get; set; }
		public DbSet<CustomEntry> CustomEntries { get; set; }
		public DbSet<CustomLeaderboard> CustomLeaderboards { get; set; }
		public DbSet<CustomLeaderboardCategory> CustomLeaderboardCategories { get; set; }
		public DbSet<Donation> Donations { get; set; }
		public DbSet<Player> Player { get; set; }
	}
}