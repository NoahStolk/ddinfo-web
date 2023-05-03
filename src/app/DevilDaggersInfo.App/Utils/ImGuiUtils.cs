using ImGuiNET;

namespace DevilDaggersInfo.App.Utils;

public static class ImGuiUtils
{
	public static float GetTextSize(string text)
	{
		return ImGui.GetFontSize() * text.Length / 2; // Width is twice as small as height (which is the font size).
	}
}
