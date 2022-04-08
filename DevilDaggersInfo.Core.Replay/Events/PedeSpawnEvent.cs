namespace DevilDaggersInfo.Core.Replay.Events;

public readonly record struct PedeSpawnEvent(int EntityId, PedeType PedeType, Vector3 Position) : IEvent
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

		bw.Write(0);
		bw.Write(Position);
		bw.Write(Vector3.Zero);
		bw.Write(Vector3.Zero);
		bw.Write(Vector3.Zero);
		bw.Write(Vector3.Zero);
	}
}
