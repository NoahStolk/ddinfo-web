using DevilDaggersInfo.App.Ui.Base.Components;
using DevilDaggersInfo.App.Ui.Base.Enums;
using DevilDaggersInfo.App.Ui.SurvivalEditor.Enums;
using DevilDaggersInfo.App.Ui.SurvivalEditor.States;
using DevilDaggersInfo.App.Ui.SurvivalEditor.Utils;
using DevilDaggersInfo.Core.Spawnset;
using Warp.Ui;
using Warp.Ui.Components;

namespace DevilDaggersInfo.App.Ui.SurvivalEditor.Components.SpawnsetArena;

public class ArenaWrapper : AbstractComponent
{
	private const int _arenaButtonSize = 20;

	private readonly ShrinkSlider _shrinkSlider;
	private readonly Arena _arena;

	public ArenaWrapper(Rectangle metric)
		: base(metric)
	{
		const int titleHeight = 48;

		_arena = new(new(0, titleHeight), 6);
		NestingContext.Add(_arena);

		Label title = new(Rectangle.At(0, 0, _arena.Metric.Size.X, titleHeight), Color.White, "Arena", TextAlign.Middle, FontSize.F12X12);
		NestingContext.Add(title);

		int buttonsOffsetX = _arena.Metric.Size.X + 8;

		Span<float> heights = stackalloc float[] { -1000, -1.1f, -1.01f, -1, -0.8f, -0.6f, -0.4f, -0.2f };
		for (int i = 0; i < heights.Length; i++)
		{
			float height = heights[i];
			int offsetX = i % 4 * _arenaButtonSize;
			int offsetY = i / 4 * _arenaButtonSize;
			AddHeightButton(height, buttonsOffsetX + offsetX, titleHeight + offsetY);
		}

		for (int i = 0; i < 32; i++)
		{
			int offsetX = i % 4 * _arenaButtonSize;
			int offsetY = i / 4 * _arenaButtonSize;

			float height = i < 16 ? i : 16 + (i - 16) * 2;
			AddHeightButton(height, buttonsOffsetX + offsetX, titleHeight + offsetY + _arenaButtonSize * 2);
		}

		_shrinkSlider = new(Rectangle.At(0, _arena.Metric.TopLeft.Y + _arena.Metric.Size.Y + 8, _arena.Metric.Size.X, 16), _arena.SetShrinkCurrent, true, 0, StateManager.SpawnsetState.Spawnset.GetSliderMaxSeconds(), 0.001f, 0, 2, Color.White);
		NestingContext.Add(_shrinkSlider);
	}

	private void AddHeightButton(float height, int offsetX, int offsetY)
	{
		HeightButton button = new(Rectangle.At(offsetX, offsetY, _arenaButtonSize, _arenaButtonSize), () => StateManager.SetArenaSelectedHeight(height), height);
		NestingContext.Add(button);
	}

	public void SetSpawnset()
	{
		_shrinkSlider.Max = StateManager.SpawnsetState.Spawnset.GetSliderMaxSeconds();
		_shrinkSlider.CurrentValue = Math.Clamp(_shrinkSlider.CurrentValue, 0, _shrinkSlider.Max);
		_arena.SetShrinkCurrent(_shrinkSlider.CurrentValue);
	}
}
