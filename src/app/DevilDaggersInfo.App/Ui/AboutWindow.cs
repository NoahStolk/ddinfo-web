using ImGuiNET;

namespace DevilDaggersInfo.App.Ui;

public static class AboutWindow
{
	public static void Render(ref bool show)
	{
		if (!show)
			return;

		ImGui.SetNextWindowSize(new(512, 384));
		if (ImGui.Begin("About ddinfo tools", ref show, ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoResize))
		{
			ImGui.Text(StringResources.AppDescription);
		}

		ImGui.End();
	}
}
