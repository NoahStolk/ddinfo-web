using DevilDaggersInfo.Core.Versioning;
using ImGuiNET;

namespace DevilDaggersInfo.App.Ui.Global;

public static class UpdateAvailableWindow
{
	public static void Render(ref bool show, UpdateAvailable updateAvailable)
	{
		if (!show)
			return;

		ImGui.SetNextWindowSize(new(512, 128));

		ImGui.Begin("Update available", ref show, ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoResize);
		ImGui.Text($"Version {updateAvailable.AvailableVersionNumber} is available. Re-run the launcher to install it.");
		ImGui.End();
	}

	public record UpdateAvailable(AppVersion AvailableVersionNumber);
}
