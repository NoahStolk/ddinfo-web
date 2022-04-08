namespace DevilDaggersInfo.Core.Replay.Events;

public readonly record struct PedeSpawnEvent(int EntityId, PedeType PedeType, int A, Vector3 Position, Vector3 B, Matrix3x3 Orientation) : IEvent
{
	public void Write(BinaryWriter bw)
	{
		bw.Write((byte)0x00);
		bw.Write((byte)(PedeType switch
		{
			PedeType.Centipede => 0x07,
			PedeType.Gigapede => 0x0c,
			PedeType.Ghostpede => 0x0f,
			_ => throw new InvalidOperationException($"Invalid {nameof(PedeType)} '{PedeType}'."),
		}));

		bw.Write(A);
		bw.Write(Position);
		bw.Write(B);
		bw.Write(Orientation);
	}
}
