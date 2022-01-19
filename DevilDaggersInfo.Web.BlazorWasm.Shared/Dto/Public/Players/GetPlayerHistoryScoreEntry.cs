namespace DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.Players;

public class GetPlayerHistoryScoreEntry
{
	public DateTime DateTime { get; init; }

	public int Rank { get; init; }

	public string Username { get; init; } = null!;

	public double Time { get; init; }

	public int Kills { get; init; }

	public int Gems { get; init; }

	public byte DeathType { get; init; }

	public int DaggersHit { get; init; }

	public int DaggersFired { get; init; }
}
