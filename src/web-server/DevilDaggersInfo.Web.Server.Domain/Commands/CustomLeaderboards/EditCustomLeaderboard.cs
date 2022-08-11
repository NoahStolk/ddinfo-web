using DevilDaggersInfo.Web.Server.Domain.Commands.CustomLeaderboards.Models;
using DevilDaggersInfo.Web.Server.Domain.Entities.Enums;

namespace DevilDaggersInfo.Web.Server.Domain.Commands.CustomLeaderboards;

public record EditCustomLeaderboard
{
	public int Id { get; init; }

	public CustomLeaderboardCategory Category { get; init; }

	public CustomLeaderboardDaggers Daggers { get; init; } = new();

	public bool IsFeatured { get; init; }
}
