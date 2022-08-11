namespace DevilDaggersInfo.Web.Server.Domain.Commands.CustomLeaderboards.Models;

public record CustomLeaderboardDaggers
{
	public int Bronze { get; init; }

	public int Silver { get; init; }

	public int Golden { get; init; }

	public int Devil { get; init; }

	public int Leviathan { get; init; }
}
