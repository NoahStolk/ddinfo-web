namespace DevilDaggersInfo.Web.BlazorWasm.Server.InternalModels.Json;

/// <summary>
/// This class must correspond to what's stored in the leaderboard history JSON.
/// </summary>
public class LeaderboardHistory
{
	public DateTime DateTime { get; init; }

	public int Players { get; set; }

	public ulong TimeGlobal { get; set; }

	public ulong KillsGlobal { get; set; }

	public ulong GemsGlobal { get; set; }

	public ulong DeathsGlobal { get; set; }

	public ulong DaggersHitGlobal { get; set; }

	public ulong DaggersFiredGlobal { get; set; }

	public List<EntryHistory> Entries { get; set; } = new();
}
