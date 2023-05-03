using DevilDaggersInfo.App.Ui.Base.User.Settings;
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

		float lookSpeed = UserSettings.Model.LookSpeed;
		ImGui.SliderFloat("Look speed", ref lookSpeed, UserSettingsModel.LookSpeedMin, UserSettingsModel.LookSpeedMax);
		if (Math.Abs(UserSettings.Model.LookSpeed - lookSpeed) > 0.001f)
			UserSettings.Model = UserSettings.Model with { LookSpeed = lookSpeed };

		int fieldOfView = UserSettings.Model.FieldOfView;
		ImGui.SliderInt("Field of view", ref fieldOfView, UserSettingsModel.FieldOfViewMin, UserSettingsModel.FieldOfViewMax);
		if (UserSettings.Model.FieldOfView != fieldOfView)
			UserSettings.Model = UserSettings.Model with { FieldOfView = fieldOfView };

		ImGui.End();
	}
}
