using DevilDaggersInfo.App.Ui.Base.Enums;
using DevilDaggersInfo.App.Ui.Base.Rendering.Data;
using System.Numerics;
using Warp.Numerics;

namespace DevilDaggersInfo.App.Ui.Base.Rendering;

public static class RenderBatchCollector
{
	private static Scissor? _contextScissor;

	public static List<RectangleTriangle> RectangleTriangles { get; } = new();
	public static List<CircleLine> CircleLines { get; } = new();
	public static List<MonoSpaceText> MonoSpaceTexts4X6 { get; } = new();
	public static List<MonoSpaceText> MonoSpaceTexts8X8 { get; } = new();
	public static List<MonoSpaceText> MonoSpaceTexts12X12 { get; } = new();
	public static List<Sprite> Sprites { get; } = new();

	public static void Clear()
	{
		RectangleTriangles.Clear();
		CircleLines.Clear();
		MonoSpaceTexts4X6.Clear();
		MonoSpaceTexts8X8.Clear();
		MonoSpaceTexts12X12.Clear();
		Sprites.Clear();
	}

	public static void SetScissor(Scissor scissor)
	{
		_contextScissor = scissor;
	}

	public static void UnsetScissor()
	{
		_contextScissor = null;
	}

	public static void RenderRectangleTopLeft(Vector2i<int> scale, Vector2i<int> topLeft, float depth, Color color)
	{
		RenderRectangleCenter(scale, topLeft + scale / 2, depth, color);
	}

	public static void RenderRectangleCenter(Vector2i<int> scale, Vector2i<int> center, float depth, Color color)
	{
		RectangleTriangles.Add(new(scale, center, depth, color, _contextScissor));
	}

	public static void RenderCircleCenter(Vector2i<int> center, float radius, float depth, Color color)
	{
		CircleLines.Add(new(center, radius, depth, color, _contextScissor));
	}

	public static void RenderMonoSpaceText(FontSize fontSize, Vector2i<int> scale, Vector2i<int> position, float depth, Color color, object? obj, TextAlign textAlign)
	{
		string? text = obj?.ToString();
		if (string.IsNullOrWhiteSpace(text))
			return;

		List<MonoSpaceText> texts = fontSize switch
		{
			FontSize.F4X6 => MonoSpaceTexts4X6,
			FontSize.F8X8 => MonoSpaceTexts8X8,
			FontSize.F12X12 => MonoSpaceTexts12X12,
			_ => throw new(),
		};
		texts.Add(new(scale, position, depth, color, text, textAlign, _contextScissor));
	}

	public static void RenderSprite(Vector2 scale, Vector2 centerPosition, float depth, Texture texture, Color color)
	{
		Sprites.Add(new(scale, centerPosition, depth, texture, color, _contextScissor));
	}
}
