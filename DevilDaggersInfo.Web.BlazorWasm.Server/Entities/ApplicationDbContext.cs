using IdentityServer4.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.Extensions.Options;

namespace DevilDaggersInfo.Web.BlazorWasm.Server.Entities;

public class ApplicationDbContext : ApiAuthorizationDbContext<ApplicationUser>
{
	public ApplicationDbContext(DbContextOptions options, IOptions<OperationalStoreOptions> operationalStoreOptions)
		: base(options, operationalStoreOptions)
	{
	}

	public virtual DbSet<CustomEntryEntity> CustomEntries => Set<CustomEntryEntity>();
	public virtual DbSet<CustomEntryDataEntity> CustomEntryData => Set<CustomEntryDataEntity>();
	public virtual DbSet<CustomLeaderboardEntity> CustomLeaderboards => Set<CustomLeaderboardEntity>();
	public virtual DbSet<DonationEntity> Donations => Set<DonationEntity>();
	public virtual DbSet<ModEntity> Mods => Set<ModEntity>();
	public virtual DbSet<PlayerEntity> Players => Set<PlayerEntity>();
	public virtual DbSet<SpawnsetEntity> Spawnsets => Set<SpawnsetEntity>();
	public virtual DbSet<TitleEntity> Titles => Set<TitleEntity>();
	public virtual DbSet<ToolEntity> Tools => Set<ToolEntity>();
	public virtual DbSet<ToolStatisticEntity> ToolStatistics => Set<ToolStatisticEntity>();

	public virtual DbSet<PlayerModEntity> PlayerMods => Set<PlayerModEntity>();
	public virtual DbSet<PlayerTitleEntity> PlayerTitles => Set<PlayerTitleEntity>();

	public virtual DbSet<InformationSchemaTable> InformationSchemaTables => Set<InformationSchemaTable>();

	protected override void OnModelCreating(ModelBuilder builder)
	{
		builder.Entity<PlayerModEntity>()
			.HasKey(pam => new { pam.PlayerId, pam.ModId });

		builder.Entity<PlayerModEntity>()
			.HasOne(pam => pam.Player)
			.WithMany(p => p.PlayerMods)
			.HasForeignKey(pam => pam.PlayerId);

		builder.Entity<PlayerModEntity>()
			.HasOne(pam => pam.Mod)
			.WithMany(am => am.PlayerMods)
			.HasForeignKey(pam => pam.ModId);

		builder.Entity<PlayerTitleEntity>()
			.HasKey(pt => new { pt.PlayerId, pt.TitleId });

		builder.Entity<PlayerTitleEntity>()
			.HasOne(pt => pt.Player)
			.WithMany(p => p.PlayerTitles)
			.HasForeignKey(pt => pt.PlayerId);

		builder.Entity<PlayerTitleEntity>()
			.HasOne(pt => pt.Title)
			.WithMany(t => t.PlayerTitles)
			.HasForeignKey(pt => pt.TitleId);

		base.OnModelCreating(builder);
	}
}
