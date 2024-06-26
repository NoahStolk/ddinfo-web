using DevilDaggersInfo.Web.Server.Domain.Entities.Enums;
using DevilDaggersInfo.Web.Server.Domain.Services.Inversion;
using Microsoft.AspNetCore.Http;

namespace DevilDaggersInfo.Web.Server.Domain.Test.Utils;

public class TestDbContext : ApplicationDbContext
{
	public TestDbContext(DbContextOptions<TestDbContext> options, IHttpContextAccessor httpContextAccessor, ILogContainerService logContainerService)
		: base(options, httpContextAccessor, logContainerService)
	{
		List<PlayerEntity> players =
		[
			new() { Id = 1, BanType = BanType.NotBanned, PlayerName = "Player 1" },
			new() { Id = 2, BanType = BanType.NotBanned, PlayerName = "Player 2" },
			new() { Id = 3, BanType = BanType.NotBanned, PlayerName = "Player 3" },
			new() { Id = 4, BanType = BanType.Cheater, PlayerName = "Cheater" },
		];

		Players.AddRange(players);
		SaveChanges();
	}

	public override DbSet<PlayerEntity> Players => Set<PlayerEntity>();
}
