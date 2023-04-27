using DevilDaggersInfo.Core.Replay.Events.Enums;
using DevilDaggersInfo.Core.Replay.Events.Interfaces;
using System.Diagnostics;

namespace DevilDaggersInfo.Core.Replay.Events;

public readonly record struct PedeSpawnEvent(int EntityId, PedeType PedeType, int A, Vector3 Position, Vector3 B, Matrix3x3 Orientation) : IEntitySpawnEvent
{
	public EntityType EntityType => PedeType switch
	{
		PedeType.Centipede => EntityType.Centipede,
		PedeType.Gigapede => EntityType.Gigapede,
		PedeType.Ghostpede => EntityType.Ghostpede,
		_ => throw new UnreachableException(),
	};

	public void Write(BinaryWriter bw)
	{
		bw.Write((byte)0x00);
		bw.Write((byte)(PedeType switch
		{
			PedeType.Centipede => 0x07,
			PedeType.Gigapede => 0x0c,
			PedeType.Ghostpede => 0x0f,
			_ => throw new UnreachableException(),
		}));

		bw.Write(A);
		bw.Write(Position);
		bw.Write(B);
		bw.Write(Orientation);
	}
}
