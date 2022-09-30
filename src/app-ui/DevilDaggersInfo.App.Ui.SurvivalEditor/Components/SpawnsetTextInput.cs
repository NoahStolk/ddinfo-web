using DevilDaggersInfo.App.Ui.Base.Components;
using DevilDaggersInfo.App.Ui.Base.Enums;
using Warp.Ui;

namespace DevilDaggersInfo.App.Ui.SurvivalEditor.Components;

public class SpawnsetTextInput : TextInput
{
	public SpawnsetTextInput(
		Rectangle metric,
		bool isNumeric,
		Action<string>? onEnter,
		Action<string>? onDeselect,
		Action<string>? onInput,
		Color backgroundColor,
		Color borderColor,
		Color hoverBackgroundColor,
		Color textColor,
		Color activeBorderColor,
		Color cursorColor,
		Color selectionColor,
		int borderSize,
		FontSize fontSize,
		int charWidth,
		int textRenderingHorizontalOffset)
		: base(metric, isNumeric, onEnter, onDeselect, onInput, backgroundColor, borderColor, hoverBackgroundColor, textColor, activeBorderColor, cursorColor, selectionColor, borderSize, fontSize, charWidth, textRenderingHorizontalOffset)
	{
	}

	public void SetTextIfDeselected(string value)
	{
		if (!IsSelected)
			SetText(value);
	}
}
