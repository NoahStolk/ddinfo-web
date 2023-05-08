using DevilDaggersInfo.App.Ui.SpawnsetEditor.Arena;
using ImGuiNET;
using System.Numerics;

namespace DevilDaggersInfo.App.Ui.SpawnsetEditor;

public static class SpawnsetEditorWindow
{
	public static void Render()
	{
		ImGui.PushStyleVar(ImGuiStyleVar.WindowMinSize, Constants.MinWindowSize);
		ImGui.Begin("Spawnset Editor", ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.MenuBar | ImGuiWindowFlags.NoScrollWithMouse);
		ImGui.PopStyleVar();

		SpawnsetEditorMenu.Render();

		SpawnsChild.Render();

		ImGui.SameLine();
		ArenaChild.Render();

		ImGui.SameLine();
		SettingsChild.Render();

		ImGui.SameLine();
		HistoryChild.Render();

		ImGui.End();

		ImGui.PushStyleVar(ImGuiStyleVar.WindowMinSize, Constants.MinWindowSize / 2);
		if (ImGui.Begin("3D Arena Editor"))
		{
			Vector2 framebufferSize = ImGui.GetWindowSize() - new Vector2(32, 48);
			Scene.SpawnsetEditorFramebufferData.ResizeIfNecessary((int)framebufferSize.X, (int)framebufferSize.Y);

			Vector2 cursorScreenPos = ImGui.GetCursorScreenPos();
			Scene.SpawnsetEditorScene.Camera.FramebufferOffset = cursorScreenPos;
			ImDrawListPtr drawList = ImGui.GetWindowDrawList();
			drawList.AddImage((IntPtr)Scene.SpawnsetEditorFramebufferData.TextureHandle, cursorScreenPos, cursorScreenPos + new Vector2(Scene.SpawnsetEditorFramebufferData.Width, Scene.SpawnsetEditorFramebufferData.Height), Vector2.UnitY, Vector2.UnitX);

			ImGui.End();
		}

		ImGui.PopStyleVar();
	}
}
