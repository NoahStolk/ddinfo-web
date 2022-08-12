using DevilDaggersInfo.Core.Spawnset;

namespace DevilDaggersInfo.Razor.CustomLeaderboard.Store.State;

public record SpawnsetState(
	bool IsLoading,
	string? Error,
	SpawnsetBinary? Spawnset);
