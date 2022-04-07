using DevilDaggersInfo.Core.Replay.Enums;
using System.Numerics;

namespace DevilDaggersInfo.Core.Replay.Events;

public readonly record struct SpiderSpawnEvent(int EntityId, SpiderType SpiderType, Vector3 Position) : IEntityEvent;
