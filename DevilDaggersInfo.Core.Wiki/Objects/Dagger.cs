using DevilDaggersInfo.Core.Wiki.Enums;
using DevilDaggersInfo.Core.Wiki.Structs;

namespace DevilDaggersInfo.Core.Wiki.Objects
{
	public record Dagger(GameVersions GameVersions, string Name, Color Color, int UnlockSecond)
		: DevilDaggersObject(GameVersions, Name, Color);
}
