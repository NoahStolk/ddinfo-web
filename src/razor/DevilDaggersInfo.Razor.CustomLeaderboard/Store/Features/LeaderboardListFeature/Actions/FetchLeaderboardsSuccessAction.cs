using DevilDaggersInfo.Api.Ddcl;
using DevilDaggersInfo.Api.Ddcl.CustomLeaderboards;

namespace DevilDaggersInfo.Razor.CustomLeaderboard.Store.Features.LeaderboardListFeature.Actions;

public record FetchLeaderboardsSuccessAction(Page<GetCustomLeaderboardForOverview> Leaderboards);
