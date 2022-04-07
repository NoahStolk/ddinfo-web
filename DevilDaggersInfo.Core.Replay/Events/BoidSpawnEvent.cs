namespace DevilDaggersInfo.Core.Replay.Events;

public readonly record struct BoidSpawnEvent(int EntityId, int SpawnerId, BoidType BoidType, Int16Vec3 Position, float Speed) : IEntityEvent;
