using DevilDaggersInfo.App.Ui.SurvivalEditor.Arena.EditorChildren;
using DevilDaggersInfo.App.Ui.SurvivalEditor.State;
using DevilDaggersInfo.Core.Spawnset;
using ImGuiNET;

namespace DevilDaggersInfo.App.Ui.SurvivalEditor.Arena;

public static class ArenaEditorControls
{
	public static void Render()
	{
		foreach (ArenaTool arenaTool in Enum.GetValues<ArenaTool>())
		{
			if (arenaTool == ArenaTool.Dagger)
				ImGui.BeginDisabled(SpawnsetState.Spawnset.GameMode != GameMode.Race);

			if (ImGui.RadioButton(arenaTool.ToString(), arenaTool == ArenaChild.ArenaTool) && ArenaChild.ArenaTool != arenaTool)
				ArenaChild.ArenaTool = arenaTool;

			if (arenaTool == ArenaTool.Dagger)
				ImGui.EndDisabled();
		}

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
	}
}
