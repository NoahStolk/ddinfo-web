using DevilDaggersInfo.App.Core.GameMemory;
using DevilDaggersInfo.App.Core.NativeInterface.Services;
using DevilDaggersInfo.App.Core.NativeInterface.Utils;
using DevilDaggersInfo.Core.Replay;
using DevilDaggersInfo.Core.Replay.Exceptions;
using DevilDaggersInfo.Razor.ReplayEditor.Enums;
using DevilDaggersInfo.Razor.ReplayEditor.Store.Features.LeaderboardBrowserFeature.Actions;
using DevilDaggersInfo.Razor.ReplayEditor.Store.Features.ReplayBinaryFeature.Actions;
using DevilDaggersInfo.Razor.ReplayEditor.Store.Features.ReplayEditorFeature.Actions;
using Fluxor;

namespace DevilDaggersInfo.Razor.ReplayEditor.Services;

public class StateFacade
{
	private readonly IDispatcher _dispatcher;
	private readonly INativeFileSystemService _fileSystemService;
	private readonly GameMemoryService _gameMemoryService;
	private readonly NetworkService _networkService;
	private readonly INativeErrorReporter _errorReporter;

	public StateFacade(IDispatcher dispatcher, INativeFileSystemService fileSystemService, GameMemoryService gameMemoryService, NetworkService networkService, INativeErrorReporter errorReporter)
	{
		_dispatcher = dispatcher;
		_fileSystemService = fileSystemService;
		_gameMemoryService = gameMemoryService;
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
			_dispatcher.Dispatch(new OpenReplayAction(localReplay, Path.GetFileNameWithoutExtension(fileResult.Path)));
		}
		catch (InvalidReplayBinaryException ex)
		{
			_errorReporter.ReportError("Could not parse local replay", "The local replay could not be parsed.", ex);
		}

		_dispatcher.Dispatch(new SelectTickRangeAction(0, 60));
	}

	public void DownloadLeaderboardReplay(int playerId)
	{
		_dispatcher.Dispatch(new DownloadLeaderboardReplayAction(playerId));
	}

	public async Task OpenCurrentReplayInGame()
	{
		long ddstatsMarkerOffset;
		try
		{
			ddstatsMarkerOffset = await _networkService.GetMarkerAsync(Api.Ddre.ProcessMemory.SupportedOperatingSystem.Windows); // TODO: Use Linux on Linux.
		}
		catch (Exception ex)
		{
			_errorReporter.ReportError("Could not fetch marker", "Could not fetch marker from the DevilDaggers.info API.", ex);
			return;
		}

		if (!_gameMemoryService.Initialize(ddstatsMarkerOffset))
		{
			_errorReporter.ReportError("Could not initialize game memory", "Make sure the game is open.");
			return;
		}

		_gameMemoryService.Scan();

		byte[] replayData = _gameMemoryService.ReadReplayFromMemory();

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
