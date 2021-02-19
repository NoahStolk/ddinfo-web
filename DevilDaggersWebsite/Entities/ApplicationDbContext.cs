using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DevilDaggersWebsite.Entities
{
	public class ApplicationDbContext : IdentityDbContext
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
			: base(options)
		{
		}

		public DbSet<AssetMod> AssetMods => Set<AssetMod>();
		public DbSet<CustomEntry> CustomEntries => Set<CustomEntry>();
		public DbSet<CustomEntryData> CustomEntryData => Set<CustomEntryData>();
		public DbSet<CustomLeaderboard> CustomLeaderboards => Set<CustomLeaderboard>();
		public DbSet<Donation> Donations => Set<Donation>();
		public DbSet<Player> Players => Set<Player>();
		public DbSet<SpawnsetFile> SpawnsetFiles => Set<SpawnsetFile>();
		public DbSet<Title> Titles => Set<Title>();
		public DbSet<ToolStatistic> ToolStatistics => Set<ToolStatistic>();

		public DbSet<PlayerAssetMod> PlayerAssetMods => Set<PlayerAssetMod>();
		public DbSet<PlayerTitle> PlayerTitles => Set<PlayerTitle>();

		protected override void OnModelCreating(ModelBuilder builder)
		{
			builder.Entity<PlayerAssetMod>()
				.HasKey(pam => new { pam.PlayerId, pam.AssetModId });

			builder.Entity<PlayerAssetMod>()
				.HasOne(pam => pam.Player)
				.WithMany(p => p.PlayerAssetMods)
				.HasForeignKey(pam => pam.PlayerId);

			builder.Entity<PlayerAssetMod>()
				.HasOne(pam => pam.AssetMod)
				.WithMany(am => am.PlayerAssetMods)
				.HasForeignKey(pam => pam.AssetModId);

			builder.Entity<PlayerTitle>()
				.HasKey(pt => new { pt.PlayerId, pt.TitleId });

			builder.Entity<PlayerTitle>()
				.HasOne(pt => pt.Player)
				.WithMany(p => p.PlayerTitles)
				.HasForeignKey(pt => pt.PlayerId);

			builder.Entity<PlayerTitle>()
				.HasOne(pt => pt.Title)
				.WithMany(t => t.PlayerTitles)
				.HasForeignKey(pt => pt.TitleId);

			base.OnModelCreating(builder);
		}
	}
}
