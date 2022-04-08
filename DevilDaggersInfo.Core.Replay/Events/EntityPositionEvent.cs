namespace DevilDaggersInfo.Core.Replay.Events;

public readonly record struct EntityPositionEvent(int EntityId, Int16Vec3 Position) : IEvent;
