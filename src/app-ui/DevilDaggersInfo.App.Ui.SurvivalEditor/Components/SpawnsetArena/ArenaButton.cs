using DevilDaggersInfo.App.Ui.Base.Components;
using DevilDaggersInfo.App.Ui.Base.Enums;
using Warp.Ui;

namespace DevilDaggersInfo.App.Ui.SurvivalEditor.Components.SpawnsetArena;

// TODO: Move to ComponentBuilder.
public class ArenaButton : Button
{
	public ArenaButton(Rectangle metric, Action onClick, Color backgroundColor, Color textColor, string text, FontSize fontSize)
		: base(metric, onClick, backgroundColor, Color.Black, Color.Blue, textColor, text, TextAlign.Middle, 2, fontSize)
	{
	}
}
