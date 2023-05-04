using DevilDaggersInfo.App.Ui.SurvivalEditor.Arena;
using ImGuiNET;
using System.Numerics;

namespace DevilDaggersInfo.App.Ui.SurvivalEditor;

public static class SurvivalEditorWindow
{
	public static void Render()
	{
		ImGui.PushStyleVar(ImGuiStyleVar.WindowMinSize, new Vector2(1024, 768));
		ImGui.Begin("Survival Editor", ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.MenuBar);
		ImGui.PopStyleVar();

		SurvivalEditorMenu.Render();

		SpawnsChild.Render();

		ImGui.SameLine();

		ArenaChild.Render();

		ImGui.SameLine();

		SettingsChild.Render();

		HistoryChild.Render();

		ImGui.SameLine();

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
