using IdentityServer4.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace DevilDaggersWebsite.BlazorWasm.Server.Entities
{
	public class ApplicationDbContext : ApiAuthorizationDbContext<ApplicationUser>
	{
		public ApplicationDbContext(DbContextOptions options, IOptions<OperationalStoreOptions> operationalStoreOptions)
			: base(options, operationalStoreOptions)
		{
		}

		public virtual DbSet<AssetMod> AssetMods => Set<AssetMod>();
		public virtual DbSet<CustomEntry> CustomEntries => Set<CustomEntry>();
		public virtual DbSet<CustomEntryData> CustomEntryData => Set<CustomEntryData>();
		public virtual DbSet<CustomLeaderboard> CustomLeaderboards => Set<CustomLeaderboard>();
		public virtual DbSet<Donation> Donations => Set<Donation>();
		public virtual DbSet<Player> Players => Set<Player>();
		public virtual DbSet<SpawnsetFile> SpawnsetFiles => Set<SpawnsetFile>();
		public virtual DbSet<Title> Titles => Set<Title>();
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
