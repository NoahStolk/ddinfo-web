using ImGuiNET;
using System.Numerics;

namespace DevilDaggersInfo.App.Ui;

public static class Constants
{
	public const ImGuiWindowFlags LayoutFlags = ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoMove | ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoTitleBar | ImGuiWindowFlags.NoBackground | ImGuiWindowFlags.NoBringToFrontOnFocus | ImGuiWindowFlags.NoNavFocus;

	public static Vector2 LayoutSize { get; } = new(1024, 768);
}
