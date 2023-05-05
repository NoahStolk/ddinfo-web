using DevilDaggersInfo.Core.Versioning;
using ImGuiNET;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace DevilDaggersInfo.App.Ui;

[SuppressMessage("Minor Code Smell", "S1104:Fields should not have public accessibility", Justification = "ref keyword")]
[SuppressMessage("Critical Code Smell", "S2223:Non-constant static fields should not be visible", Justification = "ref keyword")]
[SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "ref keyword")]
[SuppressMessage("Usage", "CA2211:Non-constant fields should not be visible", Justification = "ref keyword")]
public static class Modals
{
	private const string _updateId = "Update available";
	private const string _errorId = "Error";
	private const string _replacedSurvivalFileId = "Successfully replaced current survival file";

	public static bool ShowUpdate;
	public static bool ShowError;
	public static bool ShowReplacedSurvivalFile;

	public static AppVersion? AvailableVersion { get; set; }
	public static string? ErrorText { get; set; }

	public static void Render()
	{
		ShowModal(ref ShowUpdate, _updateId, () => ImGui.Text($"Version {AvailableVersion} is available. Re-run the launcher to install it."));
		ShowModal(ref ShowError, _errorId, () => ImGui.TextWrapped(ErrorText));
		ShowModal(ref ShowReplacedSurvivalFile, _replacedSurvivalFileId, () => ImGui.Text("The current survival file has been replaced with the current spawnset."));
	}

	private static void ShowModal(ref bool showModal, string modalId, Action onBegin)
	{
		if (showModal)
		{
			ImGui.OpenPopup(modalId);
			showModal = false;
		}

		Vector2 center = ImGui.GetMainViewport().GetCenter();
		ImGui.SetNextWindowPos(center, ImGuiCond.Always, new(0.5f, 0.5f));

		bool temp = true;
		if (ImGui.BeginPopupModal(modalId, ref temp, ImGuiWindowFlags.NoMove | ImGuiWindowFlags.NoResize))
		{
			onBegin();

			ImGui.Spacing();
			ImGui.Separator();
			ImGui.Spacing();
			if (ImGui.Button("OK", new(120, 0)))
				ImGui.CloseCurrentPopup();

			ImGui.EndPopup();
		}
	}
}
