using DevilDaggersInfo.App.Ui.Base;
using DevilDaggersInfo.App.Ui.Base.Components;
using DevilDaggersInfo.App.Ui.Base.Enums;
using DevilDaggersInfo.App.Ui.SurvivalEditor.Enums;
using DevilDaggersInfo.App.Ui.SurvivalEditor.States;
using Warp.Ui;
using Warp.Ui.Components;

namespace DevilDaggersInfo.App.Ui.SurvivalEditor.Components.SpawnsetArena;

public class ArenaToolsWrapper : AbstractComponent
{
	private const int _arenaButtonSize = 20;

	private readonly int _width;

	private Label _labelBucketTolerance = null!;
	private Label _labelBucketVoidHeight = null!;
	private TextInput _textInputBucketTolerance = null!;
	private TextInput _textInputBucketVoidHeight = null!;

	public ArenaToolsWrapper(Rectangle metric)
		: base(metric)
	{
		_width = metric.Size.X;

		AddToolButton(0 + _arenaButtonSize * 0, 0, ArenaTool.Pencil, "P");
		AddToolButton(0 + _arenaButtonSize * 1, 0, ArenaTool.Line, "L");
		AddToolButton(0 + _arenaButtonSize * 2, 0, ArenaTool.Rectangle, "R");
		AddToolButton(0 + _arenaButtonSize * 3, 0, ArenaTool.Bucket, "B");
		AddToolButton(0 + _arenaButtonSize * 4, 0, ArenaTool.Dagger, "D");

		AddBucketButtons(0, _arenaButtonSize);
	}

	private void AddToolButton(int offsetX, int offsetY, ArenaTool arenaTool, string text)
	{
		void SetArenaTool(ArenaTool arenaTool)
		{
			StateManager.SetArenaTool(arenaTool);
			SetTool();
		}

		ArenaButton button = new(Rectangle.At(offsetX, offsetY, _arenaButtonSize, _arenaButtonSize), () => SetArenaTool(arenaTool), Color.Yellow, Color.Black, text, FontSize.F8X8);
		NestingContext.Add(button);
	}

	private void AddBucketButtons(int offsetX, int offsetY)
	{
		int y = offsetY;
		(_textInputBucketTolerance, _labelBucketTolerance) = AddSetting("Tolerance", offsetX, ref y, ChangeBucketTolerance);
		(_textInputBucketVoidHeight, _labelBucketVoidHeight) = AddSetting("Void height", offsetX, ref y, ChangeBucketVoidHeight);

		_textInputBucketTolerance.SetText(StateManager.ArenaEditorState.BucketTolerance.ToString("0.0"));
		_textInputBucketVoidHeight.SetText(StateManager.ArenaEditorState.BucketVoidHeight.ToString("0.0"));

		NestingContext.Add(_textInputBucketTolerance);
		NestingContext.Add(_textInputBucketVoidHeight);
	}

	private (SpawnsetTextInput TextInput, Label Label) AddSetting(string labelText, int x, ref int y, Action<string> onInput)
	{
		int halfWidth = _width / 2;
		Label label = new(Rectangle.At(x, y, halfWidth, 16), Color.White, labelText, TextAlign.Left, FontSize.F8X8);
		SpawnsetTextInput textInput = CreateTextInput(Rectangle.At(x + halfWidth, y, halfWidth, 16), onInput);
		NestingContext.Add(label);
		NestingContext.Add(textInput);
		y += 16;

		return (textInput, label);

		SpawnsetTextInput CreateTextInput(Rectangle rectangle, Action<string> onChange)
		{
			return new(rectangle, true, onChange, onChange, onChange, Color.Black, Color.Gray(0.75f), Color.Gray(0.25f), Color.White, Color.White, Color.Green, Color.Gray(0.5f), 2, FontSize.F8X8, 8, 8);
		}
	}

	private void SetTool()
	{
		bool isBucket = StateManager.ArenaEditorState.ArenaTool == ArenaTool.Bucket;
		_labelBucketTolerance.IsActive = isBucket;
		_labelBucketVoidHeight.IsActive = isBucket;
		_textInputBucketTolerance.IsActive = isBucket;
		_textInputBucketVoidHeight.IsActive = isBucket;
	}

	private static void ChangeBucketTolerance(string s) => ParseUtils.TryParseAndExecute<float>(s, 0, StateManager.SetArenaBucketTolerance);

	private static void ChangeBucketVoidHeight(string s) => ParseUtils.TryParseAndExecute<float>(s, StateManager.SetArenaBucketVoidHeight);
}
