using DevilDaggersInfo.App.Ui.Base.StateManagement;
using DevilDaggersInfo.App.Ui.Main;
using DevilDaggersInfo.Core.Spawnset;

namespace DevilDaggersInfo.App.Ui.SurvivalEditor;

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
			GlobalModals.ShowError = true;
			GlobalModals.ErrorText = $"Could not open file '{filePath}'.";
			return;
		}

		if (SpawnsetBinary.TryParse(fileContents, out SpawnsetBinary? spawnsetBinary))
		{
			SpawnsetState.SpawnsetName = Path.GetFileName(filePath);
			SpawnsetState.Spawnset = spawnsetBinary;
		}
		else
		{
			GlobalModals.ShowError = true;
			GlobalModals.ErrorText = $"The file '{filePath}' could not be parsed as a spawnset.";
		}
	}

	public static void SaveSpawnset()
	{
		string? filePath = Root.NativeFileSystemService.CreateSaveFileDialog("Save spawnset file", null);
		if (filePath == null)
			return;

		File.WriteAllBytes(filePath, StateManager.SpawnsetState.Spawnset.ToBytes());
	}
}
