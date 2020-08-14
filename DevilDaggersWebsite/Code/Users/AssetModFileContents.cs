using System;

namespace DevilDaggersWebsite.Code.Users
{
	[Flags]
	[Obsolete("Moved to database.")]
	public enum AssetModFileContents
	{
		None = 0,
		RawAssets = 1,
		Mod = 2,
	}
}