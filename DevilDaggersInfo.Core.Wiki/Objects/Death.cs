namespace DevilDaggersInfo.Core.Wiki.Objects;

public record Death(GameVersions GameVersions, string Name, Color Color, byte LeaderboardDeathType)
	: DevilDaggersObject(GameVersions, Name, Color);
