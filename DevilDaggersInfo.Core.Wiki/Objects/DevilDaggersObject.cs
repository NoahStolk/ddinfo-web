using DevilDaggersInfo.Core.Wiki.Enums;
using DevilDaggersInfo.Core.Wiki.Structs;

namespace DevilDaggersInfo.Core.Wiki.Objects
{
	public abstract record DevilDaggersObject(GameVersions GameVersions, string Name, Color Color);
}
