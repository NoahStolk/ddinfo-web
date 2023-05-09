using ImGuiNET;

namespace DevilDaggersInfo.App.Ui;

public static class DebugWindow
{
	private static long _previousAllocatedBytes;

	private static readonly List<string> _debugMessages = new();

	public static void Add(object? obj)
	{
		_debugMessages.Add(obj?.ToString() ?? "null");
	}

	public static void Clear()
	{
		_debugMessages.Clear();
	}

	public static void Render()
	{
		ImGui.SetNextWindowSize(new(512, 384));

		bool temp = true;
		ImGui.Begin("Debug", ref temp, ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoResize);

		ImGui.Text(Application.RenderCounter.CountPerSecond + " FPS");
		ImGui.Text(Application.LastRenderDelta + " render delta");

		long allocatedBytes = GC.GetAllocatedBytesForCurrentThread();
		ImGui.Text(allocatedBytes + " bytes allocated on managed heap");
		ImGui.Text(allocatedBytes - _previousAllocatedBytes + " since last frame");
		_previousAllocatedBytes = allocatedBytes;

		ImGui.Text(GC.CollectionCount(0) + " gen 0 garbage collections");
		ImGui.Text(GC.CollectionCount(1) + " gen 1 garbage collections");
		ImGui.Text(GC.CollectionCount(2) + " gen 2 garbage collections");

		if (ImGui.Button("Clear"))
			_debugMessages.Clear();

		foreach (string debugMessage in _debugMessages)
			ImGui.Text(debugMessage);

		ImGui.End();
	}
}
