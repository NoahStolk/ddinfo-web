namespace DevilDaggersInfo.Core.Replay.Events;

public readonly record struct DaggerSpawnEvent(int EntityId, Int16Vec3 Position, Int16Mat3x3 Orientation, byte DaggerType) : IEvent;
