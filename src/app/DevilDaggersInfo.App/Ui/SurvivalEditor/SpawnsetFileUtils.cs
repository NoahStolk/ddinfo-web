using DevilDaggersInfo.App.Ui.Base.StateManagement;
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
			UiRenderer.Error = new($"Could not open file '{filePath}'.");
			return;
		}

		if (SpawnsetBinary.TryParse(fileContents, out SpawnsetBinary? spawnsetBinary))
		{
			SpawnsetState.SpawnsetName = Path.GetFileName(filePath);
			SpawnsetState.Spawnset = spawnsetBinary;
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

		File.WriteAllBytes(filePath, StateManager.SpawnsetState.Spawnset.ToBytes());
	}
}
