using DevilDaggersInfo.Api.Ddcl;
using DevilDaggersInfo.Api.Ddcl.CustomLeaderboards;
using DevilDaggersInfo.Razor.CustomLeaderboard.Services;
using DevilDaggersInfo.Razor.CustomLeaderboard.Store.Features.LeaderboardListFeature.Actions;
using DevilDaggersInfo.Razor.CustomLeaderboard.Store.State;
using Fluxor;

namespace DevilDaggersInfo.Razor.CustomLeaderboard.Store.Features.LeaderboardListFeature.Effects;

public class FetchLeaderboardsEffect : Effect<FetchLeaderboardsAction>
{
	private readonly NetworkService _networkService;
	private readonly IState<LeaderboardListState> _leaderboardListState;

	public FetchLeaderboardsEffect(NetworkService networkService, IState<LeaderboardListState> leaderboardListState)
	{
		_networkService = networkService;
		_leaderboardListState = leaderboardListState;
	}

	public override async Task HandleAsync(FetchLeaderboardsAction action, IDispatcher dispatcher)
	{
		try
		{
			Page<GetCustomLeaderboardForOverview> leaderboards = await _networkService.GetLeaderboardOverview(_leaderboardListState.Value.Category, _leaderboardListState.Value.PageIndex, _leaderboardListState.Value.PageSize, _leaderboardListState.Value.SelectedPlayerId, false);
			dispatcher.Dispatch(new FetchLeaderboardsSuccessAction(leaderboards));
		}
		catch (Exception ex)
		{
			dispatcher.Dispatch(new FetchLeaderboardsFailureAction(ex.Message));
		}
	}
}
