namespace DevilDaggersInfo.Core.Wiki.Objects;

public record Death(GameVersionFlags GameVersions, string Name, Color Color, byte LeaderboardDeathType)
	: DevilDaggersObject(GameVersions, Name, Color);
