using DevilDaggersInfo.App.Engine.Maths.Numerics;
using DevilDaggersInfo.App.Ui.SpawnsetEditor.State;
using DevilDaggersInfo.Core.Spawnset;
using ImGuiNET;
using System.Numerics;
using System.Security.Cryptography;

namespace DevilDaggersInfo.App.Ui.SpawnsetEditor;

public static class HistoryChild
{
	private static bool _updateScroll;

	static HistoryChild()
	{
		SpawnsetBinary spawnset = SpawnsetBinary.CreateDefault();
		History = new List<SpawnsetHistoryEntry> { new(spawnset, MD5.HashData(spawnset.ToBytes()), SpawnsetEditType.Reset) };
	}

	// Note; the history should never be empty.
	public static IReadOnlyList<SpawnsetHistoryEntry> History { get; private set; }

	public static int CurrentHistoryIndex { get; private set; }

	public static void UpdateHistory(IReadOnlyList<SpawnsetHistoryEntry> history, int currentHistoryIndex)
	{
		History = history;
		CurrentHistoryIndex = currentHistoryIndex;
		_updateScroll = true;
	}

	public static void Render()
	{
		ImGui.PushStyleVar(ImGuiStyleVar.ItemSpacing, new Vector2(8, 1));
		ImGui.BeginChild("HistoryChild", new(244, 712));

		for (int i = 0; i < History.Count; i++)
		{
			SpawnsetHistoryEntry history = History[i];

			if (_updateScroll && i == CurrentHistoryIndex)
			{
				ImGui.SetScrollHereY();
				_updateScroll = false;
			}

			const int borderSize = 2;
			ImGui.PushStyleVar(ImGuiStyleVar.FrameBorderSize, borderSize);
			ImGui.PushStyleVar(ImGuiStyleVar.ButtonTextAlign, new Vector2(0, 0.5f));

			Color color = history.EditType.GetColor();
			ImGui.PushStyleColor(ImGuiCol.Button, color);
			ImGui.PushStyleColor(ImGuiCol.ButtonHovered, color + new Vector4(0.3f, 0.3f, 0.3f, 0));
			ImGui.PushStyleColor(ImGuiCol.ButtonActive, color + new Vector4(0.5f, 0.5f, 0.5f, 0));
			ImGui.PushStyleColor(ImGuiCol.Border, i == CurrentHistoryIndex ? Color.White : Color.Black);

			ImGui.PushID($"HistoryButton{i}");
			if (ImGui.Button(history.EditType.GetChange(), new(226, 20)))
				SetHistoryIndex(i);

			ImGui.PopID();

			ImGui.PopStyleColor();
			ImGui.PopStyleColor();
			ImGui.PopStyleColor();
			ImGui.PopStyleColor();

			ImGui.PopStyleVar();
			ImGui.PopStyleVar();
		}

		ImGui.PopStyleVar();
		ImGui.EndChild();

		ImGuiIOPtr io = ImGui.GetIO();
		if (io.KeyCtrl)
		{
			if (ImGui.IsKeyPressed(ImGuiKey.Z))
				Undo();
			else if (ImGui.IsKeyPressed(ImGuiKey.Y))
				Redo();
		}
	}

	public static void Undo()
	{
		SetHistoryIndex(CurrentHistoryIndex - 1);
	}

	public static void Redo()
	{
		SetHistoryIndex(CurrentHistoryIndex + 1);
	}

	private static void SetHistoryIndex(int index)
	{
		if (index < 0 || index >= History.Count)
			return;

		// Workaround: Remove the window focus to prevent undo/redo not working when a text input is focused, and the next/previous history entry changes the value of that text input.
		ImGui.SetWindowFocus(null);

		CurrentHistoryIndex = Math.Clamp(index, 0, History.Count - 1);
		SpawnsetState.Spawnset = History[CurrentHistoryIndex].Spawnset.DeepCopy();
		_updateScroll = true;
	}
}
