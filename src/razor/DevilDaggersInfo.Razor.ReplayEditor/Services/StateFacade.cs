using DevilDaggersInfo.App.Core.GameMemory;
using DevilDaggersInfo.App.Core.NativeInterface.Services;
using DevilDaggersInfo.App.Core.NativeInterface.Utils;
using DevilDaggersInfo.Core.Replay;
using DevilDaggersInfo.Core.Replay.Exceptions;
using DevilDaggersInfo.Razor.ReplayEditor.Enums;
using DevilDaggersInfo.Razor.ReplayEditor.Store.Features.ReplayBinaryFeature.Actions;
using DevilDaggersInfo.Razor.ReplayEditor.Store.Features.ReplayEditorFeature.Actions;
using Fluxor;

namespace DevilDaggersInfo.Razor.ReplayEditor.Services;

public class StateFacade
{
	private readonly IDispatcher _dispatcher;
	private readonly INativeFileSystemService _fileSystemService;
	private readonly GameMemoryReaderService _readerService;

	public StateFacade(IDispatcher dispatcher, INativeFileSystemService fileSystemService, GameMemoryReaderService readerService)
	{
		_dispatcher = dispatcher;
		_fileSystemService = fileSystemService;
		_readerService = readerService;
	}

	public void NewReplay()
	{
		_dispatcher.Dispatch(new OpenReplayAction(ReplayBinary<LocalReplayBinaryHeader>.CreateDefault(), "(Untitled)"));

		_dispatcher.Dispatch(new SelectTickRangeAction(0, 60));
	}

	public void OpenReplayFile()
	{
		INativeFileSystemService.FileResult? fileResult = _fileSystemService.OpenFile(FileDialogFilterUtils.BuildFilterComdlg32(new() { ["Devil Daggers local replay"] = "*.ddreplay" }));
		if (fileResult == null)
			return;

		try
		{
			ReplayBinary<LocalReplayBinaryHeader> localReplay = new(fileResult.Contents);
			_dispatcher.Dispatch(new OpenReplayAction(localReplay, Path.GetFileName(fileResult.Path)));
		}
		catch (InvalidReplayBinaryException ex)
		{
			// TODO: Dispatch failure action.
		}

		_dispatcher.Dispatch(new SelectTickRangeAction(0, 60));
	}

	public async Task OpenLeaderboardReplayAsync(int playerId)
	{
		using FormUrlEncodedContent content = new(new List<KeyValuePair<string, string>> { new KeyValuePair<string, string>("replay", playerId.ToString()) });
		using HttpClient httpClient = new();
		using HttpResponseMessage response = await httpClient.PostAsync("http://dd.hasmodai.com/backend16/get_replay.php", content);

		// TODO: Dispatch failure action.
		if (!response.IsSuccessStatusCode)
			throw new($"The leaderboard servers returned an unsuccessful response (HTTP {(int)response.StatusCode} {response.StatusCode}).");

		byte[] responseData = await response.Content.ReadAsByteArrayAsync();

		try
		{
			ReplayBinary<LeaderboardReplayBinaryHeader> leaderboardReplay = new(responseData);
			_dispatcher.Dispatch(new OpenLeaderboardReplayAction(leaderboardReplay, playerId));
		}
		catch (InvalidReplayBinaryException ex)
		{
			// TODO: Dispatch failure action.
		}

		_dispatcher.Dispatch(new SelectTickRangeAction(0, 60));
	}

	public void OpenCurrentReplayInGame()
	{
		const long ddstatsMarkerOffset = 2452928; // TODO: Get from API.
		if (!_readerService.Initialize(ddstatsMarkerOffset))
		{
			// TODO: Dispatch failure action.
			return;
		}

		_readerService.Scan();

		byte[] replayData = _readerService.ReadReplayFromMemory();

		try
		{
			ReplayBinary<LocalReplayBinaryHeader> memoryReplay = new(replayData);
			_dispatcher.Dispatch(new OpenReplayAction(memoryReplay, "Replay from game memory"));
		}
		catch (InvalidReplayBinaryException ex)
		{
			// TODO: Dispatch failure action.
		}

		_dispatcher.Dispatch(new SelectTickRangeAction(0, 60));
	}

	public void SelectTickRange(int startTick, int endTick)
	{
		_dispatcher.Dispatch(new SelectTickRangeAction(startTick, endTick));
	}

	public void ToggleShowTicksWithoutEvents()
	{
		_dispatcher.Dispatch(new ToggleShowTicksWithoutEventsAction());
	}

	public void ToggleShowEventTypes(SwitchableEventType switchableEventType)
	{
		_dispatcher.Dispatch(new ToggleShowEventTypeAction(switchableEventType));
	}

	public void EnableAllEventTypes()
	{
		_dispatcher.Dispatch(new ToggleAllEventTypesAction(true));
	}

	public void DisableAllEventTypes()
	{
		_dispatcher.Dispatch(new ToggleAllEventTypesAction(false));
	}
}
