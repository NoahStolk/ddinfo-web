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

		[Obsolete("Remove from database.")]
		Particle = 16,

		[Obsolete("Remove from database.")]
		Spawnset = 32,
	}
}
