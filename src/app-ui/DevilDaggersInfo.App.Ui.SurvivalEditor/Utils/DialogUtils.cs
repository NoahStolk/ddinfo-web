using DevilDaggersInfo.App.Core.NativeInterface.Services;
using DevilDaggersInfo.App.Ui.Base.DependencyPattern;
using DevilDaggersInfo.App.Ui.Base.StateManagement;
using DevilDaggersInfo.App.Ui.Base.StateManagement.Base.Actions;
using DevilDaggersInfo.App.Ui.Base.StateManagement.SurvivalEditor.Actions;
using DevilDaggersInfo.Core.Spawnset;

namespace DevilDaggersInfo.App.Ui.SurvivalEditor.Utils;

public static class DialogUtils
{
	public static void OpenSpawnset()
	{
		INativeFileSystemService.FileResult? fileResult = Root.Dependencies.NativeFileSystemService.OpenFile(null);
		if (fileResult == null)
			return;

		string filePath = fileResult.Path;
		byte[] bytes = File.ReadAllBytes(filePath);
		if (SpawnsetBinary.TryParse(bytes, out SpawnsetBinary? spawnsetBinary))
		{
			StateManager.Dispatch(new LoadSpawnset(Path.GetFileName(filePath), spawnsetBinary));
			StateManager.Dispatch(new SetLayout(Root.Dependencies.SurvivalEditorMainLayout));
		}
		else
		{
			Root.Dependencies.NativeDialogService.ReportError("Failed to parse", "The file could not be parsed as a spawnset.");
		}
	}
}
