using DevilDaggersInfo.App.Engine.Maths.Numerics;
using DevilDaggersInfo.App.Ui.SurvivalEditor.State;
using ImGuiNET;
using System.Numerics;

namespace DevilDaggersInfo.App.Ui.SurvivalEditor;

public static class HistoryChild
{
	public static void Render()
	{
		ImGui.BeginChild("HistoryChild", new(256, 712));

		foreach (SpawnsetHistoryEntry h in SpawnsetState.History)
		{
			Color color = h.EditType.GetColor();
			ImGui.PushStyleColor(ImGuiCol.Button, color);
			ImGui.PushStyleColor(ImGuiCol.ButtonHovered, color + new Vector4(0.3f, 0.3f, 0.3f, 0));
			ImGui.PushStyleColor(ImGuiCol.ButtonActive, color + new Vector4(0.5f, 0.5f, 0.5f, 0));

			ImGui.Button(h.EditType.GetChange(), new(240, 20));

			ImGui.PopStyleColor();
			ImGui.PopStyleColor();
			ImGui.PopStyleColor();
		}

		ImGui.EndChild();
	}
}
