using DevilDaggersInfo.Core.Spawnset;
using System.Diagnostics;

namespace DevilDaggersInfo.Web.Client.Extensions;

public static class HandLevelExtensions
{
	public static HandLevel ToCore(this ApiSpec.Main.Spawnsets.HandLevel handLevel) => handLevel switch
	{
		ApiSpec.Main.Spawnsets.HandLevel.Level1 => HandLevel.Level1,
		ApiSpec.Main.Spawnsets.HandLevel.Level2 => HandLevel.Level2,
		ApiSpec.Main.Spawnsets.HandLevel.Level3 => HandLevel.Level3,
		ApiSpec.Main.Spawnsets.HandLevel.Level4 => HandLevel.Level4,
		_ => throw new UnreachableException(),
	};
}
