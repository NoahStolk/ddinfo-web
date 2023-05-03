using DevilDaggersInfo.App.Ui.Base;
using DevilDaggersInfo.App.Ui.Base.StateManagement;
using DevilDaggersInfo.App.Ui.Base.User.Settings;
using DevilDaggersInfo.Core.Spawnset;
using ImGuiNET;
using System.Numerics;

namespace DevilDaggersInfo.App.Ui.SurvivalEditor;

public static class SurvivalEditorLayout
{
	private const string _replacedId = "Successfully replaced current survival file";

	public static void Render()
	{
		bool showReplaced = false;
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
					showReplaced = true;
				}

				ImGui.Separator();

				if (ImGui.MenuItem("Close"))
					UiRenderer.Layout = LayoutType.Main;

				ImGui.EndMenu();
			}

			ImGui.EndMainMenuBar();
		}

		if (showReplaced)
			ImGui.OpenPopup(_replacedId);

		Vector2 center = ImGui.GetMainViewport().GetCenter();
		ImGui.SetNextWindowPos(center, ImGuiCond.Always, new(0.5f, 0.5f));
		ImGui.SetNextWindowSize(new(512, 128));
		if (ImGui.BeginPopupModal(_replacedId))
		{
			ImGui.Text("The current survival file has been replaced with the current spawnset.");

			if (ImGui.Button("OK", new(120, 0)))
				ImGui.CloseCurrentPopup();

			ImGui.EndPopup();
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
