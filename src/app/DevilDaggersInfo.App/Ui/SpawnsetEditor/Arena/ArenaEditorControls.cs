using DevilDaggersInfo.App.Ui.SpawnsetEditor.Arena.EditorChildren;
using DevilDaggersInfo.App.Ui.SpawnsetEditor.State;
using DevilDaggersInfo.Core.Spawnset;
using ImGuiNET;

namespace DevilDaggersInfo.App.Ui.SpawnsetEditor.Arena;

public static class ArenaEditorControls
{
	public static void Render()
	{
		ImGui.BeginChild("ArenaEditorControls", new(128, 144));

		foreach (ArenaTool arenaTool in Enum.GetValues<ArenaTool>())
		{
			if (arenaTool == ArenaTool.Dagger)
				ImGui.BeginDisabled(SpawnsetState.Spawnset.GameMode != GameMode.Race);

			if (ImGui.RadioButton(arenaTool.ToString(), arenaTool == ArenaChild.ArenaTool) && ArenaChild.ArenaTool != arenaTool)
				ArenaChild.ArenaTool = arenaTool;

			if (arenaTool == ArenaTool.Dagger)
				ImGui.EndDisabled();
		}

		ImGui.EndChild();

		ImGui.SameLine();

		ImGui.BeginChild("ArenaToolControls", new(256, 144));

		switch (ArenaChild.ArenaTool)
		{
			case ArenaTool.Pencil:
				PencilChild.Render();
				break;
			case ArenaTool.Line:
				LineChild.Render();
				break;
			case ArenaTool.Rectangle:
				RectangleChild.Render();
				break;
			case ArenaTool.Ellipse:
				EllipseChild.Render();
				break;
			case ArenaTool.Bucket:
				BucketChild.Render();
				break;
			case ArenaTool.Dagger:
				DaggerChild.Render();
				break;
		}

		ImGui.EndChild();
	}
}
