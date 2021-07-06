using System;

namespace DevilDaggersWebsite.Enumerators
{
	[Flags]
	public enum AssetModTypes
	{
		None = 0,
		Audio = 1,
		Texture = 2,
		Model = 4,
		Shader = 8,
	}
}
