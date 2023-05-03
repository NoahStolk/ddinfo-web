using DevilDaggersInfo.App.Ui.Base.User.Settings.Model;
using ImGuiNET;

namespace DevilDaggersInfo.App.Windows;

public static class SettingsWindow
{
	public static void Render(ref bool show)
	{
		if (!show)
			return;

		ImGui.Begin("Settings", ref show, ImGuiWindowFlags.NoCollapse);

		float lookSpeed = 20; // TODO: UserSettings.Model.LookSpeed
		ImGui.SliderFloat("Look speed", ref lookSpeed, UserSettingsModel.LookSpeedMin, UserSettingsModel.LookSpeedMax);

		int fieldOfView = 90; // TODO: UserSettings.Model.FieldOfView
		ImGui.SliderInt("Field of view", ref fieldOfView, UserSettingsModel.FieldOfViewMin, UserSettingsModel.FieldOfViewMax);

		ImGui.End();
	}
}
