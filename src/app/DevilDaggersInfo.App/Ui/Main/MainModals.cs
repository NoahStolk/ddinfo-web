using DevilDaggersInfo.Core.Versioning;
using ImGuiNET;
using System.Numerics;

namespace DevilDaggersInfo.App.Ui.Main;

public static class MainModals
{
	private const string _updateId = "Update available";

	public static bool ShowUpdate { get; set; }
	public static AppVersion? AvailableVersion { get; set; }

	public static void Render()
	{
		if (ShowUpdate)
		{
			ImGui.OpenPopup(_updateId);
			ShowUpdate = false;
		}

		Vector2 center = ImGui.GetMainViewport().GetCenter();
		ImGui.SetNextWindowPos(center, ImGuiCond.Always, new(0.5f, 0.5f));
		ImGui.SetNextWindowSize(new(512, 128));
		if (ImGui.BeginPopupModal(_updateId))
		{
			ImGui.Text($"Version {AvailableVersion} is available. Re-run the launcher to install it.");

			if (ImGui.Button("OK", new(120, 0)))
				ImGui.CloseCurrentPopup();

			ImGui.EndPopup();
		}
	}
}
