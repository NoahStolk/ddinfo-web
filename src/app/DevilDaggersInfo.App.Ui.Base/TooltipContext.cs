using Warp.NET.Text;

namespace DevilDaggersInfo.App.Ui.Base;

public readonly record struct TooltipContext(string Text, Color ForegroundColor, Color BackgroundColor, TextAlign TextAlign);