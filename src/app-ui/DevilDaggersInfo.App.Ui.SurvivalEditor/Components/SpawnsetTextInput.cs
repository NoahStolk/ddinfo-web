using DevilDaggersInfo.App.Ui.Base.Components;
using DevilDaggersInfo.App.Ui.Base.Components.Styles;
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
		TextInputStyle textInputStyle)
		: base(metric, isNumeric, onEnter, onDeselect, onInput, textInputStyle)
	{
	}

	public void SetTextIfDeselected(string value)
	{
		if (!IsSelected)
			SetText(value);
	}
}
