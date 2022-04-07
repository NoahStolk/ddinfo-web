namespace DevilDaggersInfo.Core.Replay.Events;

public readonly record struct InitialInputsEvent(byte Left, byte Right, byte Forward, byte Backward, byte Jump, byte Shoot, byte ShootHoming, short MouseX, short MouseY, float LookSpeed) : IEvent;
