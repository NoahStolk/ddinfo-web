namespace DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public;

public interface IGetEntryDto
{
	int Rank { get; }

	int Id { get; }

	string Username { get; }

	double Time { get; }

	int Kills { get; }

	int Gems { get; }

	byte DeathType { get; }

	int DaggersHit { get; }

	int DaggersFired { get; }

	double TimeTotal { get; }

	ulong KillsTotal { get; }

	ulong GemsTotal { get; }

	ulong DeathsTotal { get; }

	ulong DaggersHitTotal { get; }

	ulong DaggersFiredTotal { get; }
}
