using DevilDaggersInfo.Web.Server.Domain.Commands.CustomLeaderboards.Models;
using DevilDaggersInfo.Web.Server.Domain.Entities.Enums;

namespace DevilDaggersInfo.Web.Server.Domain.Commands.CustomLeaderboards;

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
