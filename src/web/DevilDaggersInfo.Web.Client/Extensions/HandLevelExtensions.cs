using DevilDaggersInfo.Core.Spawnset;
using System.Diagnostics;

namespace DevilDaggersInfo.Web.Client.Extensions;

public static class HandLevelExtensions
{
	public static string ToDisplayString(this HandLevel handLevel) => handLevel switch
	{
		HandLevel.Level1 => "Level 1",
		HandLevel.Level2 => "Level 2",
		HandLevel.Level3 => "Level 3",
		HandLevel.Level4 => "Level 4",
		_ => throw new UnreachableException(),
	};

	public static HandLevel ToCore(this Api.Main.Spawnsets.HandLevel handLevel) => handLevel switch
	{
		Api.Main.Spawnsets.HandLevel.Level1 => HandLevel.Level1,
		Api.Main.Spawnsets.HandLevel.Level2 => HandLevel.Level2,
		Api.Main.Spawnsets.HandLevel.Level3 => HandLevel.Level3,
		Api.Main.Spawnsets.HandLevel.Level4 => HandLevel.Level4,
		_ => throw new UnreachableException(),
	};
}
