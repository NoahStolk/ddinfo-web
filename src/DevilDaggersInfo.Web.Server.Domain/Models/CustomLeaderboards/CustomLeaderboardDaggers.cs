namespace DevilDaggersInfo.Web.Server.Domain.Models.CustomLeaderboards;

public record CustomLeaderboardDaggers
{
	public required int Bronze { get; init; }

	public required int Silver { get; init; }

	public required int Golden { get; init; }

	public required int Devil { get; init; }

	public required int Leviathan { get; init; }
}
