namespace DevilDaggersInfo.Core.Replay.Events;

public readonly record struct EntityTargetEvent(int EntityId, Int16Vec3 Position) : IEvent;
