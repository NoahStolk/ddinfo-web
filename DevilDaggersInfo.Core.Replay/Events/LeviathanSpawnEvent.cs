namespace DevilDaggersInfo.Core.Replay.Events;

public readonly record struct LeviathanSpawnEvent(int EntityId) : IEvent;
