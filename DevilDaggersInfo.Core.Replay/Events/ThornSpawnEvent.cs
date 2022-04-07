namespace DevilDaggersInfo.Core.Replay.Events;

public readonly record struct ThornSpawnEvent(int EntityId, Vector3 Position, float RotationInRadians) : IEntityEvent;
