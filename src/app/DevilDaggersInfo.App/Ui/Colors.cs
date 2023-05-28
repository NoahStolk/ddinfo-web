using DevilDaggersInfo.App.Engine.Maths.Numerics;
using ImGuiNET;

namespace DevilDaggersInfo.App.Ui;

public static class Colors
{
	private const byte _alphaButton = 102;
	private const byte _alphaButtonActive = 191;
	private const byte _alphaHeader = 79;
	private const byte _alphaHeaderHovered = 204;
	private const byte _alphaFrameBackground = 138;
	private const byte _alphaTitleBackgroundActive = 138;
	private const byte _alphaFrameBackgroundHovered = 102;
	private const byte _alphaFrameBackgroundActive = 171;
	private const byte _alphaSeparatorHovered = 199;

	public static ColorConfiguration SpawnsetEditor { get; } = new()
	{
		Primary = new(250, 66, 66, 255),
		Secondary = new(224, 61, 61, 255),
		Tertiary = new(122, 41, 41, 255),
		Quaternary = new(191, 26, 26, 255),
	};

	public static ColorConfiguration CustomLeaderboards { get; } = new()
	{
		Primary = new(150, 66, 250, 255),
		Secondary = new(133, 61, 224, 255),
		Tertiary = new(74, 41, 122, 255),
		Quaternary = new(102, 26, 191, 255),
	};

	public static ColorConfiguration ReplayEditor { get; } = new()
	{
		Primary = new(66, 66, 250, 255),
		Secondary = new(61, 61, 224, 255),
		Tertiary = new(41, 41, 122, 255),
		Quaternary = new(26, 26, 191, 255),
	};

	public static ColorConfiguration Practice { get; } = new()
	{
		Primary = new(250, 150, 66, 255),
		Secondary = new(224, 133, 61, 255),
		Tertiary = new(122, 74, 41, 255),
		Quaternary = new(191, 102, 26, 255),
	};

	public static Color AssetEditor => new(66, 250, 66, 255);
	public static Color ModManager => new(66, 250, 150, 255);

	public static void SetColors(ColorConfiguration colorConfiguration)
	{
		ImGuiStylePtr style = ImGui.GetStyle();
		style.Colors[(int)ImGuiCol.CheckMark] = colorConfiguration.Primary;
		style.Colors[(int)ImGuiCol.SliderGrab] = colorConfiguration.Secondary;
		style.Colors[(int)ImGuiCol.SliderGrabActive] = colorConfiguration.Primary;
		style.Colors[(int)ImGuiCol.Button] = colorConfiguration.Primary with { A = _alphaButton };
		style.Colors[(int)ImGuiCol.ButtonHovered] = colorConfiguration.Primary;
		style.Colors[(int)ImGuiCol.ButtonActive] = colorConfiguration.Primary with { B = _alphaButtonActive };
		style.Colors[(int)ImGuiCol.Header] = colorConfiguration.Primary with { A = _alphaHeader };
		style.Colors[(int)ImGuiCol.HeaderHovered] = colorConfiguration.Primary with { A = _alphaHeaderHovered };
		style.Colors[(int)ImGuiCol.HeaderActive] = colorConfiguration.Primary;
		style.Colors[(int)ImGuiCol.FrameBg] = colorConfiguration.Tertiary with { A = _alphaFrameBackground };
		style.Colors[(int)ImGuiCol.TitleBgActive] = colorConfiguration.Tertiary with { A = _alphaTitleBackgroundActive };
		style.Colors[(int)ImGuiCol.FrameBgHovered] = colorConfiguration.Primary with { A = _alphaFrameBackgroundHovered };
		style.Colors[(int)ImGuiCol.FrameBgActive] = colorConfiguration.Primary with { A = _alphaFrameBackgroundActive };
		style.Colors[(int)ImGuiCol.SeparatorHovered] = colorConfiguration.Quaternary with { A = _alphaSeparatorHovered };
		style.Colors[(int)ImGuiCol.SeparatorActive] = colorConfiguration.Quaternary;
	}
}
