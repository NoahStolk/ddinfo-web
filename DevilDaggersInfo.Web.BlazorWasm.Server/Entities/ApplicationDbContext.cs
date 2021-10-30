namespace DevilDaggersInfo.Web.BlazorWasm.Server.Entities;

public class ApplicationDbContext : DbContext
{
	public ApplicationDbContext(DbContextOptions options)
		: base(options)
	{
	}

	public virtual DbSet<CustomEntryEntity> CustomEntries => Set<CustomEntryEntity>();
	public virtual DbSet<CustomEntryDataEntity> CustomEntryData => Set<CustomEntryDataEntity>();
	public virtual DbSet<CustomLeaderboardEntity> CustomLeaderboards => Set<CustomLeaderboardEntity>();
	public virtual DbSet<DonationEntity> Donations => Set<DonationEntity>();
	public virtual DbSet<MarkerEntity> Markers => Set<MarkerEntity>();
	public virtual DbSet<ModEntity> Mods => Set<ModEntity>();
	public virtual DbSet<PlayerEntity> Players => Set<PlayerEntity>();
	public virtual DbSet<SpawnsetEntity> Spawnsets => Set<SpawnsetEntity>();
	public virtual DbSet<TitleEntity> Titles => Set<TitleEntity>();
	public virtual DbSet<ToolEntity> Tools => Set<ToolEntity>();
	public virtual DbSet<ToolStatisticEntity> ToolStatistics => Set<ToolStatisticEntity>();

	public virtual DbSet<PlayerModEntity> PlayerMods => Set<PlayerModEntity>();
	public virtual DbSet<PlayerTitleEntity> PlayerTitles => Set<PlayerTitleEntity>();

	public virtual DbSet<InformationSchemaTable> InformationSchemaTables => Set<InformationSchemaTable>();

	public virtual DbSet<UserEntity> Users => Set<UserEntity>();
	public virtual DbSet<RoleEntity> Roles => Set<RoleEntity>();
	public virtual DbSet<UserRoleEntity> UserRoles => Set<UserRoleEntity>();

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		// Configure relations for PlayerMods.
		modelBuilder.Entity<PlayerModEntity>()
			.HasKey(pm => new { pm.PlayerId, pm.ModId });

		modelBuilder.Entity<PlayerModEntity>()
			.HasOne(pm => pm.Player)
			.WithMany(p => p.PlayerMods)
			.HasForeignKey(pm => pm.PlayerId);

		modelBuilder.Entity<PlayerModEntity>()
			.HasOne(pm => pm.Mod)
			.WithMany(m => m.PlayerMods)
			.HasForeignKey(pm => pm.ModId);

		// Configure relations for PlayerTitles.
		modelBuilder.Entity<PlayerTitleEntity>()
			.HasKey(pt => new { pt.PlayerId, pt.TitleId });

		modelBuilder.Entity<PlayerTitleEntity>()
			.HasOne(pt => pt.Player)
			.WithMany(p => p.PlayerTitles)
			.HasForeignKey(pt => pt.PlayerId);

		modelBuilder.Entity<PlayerTitleEntity>()
			.HasOne(pt => pt.Title)
			.WithMany(t => t.PlayerTitles)
			.HasForeignKey(pt => pt.TitleId);

		// Configure relations for UserRoles.
		modelBuilder.Entity<UserRoleEntity>()
			.HasKey(ur => new { ur.UserId, ur.RoleId });

		modelBuilder.Entity<UserRoleEntity>()
			.HasOne(ur => ur.User)
			.WithMany(u => u.UserRoles)
			.HasForeignKey(ur => ur.UserId);

		modelBuilder.Entity<UserRoleEntity>()
			.HasOne(ur => ur.Role)
			.WithMany(r => r.UserRoles)
			.HasForeignKey(ur => ur.RoleId);

		base.OnModelCreating(modelBuilder);
	}
}
