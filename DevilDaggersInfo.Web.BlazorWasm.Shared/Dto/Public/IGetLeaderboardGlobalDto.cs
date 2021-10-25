namespace DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public;

public interface IGetLeaderboardGlobalDto
{
	DateTime DateTime { get; }

	int TotalPlayers { get; }

	double TimeGlobal { get; }

	ulong KillsGlobal { get; }

	ulong GemsGlobal { get; }

	ulong DeathsGlobal { get; }

	ulong DaggersHitGlobal { get; }

	ulong DaggersFiredGlobal { get; }
}
