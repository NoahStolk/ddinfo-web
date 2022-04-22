namespace DevilDaggersInfo.Core.Replay.Events;

public readonly record struct SquidSpawnEvent(int EntityId, SquidType SquidType, int A, Vector3 Position, Vector3 Direction, float RotationInRadians) : IEvent
{
	public void Write(BinaryWriter bw)
	{
		bw.Write((byte)0x00);
		bw.Write((byte)(SquidType switch
		{
			SquidType.Squid1 => 0x03,
			SquidType.Squid2 => 0x04,
			SquidType.Squid3 => 0x05,
			_ => throw new InvalidOperationException($"Invalid {nameof(SquidType)} '{SquidType}'."),
		}));

		bw.Write(A);
		bw.Write(Position);
		bw.Write(Direction);
		bw.Write(RotationInRadians);
	}
}
