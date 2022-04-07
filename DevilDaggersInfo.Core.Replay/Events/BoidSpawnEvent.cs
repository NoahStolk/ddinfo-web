using DevilDaggersInfo.Core.Replay.Enums;
using DevilDaggersInfo.Core.Replay.Numerics;

namespace DevilDaggersInfo.Core.Replay.Events;

public readonly record struct BoidSpawnEvent(int EntityId, int SpawnerId, BoidType BoidType, Int16Vec3 Position, float Speed) : IEntityEvent;
