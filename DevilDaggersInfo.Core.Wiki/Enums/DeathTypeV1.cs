namespace DevilDaggersInfo.Core.Wiki.Enums;

/// <summary>
/// Represents the death types from V1. The values are partially made up, because history was never kept track of for this game version.
/// The values must not be modified since the leaderboard history depends on it.
/// </summary>
public enum DeathTypeV1 : byte
{
	Fallen = 0,
	Swarmed = 1,
	Impaled = 2,
	Infested = 4,
	Purged = 6,
	Sacrificed = 8,
	Eviscerated = 9,
	Annihilated = 10,
	Stricken = 16,
	Devastated = 17,
	Dismembered = 18,
}
