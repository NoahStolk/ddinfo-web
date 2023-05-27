using System.Diagnostics;

namespace DevilDaggersInfo.Core.Spawnset.Extensions;

public static class SpawnsetSupportedGameVersionExtensions
{
	public static string ToDisplayString(this SpawnsetSupportedGameVersion version)
	{
		return version switch
		{
			SpawnsetSupportedGameVersion.V0AndLater => "V0 and later",
			SpawnsetSupportedGameVersion.V2AndLater => "V2 and later",
			SpawnsetSupportedGameVersion.V3_1AndLater => "V3.1 and later",
			_ => throw new UnreachableException(),
		};
	}
}
