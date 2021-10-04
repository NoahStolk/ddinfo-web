using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DevilDaggersWebsite.Entities
{
	public class ApplicationDbContext : IdentityDbContext
	{
		public ApplicationDbContext()
		{
		}

		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
			: base(options)
		{
		}

		public virtual DbSet<AssetMod> AssetMods => Set<AssetMod>();
		public virtual DbSet<CustomEntry> CustomEntries => Set<CustomEntry>();
		public virtual DbSet<CustomEntryData> CustomEntryData => Set<CustomEntryData>();
		public virtual DbSet<CustomLeaderboard> CustomLeaderboards => Set<CustomLeaderboard>();
		public virtual DbSet<Donation> Donations => Set<Donation>();
		public virtual DbSet<Marker> Markers => Set<Marker>();
		public virtual DbSet<Player> Players => Set<Player>();
		public virtual DbSet<SpawnsetFile> SpawnsetFiles => Set<SpawnsetFile>();
		public virtual DbSet<Title> Titles => Set<Title>();
		public virtual DbSet<Tool> Tools => Set<Tool>();
		public virtual DbSet<ToolStatistic> ToolStatistics => Set<ToolStatistic>();

		public virtual DbSet<PlayerAssetMod> PlayerAssetMods => Set<PlayerAssetMod>();
		public virtual DbSet<PlayerTitle> PlayerTitles => Set<PlayerTitle>();

		public virtual DbSet<InformationSchemaTable> InformationSchemaTables => Set<InformationSchemaTable>();

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
