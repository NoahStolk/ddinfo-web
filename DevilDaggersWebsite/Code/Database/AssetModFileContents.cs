using System;

namespace DevilDaggersWebsite.Code.Database
{
	[Flags]
	public enum AssetModFileContents
	{
		None = 0,
		RawAssets = 1,
		Mod = 2,
	}
}