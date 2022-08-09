using DevilDaggersInfo.Web.Core.Claims;
using DevilDaggersInfo.Web.Server.Domain.Extensions;
using DevilDaggersInfo.Web.Server.Domain.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace DevilDaggersInfo.Web.Server.Domain.Entities;

public class ApplicationDbContext : DbContext
{
	private readonly IHttpContextAccessor _httpContextAccessor;
	private readonly ILogContainerService _logContainerService;

	public ApplicationDbContext(DbContextOptions options, IHttpContextAccessor httpContextAccessor, ILogContainerService logContainerService)
		: base(options)
	{
		_httpContextAccessor = httpContextAccessor;
		_logContainerService = logContainerService;
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

	public override int SaveChanges()
	{
		BuildAuditLogs();
		return base.SaveChanges();
	}

	public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
	{
		BuildAuditLogs();
		return await base.SaveChangesAsync(cancellationToken);
	}

	public void BuildAuditLogs()
	{
		string? username = _httpContextAccessor.HttpContext?.User?.GetName();

		IEnumerable<EntityEntry> entityEntries = ChangeTracker.Entries();
		foreach (EntityEntry addedOrDeletedEntityEntry in entityEntries.Where(e => e.State == EntityState.Added || e.State == EntityState.Deleted))
		{
			Type entityType = addedOrDeletedEntityEntry.Entity.GetType();
			if (addedOrDeletedEntityEntry.Entity is IAuditable entity)
				_logContainerService.AddAuditLog($"`{entityType.Name}` with ID `{entity.Id}` was {(addedOrDeletedEntityEntry.State == EntityState.Added ? "added" : "deleted")} by {username}.");
		}

		foreach (EntityEntry modifiedEntityEntry in entityEntries.Where(e => e.State == EntityState.Modified))
		{
			Type entityType = modifiedEntityEntry.Entity.GetType();
			if (modifiedEntityEntry.Entity is not IAuditable entity)
				return;

			List<string> logs = new();
			foreach (PropertyEntry modifiedProperty in modifiedEntityEntry.Properties.Where(c => c.IsModified))
			{
				string property = modifiedProperty.Metadata.PropertyInfo?.Name ?? "<null>";
				string oldValue = modifiedProperty.OriginalValue?.ToString()?.TrimAfter(25, true) ?? "<null>";
				string newValue = modifiedProperty.CurrentValue?.ToString()?.TrimAfter(25, true) ?? "<null>";
				logs.Add($"**{property}**: ~~{oldValue}~~ {newValue}");
			}

			_logContainerService.AddAuditLog($"`{entityType.Name}` with ID `{entity.Id}` was edited by {username}:\n{string.Join("\n- ", logs)}");
		}
	}
}
