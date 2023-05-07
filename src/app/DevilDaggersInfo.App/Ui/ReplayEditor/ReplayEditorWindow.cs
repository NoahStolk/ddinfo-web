using DevilDaggersInfo.App.Ui.ReplayEditor.State;
using ImGuiNET;

namespace DevilDaggersInfo.App.Ui.ReplayEditor;

public static class ReplayEditorWindow
{
	private static float _time;
	public static float Time
	{
		get => _time;
		set => _time = value;
	}

	public static void Update(float delta)
	{
		if (_time < ReplayState.Replay.Header.Time)
			_time += delta;
	}

	public static void Render()
	{
		ImGui.PushStyleVar(ImGuiStyleVar.WindowMinSize, Constants.MinWindowSize);
		ImGui.Begin("Replay Editor", ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.MenuBar | ImGuiWindowFlags.NoScrollWithMouse);
		ImGui.PopStyleVar();

		ReplayEditorMenu.Render();

		ImGui.SliderFloat("Time", ref _time, 0, ReplayState.Replay.Header.Time, "%.4f", ImGuiSliderFlags.NoInput);

		ImGui.End();
	}
}
