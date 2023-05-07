using DevilDaggersInfo.App.Engine.Maths.Numerics;
using DevilDaggersInfo.App.Engine.Utils;
using DevilDaggersInfo.App.Ui.ReplayEditor.State;
using DevilDaggersInfo.Core.Replay.Events.Enums;
using DevilDaggersInfo.Core.Replay.PostProcessing.ReplaySimulation;
using ImGuiNET;
using System.Numerics;

namespace DevilDaggersInfo.App.Ui.ReplayEditor;

public static class ReplayEditorWindow
{
	private static float _time;
	public static float Time
	{
		get => _time;
		set => _time = value;
	}

	public static PlayerInputSnapshot Snapshot { get; set; }

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

		const int border = 4;
		const int pointerSize = 4;
		const float pointerScale = 0.25f;
		const int size = 64;
		ImDrawListPtr drawList = ImGui.GetWindowDrawList();
		Vector2 origin = ImGui.GetCursorScreenPos();
		drawList.AddRect(origin, origin + new Vector2(size), ImGui.GetColorU32(Color.Gray(0.6f)));
		drawList.AddRectFilled(origin + new Vector2(border), origin + new Vector2(size - border), ImGui.GetColorU32(new Vector4(0, 0, 0, 1)));

		const int center = size / 2;
		const float max = center - border - pointerScale / 2;
		Vector2 pointerCenter = origin + new Vector2(center) + VectorUtils.Clamp(new Vector2(Snapshot.MouseX, Snapshot.MouseY) * pointerScale, -max, max);
		drawList.AddRect(pointerCenter - new Vector2(pointerSize / 2f), pointerCenter + new Vector2(pointerSize / 2f), ImGui.GetColorU32(Color.White));

		RenderInput(Snapshot.Left, "A");
		RenderInput(Snapshot.Right, "D");
		RenderInput(Snapshot.Forward, "W");
		RenderInput(Snapshot.Backward, "S");
		RenderInput(Snapshot.Jump is JumpType.StartedPress or JumpType.Hold, "Space");
		RenderInput(Snapshot.Shoot == ShootType.Hold, "LMB");
		RenderInput(Snapshot.ShootHoming == ShootType.Hold, "RMB");

		ImGui.End();
	}

	private static void RenderInput(bool used, string input)
	{
		// TODO: Position the inputs.
		Color color = used ? Color.Red : Color.White;
		ImGui.TextColored(color, input);
	}
}
