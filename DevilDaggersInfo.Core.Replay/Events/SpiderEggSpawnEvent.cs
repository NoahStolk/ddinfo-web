namespace DevilDaggersInfo.Core.Replay.Events;

public readonly record struct SpiderEggSpawnEvent(int EntityId, Vector3 Position) : IEvent;
