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
	}
}
