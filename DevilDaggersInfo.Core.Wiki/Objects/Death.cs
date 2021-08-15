namespace DevilDaggersInfo.Core.Wiki.Objects;

public record Death(GameVersions GameVersions, string Name, Color Color, LeaderboardDeathType LeaderboardDeathType)
	: DevilDaggersObject(GameVersions, Name, Color);
