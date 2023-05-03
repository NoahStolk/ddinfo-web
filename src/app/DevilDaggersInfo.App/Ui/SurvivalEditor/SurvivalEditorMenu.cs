using DevilDaggersInfo.App.Ui.Base;
using DevilDaggersInfo.App.Ui.Base.StateManagement;
using DevilDaggersInfo.App.Ui.Base.User.Settings;
using DevilDaggersInfo.Core.Spawnset;
using ImGuiNET;

namespace DevilDaggersInfo.App.Ui.SurvivalEditor;

public static class SurvivalEditorMenu
{
	public static void Render()
	{
		if (ImGui.BeginMenuBar())
		{
			if (ImGui.BeginMenu("File"))
			{
				RenderFileMenu();
				ImGui.EndMenu();
			}

			ImGui.EndMenuBar();
		}
	}

	private static void RenderFileMenu()
	{
		if (ImGui.MenuItem("New"))
		{
			SpawnsetState.SpawnsetName = "(untitled)";
			SpawnsetState.Spawnset = SpawnsetBinary.CreateDefault();
			SpawnsetHistoryUtils.Save(SpawnsetEditType.Reset);
		}

		if (ImGui.MenuItem("Open"))
		{
			OpenSpawnset();
			SpawnsetHistoryUtils.Save(SpawnsetEditType.Reset);
		}

		if (ImGui.MenuItem("Open default (V3)"))
		{
			SpawnsetState.SpawnsetName = "V3";
			SpawnsetState.Spawnset = ContentManager.Content.DefaultSpawnset.DeepCopy();
			SpawnsetHistoryUtils.Save(SpawnsetEditType.Reset);
		}

		if (ImGui.MenuItem("Save"))
		{
			string? filePath = Root.NativeFileSystemService.CreateSaveFileDialog("Save spawnset file", null);
			if (filePath != null)
				File.WriteAllBytes(filePath, StateManager.SpawnsetState.Spawnset.ToBytes());
		}

		if (ImGui.MenuItem("Replace"))
		{
			File.WriteAllBytes(UserSettings.ModsSurvivalPath, StateManager.SpawnsetState.Spawnset.ToBytes());
			Modals.ShowReplacedSurvivalFile = true;
		}

		ImGui.Separator();

		if (ImGui.MenuItem("Close"))
			UiRenderer.Layout = LayoutType.Main;
	}

	private static void OpenSpawnset()
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
			Modals.ShowError = true;
			Modals.ErrorText = $"Could not open file '{filePath}'.";
			return;
		}

		if (SpawnsetBinary.TryParse(fileContents, out SpawnsetBinary? spawnsetBinary))
		{
			SpawnsetState.SpawnsetName = Path.GetFileName(filePath);
			SpawnsetState.Spawnset = spawnsetBinary;
		}
		else
		{
			Modals.ShowError = true;
			Modals.ErrorText = $"The file '{filePath}' could not be parsed as a spawnset.";
		}
	}
}
