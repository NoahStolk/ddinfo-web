namespace DevilDaggersInfo.Core.Wiki.Objects;

public record Death(GameVersion GameVersion, string Name, Color Color, byte LeaderboardDeathType)
	: DevilDaggersObject(GameVersion, Name, Color);
