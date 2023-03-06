using DevilDaggersInfo.Types.Core.Spawnsets;
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
}
