using DevilDaggersInfo.Core.Spawnset.Enums;

namespace DevilDaggersInfo.Core.Spawnset.Extensions
{
	public static class HandLevelExtensions
	{
		public static int GetStartGems(this HandLevel handLevel) => handLevel switch
		{
			HandLevel.Level2 => 10,
			HandLevel.Level3 => 70,
			HandLevel.Level4 => 220,
			_ => 0,
		};
	}
}
