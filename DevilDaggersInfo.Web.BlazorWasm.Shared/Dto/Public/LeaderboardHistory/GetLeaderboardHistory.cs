using DevilDaggersInfo.Web.BlazorWasm.Shared.Extensions;
using Newtonsoft.Json;

namespace DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.LeaderboardHistory;

// This class must correspond to what's stored in the leaderboard history JSON.
public class GetLeaderboardHistory : IGetLeaderboardDto<GetEntryHistory>
{
	public DateTime DateTime { get; init; }

	public int Players { get; set; }

	// TODO: Translate all history TimeGlobal to double.
	public ulong TimeGlobal { get; set; }

	public ulong KillsGlobal { get; set; }

	public ulong GemsGlobal { get; set; }

	public ulong DeathsGlobal { get; set; }

	public ulong DaggersHitGlobal { get; set; }

	public ulong DaggersFiredGlobal { get; set; }

	public List<GetEntryHistory> Entries { get; } = new();

	[JsonIgnore]
	public int TotalPlayers => Players;

	[JsonIgnore]
	double IGetLeaderboardGlobalDto.TimeGlobal => TimeGlobal.ToSecondsTime();
}
