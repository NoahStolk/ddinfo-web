using DevilDaggersInfo.Core.Replay.Enums;
using System.Numerics;

namespace DevilDaggersInfo.Core.Replay.Events;

public readonly record struct PedeSpawnEvent(int EntityId, PedeType PedeType, Vector3 Position) : IEntityEvent;
