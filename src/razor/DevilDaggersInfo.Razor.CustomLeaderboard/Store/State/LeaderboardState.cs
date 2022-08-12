using DevilDaggersInfo.Api.Ddcl.CustomLeaderboards;

namespace DevilDaggersInfo.Razor.CustomLeaderboard.Store.State;

public record LeaderboardState(
	string? Error,
	GetCustomLeaderboard? Leaderboard);
