using DevilDaggersInfo.App.Ui.Base.Components;
using DevilDaggersInfo.App.Ui.Base.Enums;
using Warp.Numerics;
using Warp.Ui;

namespace DevilDaggersInfo.App.Ui.Base;

public static class ComponentBuilder
{
	public static TextInput CreateTextInput(Rectangle rectangle, bool isNumeric, Action<string>? onEnter, Action<string>? onDeselect, Action<string>? onInput)
	{
		return new(rectangle, isNumeric, onEnter, onDeselect, onInput, Color.Black, Color.Gray(0.75f), Color.Gray(0.25f), Color.White, Color.White, Color.Green, Color.Gray(0.5f), 2, FontSize.F8X8, 8, 8);
	}
}