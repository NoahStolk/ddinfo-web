using DevilDaggersInfo.App.Ui.SurvivalEditor.Arena.EditorChildren;
using ImGuiNET;

namespace DevilDaggersInfo.App.Ui.SurvivalEditor.Arena;

public static class ArenaEditorControls
{
	public static void Render()
	{
		foreach (ArenaTool arenaTool in Enum.GetValues<ArenaTool>())
		{
			if (ImGui.RadioButton(arenaTool.ToString(), arenaTool == ArenaChild.ArenaTool) && ArenaChild.ArenaTool != arenaTool)
				ArenaChild.ArenaTool = arenaTool;
		}

		switch (ArenaChild.ArenaTool)
		{
			case ArenaTool.Bucket:
				BucketChild.Render();
				break;
			case ArenaTool.Rectangle:
				RectangleChild.Render();
				break;
		}
	}
}
