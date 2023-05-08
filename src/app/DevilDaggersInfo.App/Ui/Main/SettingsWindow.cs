using DevilDaggersInfo.App.User.Settings;
using DevilDaggersInfo.App.User.Settings.Model;
using ImGuiNET;

namespace DevilDaggersInfo.App.Ui.Main;

public static class SettingsWindow
{
	public static void Render(ref bool show)
	{
		if (!show)
			return;

		ImGui.SetNextWindowSize(new(512, 128));

		ImGui.Begin("Settings", ref show, ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoResize);

		float lookSpeed = UserSettings.Model.LookSpeed;
		ImGui.SliderFloat("Look speed", ref lookSpeed, UserSettingsModel.LookSpeedMin, UserSettingsModel.LookSpeedMax);
		if (Math.Abs(UserSettings.Model.LookSpeed - lookSpeed) > 0.001f)
			UserSettings.Model = UserSettings.Model with { LookSpeed = lookSpeed };

		int fieldOfView = UserSettings.Model.FieldOfView;
		ImGui.SliderInt("Field of view", ref fieldOfView, UserSettingsModel.FieldOfViewMin, UserSettingsModel.FieldOfViewMax);
		if (UserSettings.Model.FieldOfView != fieldOfView)
			UserSettings.Model = UserSettings.Model with { FieldOfView = fieldOfView };

		bool showDebugOutput = UserSettings.Model.ShowDebugOutput;
		ImGui.Checkbox("Show debug output", ref showDebugOutput);
		if (UserSettings.Model.ShowDebugOutput != showDebugOutput)
			UserSettings.Model = UserSettings.Model with { ShowDebugOutput = showDebugOutput };

		ImGui.End();
	}
}
