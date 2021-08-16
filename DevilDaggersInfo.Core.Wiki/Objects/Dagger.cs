namespace DevilDaggersInfo.Core.Wiki.Objects;

public record Dagger(GameVersionFlags GameVersions, string Name, Color Color, int UnlockSecond)
	: DevilDaggersObject(GameVersions, Name, Color);
