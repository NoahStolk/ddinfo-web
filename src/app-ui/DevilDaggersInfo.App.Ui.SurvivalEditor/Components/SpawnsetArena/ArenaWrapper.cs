using DevilDaggersInfo.App.Ui.Base;
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

	private readonly SpawnsetTextInput _textInputShrinkStart;
	private readonly SpawnsetTextInput _textInputShrinkEnd;
	private readonly SpawnsetTextInput _textInputShrinkRate;
	private readonly SpawnsetTextInput _textInputBrightness;
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

		const int toolButtonOffsetY = 288;
		AddToolButton(buttonsOffsetX, toolButtonOffsetY, ArenaTool.Pencil, "P");
		AddToolButton(buttonsOffsetX + _arenaButtonSize, toolButtonOffsetY, ArenaTool.Line, "L");
		AddToolButton(buttonsOffsetX + _arenaButtonSize * 2, toolButtonOffsetY, ArenaTool.Rectangle, "R");
		AddToolButton(buttonsOffsetX + _arenaButtonSize * 3, toolButtonOffsetY, ArenaTool.Bucket, "B");
		AddToolButton(buttonsOffsetX, toolButtonOffsetY + _arenaButtonSize, ArenaTool.Dagger, "D");

		AddBucketButtons(buttonsOffsetX, toolButtonOffsetY);

		_shrinkSlider = new(Rectangle.At(0, _arena.Metric.TopLeft.Y + _arena.Metric.Size.Y + 8, _arena.Metric.Size.X, 16), _arena.SetShrinkCurrent, true, 0, StateManager.SpawnsetState.Spawnset.GetSliderMaxSeconds(), 0.001f, 0, 2, Color.White);
		NestingContext.Add(_shrinkSlider);

		_textInputShrinkStart = AddSetting("Shrink start", SpawnsetEditType.ShrinkStart, _arena.Metric.TopLeft.Y + _arena.Metric.Size.Y + 24, ChangeShrinkStart);
		_textInputShrinkEnd = AddSetting("Shrink end", SpawnsetEditType.ShrinkEnd, _arena.Metric.TopLeft.Y + _arena.Metric.Size.Y + 40, ChangeShrinkEnd);
		_textInputShrinkRate = AddSetting("Shrink rate", SpawnsetEditType.ShrinkRate, _arena.Metric.TopLeft.Y + _arena.Metric.Size.Y + 56, ChangeShrinkRate);
		_textInputBrightness = AddSetting("Brightness", SpawnsetEditType.Brightness, _arena.Metric.TopLeft.Y + _arena.Metric.Size.Y + 72, ChangeBrightness);
	}

	private SpawnsetTextInput AddSetting(string labelText, SpawnsetEditType spawnsetEditType, int y1, Action<string> onInput)
	{
		const int labelWidth = 112;
		Label label = new(Rectangle.At(0, y1, labelWidth, 16), Color.White, labelText, TextAlign.Left, FontSize.F8X8);
		SpawnsetTextInput textInput = SpawnsetComponentBuilder.CreateSpawnsetTextInput(Rectangle.At(labelWidth, y1, 64, 16), onInput, spawnsetEditType);
		NestingContext.Add(label);
		NestingContext.Add(textInput);
		return textInput;
	}

	private static void ChangeShrinkStart(string input) => SpawnsetSettingEditUtils.ChangeSetting<float>(v => StateManager.SpawnsetState.Spawnset with { ShrinkStart = v }, input);

	private static void ChangeShrinkEnd(string input) => SpawnsetSettingEditUtils.ChangeSetting<float>(v => StateManager.SpawnsetState.Spawnset with { ShrinkEnd = v }, input);

	private static void ChangeShrinkRate(string input) => SpawnsetSettingEditUtils.ChangeSetting<float>(v => StateManager.SpawnsetState.Spawnset with { ShrinkRate = v }, input);

	private static void ChangeBrightness(string input) => SpawnsetSettingEditUtils.ChangeSetting<float>(v => StateManager.SpawnsetState.Spawnset with { Brightness = v }, input);

	private void AddHeightButton(float height, int offsetX, int offsetY)
	{
		HeightButton button = new(Rectangle.At(offsetX, offsetY, _arenaButtonSize, _arenaButtonSize), () => StateManager.SetArenaSelectedHeight(height), height);
		NestingContext.Add(button);
	}

	private void AddToolButton(int offsetX, int offsetY, ArenaTool arenaTool, string text)
	{
		ArenaButton button = new(Rectangle.At(offsetX, offsetY, _arenaButtonSize, _arenaButtonSize), () => StateManager.SetArenaTool(arenaTool), Color.Yellow, Color.Black, text, FontSize.F8X8);
		NestingContext.Add(button);
	}

	private void AddBucketButtons(int offsetX, int offsetY)
	{
		void ChangeBucketTolerance(string s) => ParseUtils.TryParseAndExecute<float>(s, 0, StateManager.SetArenaBucketTolerance);
		void ChangeBucketVoidHeight(string s) => ParseUtils.TryParseAndExecute<float>(s, StateManager.SetArenaBucketVoidHeight);

		TextInput bucketTolerance = new(Rectangle.At(offsetX, offsetY + _arenaButtonSize * 2, 80, 16), true, ChangeBucketTolerance, ChangeBucketTolerance, ChangeBucketTolerance, Color.Black, Color.White, Color.Gray(63), Color.White, Color.White, Color.White, Color.Gray(63), 2, FontSize.F8X8, 8, 4);
		TextInput bucketVoidHeight = new(Rectangle.At(offsetX, offsetY + _arenaButtonSize * 3, 80, 16), true, ChangeBucketVoidHeight, ChangeBucketVoidHeight, ChangeBucketVoidHeight, Color.Black, Color.White, Color.Gray(63), Color.White, Color.White, Color.White, Color.Gray(63), 2, FontSize.F8X8, 8, 4);

		bucketTolerance.SetText(StateManager.ArenaEditorState.BucketTolerance.ToString("0.0"));
		bucketVoidHeight.SetText(StateManager.ArenaEditorState.BucketVoidHeight.ToString("0.0"));

		NestingContext.Add(bucketTolerance);
		NestingContext.Add(bucketVoidHeight);
	}

	public void SetSpawnset()
	{
		SpawnsetBinary spawnset = StateManager.SpawnsetState.Spawnset;

		_textInputShrinkStart.SetTextIfDeselected(spawnset.ShrinkStart.ToString("0.0"));
		_textInputShrinkEnd.SetTextIfDeselected(spawnset.ShrinkEnd.ToString("0.0"));
		_textInputShrinkRate.SetTextIfDeselected(spawnset.ShrinkRate.ToString("0.000"));
		_textInputBrightness.SetTextIfDeselected(spawnset.Brightness.ToString("0.0"));
		_shrinkSlider.Max = StateManager.SpawnsetState.Spawnset.GetSliderMaxSeconds();
		_shrinkSlider.CurrentValue = Math.Clamp(_shrinkSlider.CurrentValue, 0, _shrinkSlider.Max);
		_arena.SetShrinkCurrent(_shrinkSlider.CurrentValue);
	}
}
