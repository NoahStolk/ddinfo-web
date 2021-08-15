namespace DevilDaggersInfo.Core.Wiki.Objects;

public record Dagger(GameVersions GameVersions, string Name, Color Color, int UnlockSecond)
	: DevilDaggersObject(GameVersions, Name, Color);
