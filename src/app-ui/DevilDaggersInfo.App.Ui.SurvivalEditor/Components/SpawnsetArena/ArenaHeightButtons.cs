using DevilDaggersInfo.App.Ui.Base;
using DevilDaggersInfo.App.Ui.Base.Components.Styles;
using DevilDaggersInfo.App.Ui.Base.Enums;
using DevilDaggersInfo.App.Ui.SurvivalEditor.States;
using DevilDaggersInfo.App.Ui.SurvivalEditor.Utils;
using Warp.NET.Ui;
using Warp.NET.Ui.Components;

namespace DevilDaggersInfo.App.Ui.SurvivalEditor.Components.SpawnsetArena;

public class ArenaHeightButtons : AbstractComponent
{
	private const int _arenaButtonSize = 20;

	public ArenaHeightButtons(IBounds bounds)
		: base(bounds)
	{
		Span<float> heights = stackalloc float[] { -1000, -1.1f, -1.01f, -1, -0.8f, -0.6f, -0.4f, -0.2f };
		for (int i = 0; i < heights.Length; i++)
		{
			float height = heights[i];
			int offsetX = i % 4 * _arenaButtonSize;
			int offsetY = i / 4 * _arenaButtonSize;
			AddHeightButton(height, offsetX, offsetY);
		}

		for (int i = 0; i < 32; i++)
		{
			int offsetX = i % 4 * _arenaButtonSize;
			int offsetY = i / 4 * _arenaButtonSize;

			float height = i < 16 ? i : 16 + (i - 16) * 2;
			AddHeightButton(height, offsetX, offsetY + _arenaButtonSize * 2);
		}
	}

	private void AddHeightButton(float height, int offsetX, int offsetY)
	{
		Color heightColor = TileUtils.GetColorFromHeight(height);

		string text = height.ToString(); // TODO: -1000 should probably be written as -1K.
		FontSize fontSize = text.Length > 2 ? FontSize.F4X6 : FontSize.F8X8;
		ButtonStyle buttonStyle = new(heightColor, Color.Black, Color.Lerp(heightColor, Color.White, 0.75f), 1);
		TextButtonStyle textStyle = new(heightColor.ReadableColorForBrightness(), TextAlign.Middle, fontSize);

		HeightButton button = new(Rectangle.At(offsetX, offsetY, _arenaButtonSize, _arenaButtonSize), () => StateManager.SetArenaSelectedHeight(height), buttonStyle, textStyle, text, height);
		NestingContext.Add(button);
	}
}
