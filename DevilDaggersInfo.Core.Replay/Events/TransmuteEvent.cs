using DevilDaggersInfo.Core.Replay.Numerics;

namespace DevilDaggersInfo.Core.Replay.Events;

public readonly record struct TransmuteEvent(int EntityId, Int16Vec3 A, Int16Vec3 B, Int16Vec3 C, Int16Vec3 D) : IEntityEvent;
