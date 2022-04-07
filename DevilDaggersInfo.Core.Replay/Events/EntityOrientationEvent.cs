namespace DevilDaggersInfo.Core.Replay.Events;

public readonly record struct EntityOrientationEvent(int EntityId, Int16Mat3x3 Orientation) : IEntityEvent;
