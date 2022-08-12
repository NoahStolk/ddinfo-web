using DevilDaggersInfo.Api.Ddcl;
using DevilDaggersInfo.Api.Ddcl.CustomLeaderboards;
using DevilDaggersInfo.Types.Web;

namespace DevilDaggersInfo.Razor.CustomLeaderboard.Store.State;

public record LeaderboardListState(
	bool IsLoading,
	string? Error,
	Page<GetCustomLeaderboardForOverview>? Leaderboards,
	int SelectedPlayerId,
	CustomLeaderboardCategory Category,
	int PageIndex,
	int PageSize);
