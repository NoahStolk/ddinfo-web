using DevilDaggersInfo.App.Ui.Base.StateManagement;
using DevilDaggersInfo.App.Ui.Base.StateManagement.SurvivalEditor.Actions;
using DevilDaggersInfo.App.Ui.Base.User.Settings;
using DevilDaggersInfo.Core.Spawnset;

namespace DevilDaggersInfo.App.Utils;

public static class SpawnsetFileUtils
{
	public static void OpenSpawnset()
	{
		string? filePath = Root.NativeFileSystemService.CreateOpenFileDialog("Open spawnset file", null);
		if (filePath == null)
			return;

		byte[] fileContents;
		try
		{
			fileContents = File.ReadAllBytes(filePath);
		}
		catch (Exception ex)
		{
			// TODO: Log exception.
			UiRenderer.Error = new($"Could not open file '{filePath}'.");
			return;
		}

		if (SpawnsetBinary.TryParse(fileContents, out SpawnsetBinary? spawnsetBinary))
		{
			StateManager.Dispatch(new LoadSpawnset(Path.GetFileName(filePath), spawnsetBinary));
		}
		else
		{
			UiRenderer.Error = new($"The file '{filePath}' could not be parsed as a spawnset.");
		}
	}

	public static void SaveSpawnset()
	{
		string? filePath = Root.NativeFileSystemService.CreateSaveFileDialog("Save spawnset file", null);
		if (filePath == null)
			return;

		if (Directory.Exists(filePath))
		{
			UiRenderer.Error = new("Specified file path is an existing directory. Please specify a file path.");
			return;
		}

		if (File.Exists(filePath))
		{
			// bool? result = Root.NativeDialogService.PromptYesNo("File already exists", "The specified file path already exists. Do you want to overwrite it?");
			// if (result is null or false)
			// 	return;
		}

		File.WriteAllBytes(filePath, StateManager.SpawnsetState.Spawnset.ToBytes());
	}

	public static void ReplaceSpawnset()
	{
		File.WriteAllBytes(UserSettings.ModsSurvivalPath, StateManager.SpawnsetState.Spawnset.ToBytes());
		UiRenderer.Message = new("Successfully replaced current survival file", "The current survival file has been replaced with the current spawnset.");
	}
}
