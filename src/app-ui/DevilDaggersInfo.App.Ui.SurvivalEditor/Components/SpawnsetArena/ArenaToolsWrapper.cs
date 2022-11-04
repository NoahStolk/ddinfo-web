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

		AddToolButton(0 + _arenaButtonSize * 0, 0, ArenaTool.Pencil, Textures.Pencil, "Pencil");
		AddToolButton(0 + _arenaButtonSize * 1, 0, ArenaTool.Line, Textures.Line, "Line");
		AddToolButton(0 + _arenaButtonSize * 2, 0, ArenaTool.Rectangle, Textures.Rectangle, "Rectangle");
		AddToolButton(0 + _arenaButtonSize * 3, 0, ArenaTool.Bucket, Textures.Bucket, "Bucket");
		AddToolButton(0 + _arenaButtonSize * 4, 0, ArenaTool.Dagger, Textures.Dagger, "Race dagger");

		AddBucketButtons(0, _arenaButtonSize);

		SetTool();
	}

	private void AddToolButton(int offsetX, int offsetY, ArenaTool arenaTool, Texture texture, string tooltipText)
	{
		void SetArenaTool()
		{
			StateManager.SetArenaTool(arenaTool);
			SetTool();
		}

		IconButton button = new(Rectangle.At(offsetX, offsetY, _arenaButtonSize, _arenaButtonSize), SetArenaTool, GlobalStyles.DefaultButtonStyle, tooltipText, texture);
		NestingContext.Add(button);
	}

	private void AddBucketButtons(int offsetX, int offsetY)
	{
		int y = offsetY;
		(_textInputBucketTolerance, _labelBucketTolerance) = AddSetting("Tolerance", offsetX, ref y, ChangeBucketTolerance);
		(_textInputBucketVoidHeight, _labelBucketVoidHeight) = AddSetting("Void height", offsetX, ref y, ChangeBucketVoidHeight);

		_textInputBucketTolerance.SetText(StateManager.ArenaEditorState.BucketTolerance.ToString("0.0"));
		_textInputBucketVoidHeight.SetText(StateManager.ArenaEditorState.BucketVoidHeight.ToString("0.0"));
	}

	private (TextInput TextInput, Label Label) AddSetting(string labelText, int x, ref int y, Action<string> onInput)
	{
		int halfWidth = _width / 2;
		Label label = new(Rectangle.At(x, y, halfWidth, 16), Color.White, labelText, TextAlign.Left, FontSize.F8X8);
		TextInput textInput = CreateTextInput(Rectangle.At(x + halfWidth, y, halfWidth, 16), onInput);
		NestingContext.Add(label);
		NestingContext.Add(textInput);
		y += 16;

		return (textInput, label);

		TextInput CreateTextInput(Rectangle rectangle, Action<string> onChange)
		{
			return new(rectangle, true, onChange, onChange, onChange, GlobalStyles.TextInput);
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
