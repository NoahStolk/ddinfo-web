using System;

namespace DevilDaggersWebsite.Core.Enumerators
{
	[Flags]
	public enum AssetModTypes
	{
		None = 0,
		Audio = 1,
		Texture = 2,
		Model = 4,
		Shader = 8,
		Particle = 16,
		Spawnset = 32,
	}
}