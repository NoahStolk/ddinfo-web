using DevilDaggersInfo.Api.Ddcl.CustomLeaderboards;
using DevilDaggersInfo.Core.Spawnset;

namespace DevilDaggersInfo.Razor.CustomLeaderboard.Store.State;

public record LeaderboardState(
	bool IsLoading,
	string? Error,
	GetCustomLeaderboard? Leaderboard,
	SpawnsetBinary? Spawnset);
