using DevilDaggersInfo.Core.Wiki.Enums;
using DevilDaggersInfo.Core.Wiki.Structs;

namespace DevilDaggersInfo.Core.Wiki.Objects
{
	public record Death(GameVersions GameVersions, string Name, Color Color, LeaderboardDeathType LeaderboardDeathType)
		: DevilDaggersObject(GameVersions, Name, Color);
}
