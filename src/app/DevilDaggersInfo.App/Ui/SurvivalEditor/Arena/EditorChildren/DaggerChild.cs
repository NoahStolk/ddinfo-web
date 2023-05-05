using ImGuiNET;
using System.Numerics;

namespace DevilDaggersInfo.App.Ui.SurvivalEditor.Arena.EditorChildren;

public static class DaggerChild
{
	private static Vector2 _snap = Vector2.One;

	public static Vector2 Snap => _snap;

	public static void Render()
	{
		ImGui.SliderFloat2("Snap", ref _snap, 0.25f, 2, "%.2f");
	}
}
