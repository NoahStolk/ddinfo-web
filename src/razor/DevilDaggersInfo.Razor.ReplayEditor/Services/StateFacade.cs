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
	private readonly NetworkService _networkService;
	private readonly INativeErrorReporter _errorReporter;

	public StateFacade(IDispatcher dispatcher, INativeFileSystemService fileSystemService, GameMemoryReaderService readerService, NetworkService networkService, INativeErrorReporter errorReporter)
	{
		_dispatcher = dispatcher;
		_fileSystemService = fileSystemService;
		_readerService = readerService;
		_networkService = networkService;
		_errorReporter = errorReporter;
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
			_errorReporter.ReportError("Could not parse local replay", "The local replay could not be parsed.", ex);
		}

		_dispatcher.Dispatch(new SelectTickRangeAction(0, 60));
	}

	public async Task OpenLeaderboardReplayAsync(int playerId)
	{
		using FormUrlEncodedContent content = new(new List<KeyValuePair<string, string>> { new KeyValuePair<string, string>("replay", playerId.ToString()) });
		using HttpClient httpClient = new();
		using HttpResponseMessage response = await httpClient.PostAsync("http://dd.hasmodai.com/backend16/get_replay.php", content);

		if (!response.IsSuccessStatusCode)
		{
			_errorReporter.ReportError("Could not fetch leaderboard replay", $"The leaderboard servers returned an unsuccessful response (HTTP {(int)response.StatusCode} {response.StatusCode}).");
			return;
		}

		byte[] responseData = await response.Content.ReadAsByteArrayAsync();

		try
		{
			ReplayBinary<LeaderboardReplayBinaryHeader> leaderboardReplay = new(responseData);
			_dispatcher.Dispatch(new OpenLeaderboardReplayAction(leaderboardReplay, playerId));
		}
		catch (InvalidReplayBinaryException ex)
		{
			_errorReporter.ReportError("Could not parse leaderboard replay", "The leaderboard replay could not be parsed.", ex);
		}

		_dispatcher.Dispatch(new SelectTickRangeAction(0, 60));
	}

	public async Task OpenCurrentReplayInGame()
	{
		long ddstatsMarkerOffset;
		try
		{
			// TODO: Cache this value.
			ddstatsMarkerOffset = await _networkService.GetMarker(Api.Ddre.ProcessMemory.SupportedOperatingSystem.Windows); // TODO: Use Linux on Linux.
		}
		catch (Exception ex)
		{
			_errorReporter.ReportError("Could not fetch marker", "Could not fetch marker from the DevilDaggers.info API.", ex);
			return;
		}

		if (!_readerService.Initialize(ddstatsMarkerOffset))
		{
			_errorReporter.ReportError("Could not initialize game memory", "Make sure the game is open.");
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
			_errorReporter.ReportError("Could not parse replay from game memory", "The replay from game memory could not be parsed.", ex);
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
