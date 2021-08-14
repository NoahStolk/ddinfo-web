using DevilDaggersInfo.Core.Wiki.Enums;
using DevilDaggersInfo.Core.Wiki.Objects;

namespace DevilDaggersInfo.Core.Wiki
{
	public static class Daggers
	{
		public static readonly Dagger Default = new(GameVersions.V1 | GameVersions.V2 | GameVersions.V3 | GameVersions.V31, "Default", new(0x44, 0x44, 0x44), null);
		public static readonly Dagger Bronze = new(GameVersions.V1 | GameVersions.V2 | GameVersions.V3 | GameVersions.V31, "Bronze", new(0xCD, 0x7F, 0x32), 60);
		public static readonly Dagger Silver = new(GameVersions.V1 | GameVersions.V2 | GameVersions.V3 | GameVersions.V31, "Silver", new(0xDD, 0xDD, 0xDD), 120);
		public static readonly Dagger Golden = new(GameVersions.V1 | GameVersions.V2 | GameVersions.V3 | GameVersions.V31, "Golden", new(0xFF, 0xDF, 0x00), 250);
		public static readonly Dagger Devil = new(GameVersions.V1 | GameVersions.V2 | GameVersions.V3 | GameVersions.V31, "Devil", new(0xFF, 0x00, 0x00), 500);
		public static readonly Dagger Leviathan = new(GameVersions.V31, "Leviathan", new(0xA0, 0x00, 0x00), 1000);
	}
}
