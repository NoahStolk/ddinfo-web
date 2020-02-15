using System;

namespace DevilDaggersWebsite.Code.Users
{
	[Flags]
	public enum AssetModFileContents
	{
		None = 0,
		Assets = 1,
		Packed = 2
	}
}