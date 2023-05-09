using ImGuiNET;
using System.Numerics;

namespace DevilDaggersInfo.App.Ui.SpawnsetEditor;

public static class SpawnsetEditor3DWindow
{
	public static void Render()
	{
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
