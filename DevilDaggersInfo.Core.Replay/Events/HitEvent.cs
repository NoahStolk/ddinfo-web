namespace DevilDaggersInfo.Core.Replay.Events;

public readonly record struct HitEvent(int A, int B, int C) : IEvent;
