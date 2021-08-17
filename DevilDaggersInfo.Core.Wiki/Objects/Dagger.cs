namespace DevilDaggersInfo.Core.Wiki.Objects;

public record Dagger(GameVersion GameVersion, string Name, Color Color, int UnlockSecond)
	: DevilDaggersObject(GameVersion, Name, Color);
