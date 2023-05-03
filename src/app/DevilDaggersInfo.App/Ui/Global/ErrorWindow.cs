using ImGuiNET;

namespace DevilDaggersInfo.App.Ui.Global;

public static class ErrorWindow
{
	public static void Render(ref bool show, Error error)
	{
		if (!show)
			return;

		ImGui.Begin("Error", ref show, ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoResize);
		ImGui.Text(error.Text);
		ImGui.End();
	}

	public record Error(string Text);
}
