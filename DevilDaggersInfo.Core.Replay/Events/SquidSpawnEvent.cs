using DevilDaggersInfo.Core.Replay.Enums;
using System.Numerics;

namespace DevilDaggersInfo.Core.Replay.Events;

public readonly record struct SquidSpawnEvent(int EntityId, SquidType SquidType, Vector3 Position, float RotationInRadians) : IEntityEvent;
