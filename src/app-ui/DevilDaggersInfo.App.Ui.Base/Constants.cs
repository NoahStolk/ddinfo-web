using Warp.Ui;

namespace DevilDaggersInfo.App.Ui.Base;

public static class Constants
{
	public const int NativeWidth = 1024;
	public const int NativeHeight = 768;

	public static Rectangle Full { get; } = new(0, 0, ushort.MaxValue, ushort.MaxValue);
}
