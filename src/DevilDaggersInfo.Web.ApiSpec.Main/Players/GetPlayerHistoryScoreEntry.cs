namespace DevilDaggersInfo.Web.ApiSpec.Main.Players;

public record GetPlayerHistoryScoreEntry
{
	public required DateTime DateTime { get; init; }

	public required int Rank { get; init; }

	public required string Username { get; init; }

	public required double Time { get; init; }

	public required int Kills { get; init; }

	public required int Gems { get; init; }

	public required byte DeathType { get; init; }

	public required int DaggersHit { get; init; }

	public required int DaggersFired { get; init; }
}
