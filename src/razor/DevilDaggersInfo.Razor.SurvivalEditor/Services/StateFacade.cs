using DevilDaggersInfo.App.Core.NativeInterface.Services;
using DevilDaggersInfo.App.Core.NativeInterface.Utils;
using DevilDaggersInfo.Core.Spawnset;
using DevilDaggersInfo.Razor.SurvivalEditor.Store.Features.SpawnsetBinaryFeature.Actions;
using DevilDaggersInfo.Razor.SurvivalEditor.Store.Features.SpawnsListFeature.Actions;
using DevilDaggersInfo.Types.Core.Wiki;
using Fluxor;

namespace DevilDaggersInfo.Razor.SurvivalEditor.Services;

public class StateFacade
{
	private readonly IDispatcher _dispatcher;
	private readonly INativeFileSystemService _fileSystemService;
	private readonly NetworkService _networkService;
	private readonly INativeErrorReporter _errorReporter;

	public StateFacade(IDispatcher dispatcher, INativeFileSystemService fileSystemService, NetworkService networkService, INativeErrorReporter errorReporter)
	{
		_dispatcher = dispatcher;
		_fileSystemService = fileSystemService;
		_networkService = networkService;
		_errorReporter = errorReporter;
	}

	public void NewSpawnset()
	{
		_dispatcher.Dispatch(new OpenSpawnsetAction(SpawnsetBinary.CreateDefault(), "(new spawnset)"));
	}

	public void OpenSpawnsetFile()
	{
		INativeFileSystemService.FileResult? fileResult = _fileSystemService.OpenFile(FileDialogFilterUtils.BuildFilterComdlg32(new()
		{
			["Devil Daggers survival file"] = "*.*",
		}));
		if (fileResult == null)
			return;

		if (SpawnsetBinary.TryParse(fileResult.Contents, out SpawnsetBinary? spawnsetBinary))
		{
			_dispatcher.Dispatch(new OpenSpawnsetAction(spawnsetBinary, Path.GetFileNameWithoutExtension(fileResult.Path)));
			_dispatcher.Dispatch(new SetSpawnsListAction(new(spawnsetBinary, GameVersion.V3_2)));
		}
		else
		{
			_errorReporter.ReportError("Could not parse spawnset", "The spawnset could not be parsed.");
		}
	}
}
