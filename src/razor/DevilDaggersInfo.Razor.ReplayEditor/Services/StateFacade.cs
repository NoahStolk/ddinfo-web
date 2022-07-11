using DevilDaggersInfo.Core.NativeInterface;
using DevilDaggersInfo.Core.NativeInterface.Utils;
using DevilDaggersInfo.Core.Replay;
using DevilDaggersInfo.Razor.ReplayEditor.Enums;
using DevilDaggersInfo.Razor.ReplayEditor.Store.Features.ReplayBinaryFeature.Actions;
using DevilDaggersInfo.Razor.ReplayEditor.Store.Features.ReplayEditorFeature.Actions;
using Fluxor;
using System.Diagnostics;

namespace DevilDaggersInfo.Razor.ReplayEditor.Services;

public class StateFacade
{
	private readonly IDispatcher _dispatcher;
	private readonly INativeFileSystemService _fileSystemService;
	private readonly INativeMemoryService _nativeMemoryService;

	public StateFacade(IDispatcher dispatcher, INativeFileSystemService fileSystemService, INativeMemoryService nativeMemoryService)
	{
		_dispatcher = dispatcher;
		_fileSystemService = fileSystemService;
		_nativeMemoryService = nativeMemoryService;
	}

	public void NewReplay()
	{
		_dispatcher.Dispatch(new OpenReplayAction(ReplayBinary.CreateDefault(), "(Untitled)"));
	}

	public void OpenReplayFile()
	{
		INativeFileSystemService.FileResult? fileResult = _fileSystemService.OpenFile(FileDialogFilterUtils.BuildFilterComdlg32(new() { ["Devil Daggers local replay"] = "*.ddreplay" }));
		if (fileResult == null)
			return;

		_dispatcher.Dispatch(new OpenReplayAction(new(fileResult.Contents), Path.GetFileName(fileResult.Path)));
	}

	// TODO: Move to NativeInterface project.
	public void OpenCurrentReplayInGame()
	{
		const long ddstatsMarkerOffset = 2452928; // TODO: Get from API.

		Process? process = _nativeMemoryService.GetDevilDaggersProcess();
		if (process == null || process.MainModule == null)
			return;

		byte[] pointerBytes = new byte[sizeof(long)];
		_nativeMemoryService.ReadMemory(process, process.MainModule.BaseAddress.ToInt64() + ddstatsMarkerOffset, pointerBytes, 0, sizeof(long));
		long ddstatsAddress = BitConverter.ToInt64(pointerBytes);

		byte[] replayPointerBytes = new byte[sizeof(long)];
		_nativeMemoryService.ReadMemory(process, ddstatsAddress + 304, replayPointerBytes, 0, sizeof(long));
		long replayAddress = BitConverter.ToInt64(replayPointerBytes);

		byte[] replayBufferLengthBytes = new byte[sizeof(int)];
		_nativeMemoryService.ReadMemory(process, ddstatsAddress + 312, replayBufferLengthBytes, 0, sizeof(int));
		int replayBufferLength = BitConverter.ToInt32(replayBufferLengthBytes);

		byte[] replayData = new byte[replayBufferLength];
		_nativeMemoryService.ReadMemory(process, replayAddress, replayData, 0, replayBufferLength);

		_dispatcher.Dispatch(new OpenReplayAction(new(replayData), "Replay from game memory"));
	}

	public void SelectTickRange(int startTick, int endTick)
	{
		_dispatcher.Dispatch(new SelectTickRangeAction(startTick, endTick));
	}

	public void ToggleTick(int tick)
	{
		_dispatcher.Dispatch(new ToggleTickAction(tick));
	}

	public void ToggleShowTicksWithoutEvents()
	{
		_dispatcher.Dispatch(new ToggleShowTicksWithoutEventsAction());
	}

	public void ToggleShowEventTypes(SwitchableEventType switchableEventType)
	{
		_dispatcher.Dispatch(new ToggleShowEventTypeAction(switchableEventType));
	}

	public void ShowAllEvents()
	{
		_dispatcher.Dispatch(new ToggleAllEventsAction(true));
	}

	public void HideAllEvents()
	{
		_dispatcher.Dispatch(new ToggleAllEventsAction(false));
	}
}
