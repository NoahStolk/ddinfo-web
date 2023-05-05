using ImGuiNET;

namespace DevilDaggersInfo.App.Ui.Main;

public static class DebugWindow
{
	public static List<string> DebugMessages { get; } = new();

	public static void Render()
	{
		ImGui.SetNextWindowSize(new(512, 128));

		bool temp = true;
		ImGui.Begin("Debug", ref temp, ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoResize);

		foreach (string debugMessage in DebugMessages)
			ImGui.Text(debugMessage);

		if (ImGui.Button("Clear"))
			DebugMessages.Clear();

		ImGui.End();
	}
}
