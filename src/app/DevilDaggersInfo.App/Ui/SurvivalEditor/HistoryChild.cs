using DevilDaggersInfo.App.Engine.Maths.Numerics;
using DevilDaggersInfo.App.Ui.SurvivalEditor.State;
using DevilDaggersInfo.App.Utils;
using ImGuiNET;
using Silk.NET.Input;
using System.Numerics;

namespace DevilDaggersInfo.App.Ui.SurvivalEditor;

public static class HistoryChild
{
	private static readonly ImGuiIoState _keyZ = new(false, (int)Key.Z);
	private static readonly ImGuiIoState _keyY = new(false, (int)Key.Y);

	public static void Render()
	{
		ImGui.PushStyleVar(ImGuiStyleVar.ItemSpacing, new Vector2(8, 1));
		ImGui.BeginChild("HistoryChild", new(256, 712));

		for (int i = 0; i < SpawnsetState.History.Count; i++)
		{
			SpawnsetHistoryEntry history = SpawnsetState.History[i];

			const int borderSize = 2;
			ImGui.PushStyleVar(ImGuiStyleVar.FrameBorderSize, borderSize);

			Color color = history.EditType.GetColor();
			ImGui.PushStyleColor(ImGuiCol.Button, color);
			ImGui.PushStyleColor(ImGuiCol.ButtonHovered, color + new Vector4(0.3f, 0.3f, 0.3f, 0));
			ImGui.PushStyleColor(ImGuiCol.ButtonActive, color + new Vector4(0.5f, 0.5f, 0.5f, 0));
			ImGui.PushStyleColor(ImGuiCol.Border, i == SpawnsetState.CurrentHistoryIndex ? Color.White : Color.Black);

			ImGui.Button(history.EditType.GetChange(), new(240, 20));

			ImGui.PopStyleColor();
			ImGui.PopStyleColor();
			ImGui.PopStyleColor();
			ImGui.PopStyleColor();

			ImGui.PopStyleVar();
		}

		ImGui.PopStyleVar();
		ImGui.EndChild();

		ImGuiIOPtr io = ImGui.GetIO();
		_keyZ.Update(io);
		_keyY.Update(io);

		if (io.KeyCtrl)
		{
			if (_keyZ.JustPressed && SpawnsetState.CurrentHistoryIndex > 0)
				SetHistory(SpawnsetState.CurrentHistoryIndex - 1);
			else if (_keyY.JustPressed && SpawnsetState.CurrentHistoryIndex < SpawnsetState.History.Count - 1)
				SetHistory(SpawnsetState.CurrentHistoryIndex + 1);
		}

		static void SetHistory(int index)
		{
			SpawnsetState.CurrentHistoryIndex = Math.Clamp(index, 0, SpawnsetState.History.Count - 1);
			SpawnsetState.Spawnset = SpawnsetState.History[SpawnsetState.CurrentHistoryIndex].Spawnset.DeepCopy();
		}
	}
}
