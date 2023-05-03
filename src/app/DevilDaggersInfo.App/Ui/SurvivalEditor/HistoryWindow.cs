using ImGuiNET;

namespace DevilDaggersInfo.App.Ui.SurvivalEditor;

public static class HistoryWindow
{
	public static void Render()
	{
		ImGui.BeginChild("HistoryChild", new(384, 512));

		foreach (SpawnsetHistoryEntry h in SpawnsetState.History)
		{
			ImGui.PushStyleColor(ImGuiCol.TextSelectedBg, h.EditType.GetColor());
			ImGui.Selectable(h.EditType.GetChange());
			ImGui.PopStyleColor();
		}

		ImGui.EndChild();
	}
}
