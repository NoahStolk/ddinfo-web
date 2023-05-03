using ImGuiNET;

namespace DevilDaggersInfo.App.Ui.Global;

public static class ErrorWindow
{
	public static void Render(ref bool show, string error)
	{
		if (!show)
			return;

		ImGui.SetNextWindowSize(new(512, 128));

		ImGui.Begin("Error", ref show, ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoResize);
		ImGui.Text(error);
		ImGui.End();
	}
}
