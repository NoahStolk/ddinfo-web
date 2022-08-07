using Microsoft.EntityFrameworkCore;

namespace DevilDaggersInfo.Web.Server.Domain.Entities;

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
	public virtual DbSet<ToolEntity> Tools => Set<ToolEntity>();
	public virtual DbSet<ToolDistributionEntity> ToolDistributions => Set<ToolDistributionEntity>();

	public virtual DbSet<PlayerModEntity> PlayerMods => Set<PlayerModEntity>();

	public virtual DbSet<InformationSchemaTable> InformationSchemaTables => Set<InformationSchemaTable>();

	public virtual DbSet<UserEntity> Users => Set<UserEntity>();
	public virtual DbSet<RoleEntity> Roles => Set<RoleEntity>();
	public virtual DbSet<UserRoleEntity> UserRoles => Set<UserRoleEntity>();

#if DEBUG
	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.LogTo(Console.WriteLine);
#endif

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

		// Configure relations for UserRoles.
		modelBuilder.Entity<UserRoleEntity>()
			.HasKey(ur => new { ur.UserId, ur.RoleName });

		modelBuilder.Entity<UserRoleEntity>()
			.HasOne(ur => ur.User)
			.WithMany(u => u.UserRoles)
			.HasForeignKey(ur => ur.UserId);

		modelBuilder.Entity<UserRoleEntity>()
			.HasOne(ur => ur.Role)
			.WithMany(r => r.UserRoles)
			.HasForeignKey(ur => ur.RoleName);

		base.OnModelCreating(modelBuilder);
	}
}
