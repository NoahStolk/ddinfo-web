using DevilDaggersInfo.Core.Versioning;
using ImGuiNET;

namespace DevilDaggersInfo.App.Ui.Global;

public static class UpdateAvailableWindow
{
	public static void Render(ref bool show, AppVersion availableVersionNumber)
	{
		if (!show)
			return;

		ImGui.SetNextWindowSize(new(512, 128));

		ImGui.Begin("Update available", ref show, ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoResize);
		ImGui.Text($"Version {availableVersionNumber} is available. Re-run the launcher to install it.");
		ImGui.End();
	}
}
