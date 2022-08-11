using DevilDaggersInfo.Types.Web;
using DevilDaggersInfo.Web.Server.Domain.Admin.Commands.CustomLeaderboards.Models;

namespace DevilDaggersInfo.Web.Server.Domain.Admin.Commands.CustomLeaderboards;

public record EditCustomLeaderboard
{
	public int Id { get; init; }

	public CustomLeaderboardCategory Category { get; init; }

	public CustomLeaderboardDaggers Daggers { get; init; } = new();

	public bool IsFeatured { get; init; }
}
