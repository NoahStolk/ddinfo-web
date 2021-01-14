using System;

namespace DevilDaggersWebsite.Razor.Pagination
{
	[Flags]
	public enum ScreenWidthVisibilities
	{
		None = 0,
		Lg = 1,
		Md = 2,
		Sm = 4,
		Xs = 8,
	}
}
