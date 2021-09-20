namespace DevilDaggersInfo.Core.Wiki.Objects;

public readonly record struct Death(GameVersion GameVersion, string Name, Color Color, byte LeaderboardDeathType);
