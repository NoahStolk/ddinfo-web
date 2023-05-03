using DevilDaggersInfo.Core.Versioning;
using ImGuiNET;
using System.Numerics;

namespace DevilDaggersInfo.App.Ui;

// TODO: Remove duplicate code.
public static class GlobalModals
{
	private const string _updateId = "Update available";
	private const string _errorId = "Error";

	public static bool ShowUpdate { get; set; }
	public static AppVersion? AvailableVersion { get; set; }

	public static bool ShowError { get; set; }
	public static string? ErrorText { get; set; }

	public static void Render()
	{
		ShowUpdateModal();
		ShowErrorModal();
	}

	private static void ShowUpdateModal()
	{
		if (ShowUpdate)
		{
			ImGui.OpenPopup(_updateId);
			ShowUpdate = false;
		}

		Vector2 center = ImGui.GetMainViewport().GetCenter();
		ImGui.SetNextWindowPos(center, ImGuiCond.Always, new(0.5f, 0.5f));
		if (ImGui.BeginPopupModal(_updateId))
		{
			ImGui.Text($"Version {AvailableVersion} is available. Re-run the launcher to install it.");

			if (ImGui.Button("OK", new(120, 0)))
				ImGui.CloseCurrentPopup();

			ImGui.EndPopup();
		}
	}

	private static void ShowErrorModal()
	{
		if (ShowError)
		{
			ImGui.OpenPopup(_errorId);
			ShowError = false;
		}

		Vector2 center = ImGui.GetMainViewport().GetCenter();
		ImGui.SetNextWindowPos(center, ImGuiCond.Always, new(0.5f, 0.5f));
		if (ImGui.BeginPopupModal(_errorId))
		{
			ImGui.Text(ErrorText);

			if (ImGui.Button("OK", new(120, 0)))
				ImGui.CloseCurrentPopup();

			ImGui.EndPopup();
		}
	}
}
