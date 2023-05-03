using ImGuiNET;
using System.Numerics;

namespace DevilDaggersInfo.App.Ui.SurvivalEditor;

public static class SurvivalEditorModals
{
	private const string _replacedId = "Successfully replaced current survival file";

	public static bool ShowReplaced { get; set; }

	public static void Render()
	{
		if (ShowReplaced)
		{
			ImGui.OpenPopup(_replacedId);
			ShowReplaced = false;
		}

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
	}
}
