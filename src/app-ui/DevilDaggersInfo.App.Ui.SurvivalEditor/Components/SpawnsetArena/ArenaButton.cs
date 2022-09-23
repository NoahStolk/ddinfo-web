using DevilDaggersInfo.App.Ui.Base.Components;
using DevilDaggersInfo.App.Ui.Base.Enums;
using Warp.Ui;

namespace DevilDaggersInfo.App.Ui.SurvivalEditor.Components.SpawnsetArena;

public class ArenaButton : Button
{
	public ArenaButton(Rectangle metric, Action onClick, Color backgroundColor, Color textColor, string text, bool useSmallFont)
		: base(metric, onClick, backgroundColor, Color.Black, Color.Blue, textColor, text, TextAlign.Middle, 2, useSmallFont)
	{
	}
}
