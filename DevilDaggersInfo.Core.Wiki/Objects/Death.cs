namespace DevilDaggersInfo.Core.Wiki.Objects;

public record Death(GameVersions GameVersions, string Name, Color Color)
	: DevilDaggersObject(GameVersions, Name, Color);
