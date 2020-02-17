using System;

namespace DevilDaggersWebsite.Code.Users
{
	[Flags]
	public enum AssetModFileContents
	{
		None = 0,
		RawAssets = 1,
		Mod = 2
	}
}