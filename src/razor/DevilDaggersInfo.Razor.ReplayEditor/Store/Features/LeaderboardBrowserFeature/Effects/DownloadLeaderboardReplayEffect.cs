using DevilDaggersInfo.App.Core.NativeInterface.Services;
using DevilDaggersInfo.Core.Replay;
using DevilDaggersInfo.Core.Replay.Exceptions;
using DevilDaggersInfo.Razor.ReplayEditor.Store.Features.LeaderboardBrowserFeature.Actions;
using DevilDaggersInfo.Razor.ReplayEditor.Store.Features.ReplayBinaryFeature.Actions;
using DevilDaggersInfo.Razor.ReplayEditor.Store.Features.ReplayEditorFeature.Actions;
using Fluxor;
using Microsoft.AspNetCore.Components;

namespace DevilDaggersInfo.Razor.ReplayEditor.Store.Features.LeaderboardBrowserFeature.Effects;

public class DownloadLeaderboardReplayEffect : Effect<DownloadLeaderboardReplayAction>
{
	private readonly INativeErrorReporter _errorReporter;
	private readonly NavigationManager _navigationManager;

	public DownloadLeaderboardReplayEffect(INativeErrorReporter errorReporter, NavigationManager navigationManager)
	{
		_errorReporter = errorReporter;
		_navigationManager = navigationManager;
	}

	public override async Task HandleAsync(DownloadLeaderboardReplayAction action, IDispatcher dispatcher)
	{
		using FormUrlEncodedContent content = new(new List<KeyValuePair<string, string>> { new KeyValuePair<string, string>("replay", action.PlayerId.ToString()) });
		using HttpClient httpClient = new();
		using HttpResponseMessage response = await httpClient.PostAsync("http://dd.hasmodai.com/backend16/get_replay.php", content);

		if (!response.IsSuccessStatusCode)
		{
			_errorReporter.ReportError("Could not fetch leaderboard replay", $"The leaderboard servers returned an unsuccessful response (HTTP {(int)response.StatusCode} {response.StatusCode}).");
			dispatcher.Dispatch(new DownloadLeaderboardReplayFailureAction());
			return;
		}

		byte[] responseData = await response.Content.ReadAsByteArrayAsync();

		try
		{
			ReplayBinary<LeaderboardReplayBinaryHeader> leaderboardReplay = new(responseData);
			dispatcher.Dispatch(new OpenLeaderboardReplayAction(leaderboardReplay, action.PlayerId));
			dispatcher.Dispatch(new SelectTickRangeAction(0, 60));
			dispatcher.Dispatch(new DownloadLeaderboardReplaySuccessAction());

			_navigationManager.NavigateTo("/");
		}
		catch (InvalidReplayBinaryException ex)
		{
			_errorReporter.ReportError("Could not parse leaderboard replay", "The leaderboard replay could not be parsed.", ex);
			dispatcher.Dispatch(new DownloadLeaderboardReplayFailureAction());
		}
	}
}
