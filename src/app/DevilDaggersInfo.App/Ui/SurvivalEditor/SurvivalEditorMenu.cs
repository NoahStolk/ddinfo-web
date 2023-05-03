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
		if (ImGui.BeginMainMenuBar())
		{
			if (ImGui.BeginMenu("File"))
			{
				if (ImGui.MenuItem("New"))
				{
					SpawnsetState.SpawnsetName = "(untitled)";
					SpawnsetState.Spawnset = SpawnsetBinary.CreateDefault();
				}

				if (ImGui.MenuItem("Open"))
					SpawnsetFileUtils.OpenSpawnset();

				if (ImGui.MenuItem("Open default (V3)"))
				{
					SpawnsetState.SpawnsetName = "V3";
					SpawnsetState.Spawnset = ContentManager.Content.DefaultSpawnset.DeepCopy();
				}

				if (ImGui.MenuItem("Save"))
					SpawnsetFileUtils.SaveSpawnset();

				if (ImGui.MenuItem("Replace"))
				{
					File.WriteAllBytes(UserSettings.ModsSurvivalPath, StateManager.SpawnsetState.Spawnset.ToBytes());
					SurvivalEditorModals.ShowReplaced = true;
				}

				ImGui.Separator();

				if (ImGui.MenuItem("Close"))
					UiRenderer.Layout = LayoutType.Main;

				ImGui.EndMenu();
			}

			ImGui.EndMainMenuBar();
		}
	}
}
