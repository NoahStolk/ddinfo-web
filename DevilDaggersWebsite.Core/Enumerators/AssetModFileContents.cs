using System;

namespace DevilDaggersWebsite.Core.Enumerators
{
	[Flags]
	public enum AssetModFileContents
	{
		None = 0,
		RawAssets = 1,
		Mod = 2,
	}
}