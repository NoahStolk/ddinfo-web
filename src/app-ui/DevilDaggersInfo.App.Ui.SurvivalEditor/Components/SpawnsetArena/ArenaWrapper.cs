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

	private readonly TextInput _textInputShrinkStart;
	private readonly TextInput _textInputShrinkEnd;
	private readonly TextInput _textInputShrinkRate;
	private readonly TextInput _textInputBrightness;

	public ArenaWrapper(Rectangle metric)
		: base(metric)
	{
		Arena arena = new(default, 6);
		NestingContext.Add(arena);

		int buttonsOffsetX = arena.Metric.Size.X + 8;

		float[] heights = { -1000, -1.1f, -1.01f, -1, -0.8f, -0.6f, -0.4f, -0.2f };
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

		const int toolButtonOffsetY = 240;
		AddToolButton(0, toolButtonOffsetY, ArenaTool.Pencil, "P");
		AddToolButton(_arenaButtonSize, toolButtonOffsetY, ArenaTool.Line, "L");
		AddToolButton(_arenaButtonSize * 2, toolButtonOffsetY, ArenaTool.Rectangle, "R");
		AddToolButton(_arenaButtonSize * 3, toolButtonOffsetY, ArenaTool.Bucket, "B");

		TextInput AddSetting(string labelText, int y1, Action<string> onInput)
		{
			const int labelWidth = 112;
			Label label = new(Rectangle.At(0, y1, labelWidth, 16), Color.White, labelText, TextAlign.Left, FontSize.F8X8);
			TextInput textInput = ComponentBuilder.CreateTextInput(Rectangle.At(labelWidth, y1, 64, 16), true, OnInputAndSave, OnInputAndSave, onInput);
			NestingContext.Add(label);
			NestingContext.Add(textInput);
			return textInput;

			void OnInputAndSave(string input)
			{
				onInput(input);
				SpawnsetHistoryManager.Save(labelText);
			}
		}

		_textInputShrinkStart = AddSetting("Shrink start", arena.Metric.Size.Y + 8, ChangeShrinkStart);
		_textInputShrinkEnd = AddSetting("Shrink end", arena.Metric.Size.Y + 24, ChangeShrinkEnd);
		_textInputShrinkRate = AddSetting("Shrink rate", arena.Metric.Size.Y + 40, ChangeShrinkRate);
		_textInputBrightness = AddSetting("Brightness", arena.Metric.Size.Y + 56, ChangeBrightness);

		void ChangeShrinkStart(string input) => SpawnsetSettingEditUtils.ChangeSetting<float>(v => StateManager.SpawnsetState.Spawnset with { ShrinkStart = v }, input);
		void ChangeShrinkEnd(string input) => SpawnsetSettingEditUtils.ChangeSetting<float>(v => StateManager.SpawnsetState.Spawnset with { ShrinkEnd = v }, input);
		void ChangeShrinkRate(string input) => SpawnsetSettingEditUtils.ChangeSetting<float>(v => StateManager.SpawnsetState.Spawnset with { ShrinkRate = v }, input);
		void ChangeBrightness(string input) => SpawnsetSettingEditUtils.ChangeSetting<float>(v => StateManager.SpawnsetState.Spawnset with { Brightness = v }, input);

		void AddHeightButton(float height, int offsetX, int offsetY)
		{
			HeightButton button = new(Rectangle.At(buttonsOffsetX + offsetX, offsetY, _arenaButtonSize, _arenaButtonSize), () => StateManager.SetArenaSelectedHeight(height), height);
			NestingContext.Add(button);
		}

		void AddToolButton(int offsetX, int offsetY, ArenaTool arenaTool, string text)
		{
			ArenaButton button = new(Rectangle.At(buttonsOffsetX + offsetX, offsetY, _arenaButtonSize, _arenaButtonSize), () => StateManager.SetArenaTool(arenaTool), Color.Yellow, Color.Black, text, FontSize.F8X8);
			NestingContext.Add(button);
		}
	}

	public void SetSpawnset()
	{
		SpawnsetBinary spawnset = StateManager.SpawnsetState.Spawnset;

		_textInputShrinkStart.SetText(spawnset.ShrinkStart.ToString());
		_textInputShrinkEnd.SetText(spawnset.ShrinkEnd.ToString());
		_textInputShrinkRate.SetText(spawnset.ShrinkRate.ToString());
		_textInputBrightness.SetText(spawnset.Brightness.ToString());
	}
}
