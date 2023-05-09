using ImGuiNET;
using System.Numerics;

namespace DevilDaggersInfo.App.Ui.ReplayEditor;

public static class ReplayEditor3DWindow
{
	public static void Render()
	{
		ImGui.PushStyleVar(ImGuiStyleVar.WindowMinSize, Constants.MinWindowSize / 2);
		if (ImGui.Begin("3D Replay Viewer"))
		{
			Vector2 framebufferSize = ImGui.GetWindowSize() - new Vector2(32, 48);
			Scene.ReplayEditorFramebufferData.ResizeIfNecessary((int)framebufferSize.X, (int)framebufferSize.Y);

			Vector2 cursorScreenPos = ImGui.GetCursorScreenPos();
			Scene.ReplayArenaScene.Camera.FramebufferOffset = cursorScreenPos;
			ImDrawListPtr drawList = ImGui.GetWindowDrawList();
			drawList.AddImage((IntPtr)Scene.ReplayEditorFramebufferData.TextureHandle, cursorScreenPos, cursorScreenPos + new Vector2(Scene.ReplayEditorFramebufferData.Width, Scene.ReplayEditorFramebufferData.Height), Vector2.UnitY, Vector2.UnitX);

			ImGui.End();
		}

		ImGui.PopStyleVar();
	}
}
