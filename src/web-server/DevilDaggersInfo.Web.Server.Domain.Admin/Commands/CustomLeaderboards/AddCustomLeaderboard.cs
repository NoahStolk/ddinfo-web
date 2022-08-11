using DevilDaggersInfo.Types.Web;
using DevilDaggersInfo.Web.Server.Domain.Admin.Commands.CustomLeaderboards.Models;

namespace DevilDaggersInfo.Web.Server.Domain.Admin.Commands.CustomLeaderboards;

public record AddCustomLeaderboard
{
	[Required]
	public int SpawnsetId { get; init; }

	[Required]
	public CustomLeaderboardCategory Category { get; init; }

	public CustomLeaderboardDaggers Daggers { get; init; } = new();

	[Required]
	public bool IsFeatured { get; init; }
}
