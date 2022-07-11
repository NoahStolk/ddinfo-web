using DevilDaggersInfo.Core.NativeInterface;
using DevilDaggersInfo.Core.NativeInterface.Utils;
using DevilDaggersInfo.Core.Replay;
using DevilDaggersInfo.Core.Replay.Enums;
using DevilDaggersInfo.Razor.ReplayEditor.Store.Features.ReplayEditor.Actions;
using Fluxor;

namespace DevilDaggersInfo.Razor.ReplayEditor.Services;

public class StateFacade
{
	private readonly IDispatcher _dispatcher;
	private readonly INativeFileSystemService _fileSystemService;

	public StateFacade(IDispatcher dispatcher, INativeFileSystemService fileSystemService)
	{
		_dispatcher = dispatcher;
		_fileSystemService = fileSystemService;
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

		_dispatcher.Dispatch(new OpenReplayAction(new(fileResult.Contents, ReplayBinaryReadComprehensiveness.All), Path.GetFileName(fileResult.Path)));
	}

	public void SelectTickRange(int startTick, int endTick)
	{
		if (startTick < 0)
			startTick = 0;

		if (endTick <= startTick)
			endTick = startTick + 1;

		_dispatcher.Dispatch(new SelectTickRangeAction(startTick, endTick));
	}
}
