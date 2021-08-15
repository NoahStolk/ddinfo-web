namespace DevilDaggersInfo.Core.Wiki.Enums;

/// <summary>
/// Represents the death types from V2. The values are partially made up, because history was never kept track of for this game version.
/// The values must not be modified since the leaderboard history depends on it.
/// </summary>
public enum DeathTypeV2 : byte
{
	Fallen = 0,
	Swarmed = 1,
	Impaled = 2,
	Gored = 3,
	Infested = 4,
	Opened = 5,
	Purged = 6,
	Desecrated = 7,
	Sacrificed = 8,
	Eviscerated = 9,
	Annihilated = 10,
	Envenomated = 12,
	Stricken = 16,
	Devastated = 17,
}
