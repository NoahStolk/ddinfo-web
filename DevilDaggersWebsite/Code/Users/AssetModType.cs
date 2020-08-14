using System;

namespace DevilDaggersWebsite.Code.Users
{
	[Flags]
	public enum AssetModType
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