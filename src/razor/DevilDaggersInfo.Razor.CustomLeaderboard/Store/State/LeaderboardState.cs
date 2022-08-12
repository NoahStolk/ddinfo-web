using DevilDaggersInfo.Api.Ddcl.CustomLeaderboards;

namespace DevilDaggersInfo.Razor.CustomLeaderboard.Store.State;

public record LeaderboardState(
	bool IsLoading,
	string? Error,
	GetCustomLeaderboard? Leaderboard);
