using DevilDaggersInfo.App.Ui.Base;
using DevilDaggersInfo.App.Utils;
using DevilDaggersInfo.Core.Spawnset;
using ImGuiNET;

namespace DevilDaggersInfo.App.Ui.SurvivalEditor;

public static class SurvivalEditorLayout
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
					SpawnsetFileUtils.ReplaceSpawnset();

				ImGui.Separator();

				if (ImGui.MenuItem("Close"))
					UiRenderer.Layout = LayoutType.Main;

				ImGui.EndMenu();
			}

			ImGui.EndMainMenuBar();
		}

		ImGui.SetNextWindowPos(new(0, 16));
		ImGui.SetNextWindowSize(Constants.LayoutSize);

		ImGui.Begin("Survival Editor", Constants.LayoutFlags);

		SpawnsWindow.Render();

		ImGui.End();
	}

	/*
	public void Update()
	{
		if (!Input.IsCtrlHeld())
			return;

		Keys? key = _keySubmitter.GetKey();
		if (key.HasValue)
		{
			if (key == Keys.Z && StateManager.SpawnsetHistoryState.CurrentIndex > 0)
				StateManager.Dispatch(new SetSpawnsetHistoryIndex(StateManager.SpawnsetHistoryState.CurrentIndex - 1));
			else if (key == Keys.Y && StateManager.SpawnsetHistoryState.CurrentIndex < StateManager.SpawnsetHistoryState.History.Count - 1)
				StateManager.Dispatch(new SetSpawnsetHistoryIndex(StateManager.SpawnsetHistoryState.CurrentIndex + 1));
		}

		if (Input.IsKeyPressed(Keys.N))
			StateManager.Dispatch(new LoadSpawnset("(untitled)", SpawnsetBinary.CreateDefault()));
		else if (Input.IsKeyPressed(Keys.O))
			SpawnsetFileUtils.OpenSpawnset();
		else if (Input.IsKeyPressed(Keys.S))
			SpawnsetFileUtils.SaveSpawnset();
		else if (Input.IsKeyPressed(Keys.R))
			SpawnsetFileUtils.ReplaceSpawnset();
	}
	 */
}
