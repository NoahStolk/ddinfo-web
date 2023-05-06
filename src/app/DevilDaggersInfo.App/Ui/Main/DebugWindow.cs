using ImGuiNET;

namespace DevilDaggersInfo.App.Ui.Main;

public static class DebugWindow
{
	private static readonly List<string> _debugMessages = new();

	public static void Add(object? obj)
	{
		_debugMessages.Add(obj?.ToString() ?? "null");
	}

	public static void Render()
	{
		ImGui.SetNextWindowSize(new(512, 128));

		bool temp = true;
		ImGui.Begin("Debug", ref temp, ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoResize);

		foreach (string debugMessage in _debugMessages)
			ImGui.Text(debugMessage);

		if (ImGui.Button("Clear"))
			_debugMessages.Clear();

		ImGui.End();
	}
}
