using ImGuiNET;

namespace DevilDaggersInfo.App.Ui.SurvivalEditor;

public static class SurvivalEditorLayout
{
	public static void Render()
	{
		SurvivalEditorMenu.Render();
		SurvivalEditorModals.Render();

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
