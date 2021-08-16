namespace DevilDaggersInfo.Core.Wiki.Enums;

[Flags]
#pragma warning disable S2344 // Enumeration type names should not have "Flags" or "Enum" suffixes
public enum GameVersionFlags
#pragma warning restore S2344 // Enumeration type names should not have "Flags" or "Enum" suffixes
{
	None = 0,
	V1_0 = 1,
	V2_0 = 1 << 1,
	V3_0 = 1 << 2,
	V3_1 = 1 << 3,
}
