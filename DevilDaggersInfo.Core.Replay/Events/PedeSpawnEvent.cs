namespace DevilDaggersInfo.Core.Replay.Events;

public readonly record struct PedeSpawnEvent(int EntityId, PedeType PedeType, int A, Vector3 Position, Vector3 B, Vector3 C, Vector3 D, Vector3 E) : IEvent
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
		bw.Write(C);
		bw.Write(D);
		bw.Write(E);
	}
}
