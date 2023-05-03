using DevilDaggersInfo.Core.Spawnset;
using ImGuiNET;

namespace DevilDaggersInfo.App.Ui.SurvivalEditor;

public static class SpawnsWindow
{
	public static void Render()
	{
		ImGui.Begin("Spawns");

		ImGui.Text(SpawnsetState.SpawnsetName);
		ImGui.Text(SpawnsetState.Spawnset.Spawns.Length.ToString());
		foreach (Spawn spawn in SpawnsetState.Spawnset.Spawns)
		{
			ImGui.Text(spawn.ToString());
		}

		ImGui.End();
	}
}
