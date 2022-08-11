using DevilDaggersInfo.Core.Replay.Events.Interfaces;
using DevilDaggersInfo.Types.Core.Replays;

namespace DevilDaggersInfo.Core.Replay.Events;

public readonly record struct SpiderEggSpawnEvent(int EntityId, int SpawnerEntityId, Vector3 Position, Vector3 TargetPosition) : IEntitySpawnEvent
{
	public EntityType EntityType => EntityType.SpiderEgg;

	public void Write(BinaryWriter bw)
	{
		bw.Write((byte)0x00);
		bw.Write((byte)0x0a);

		bw.Write(SpawnerEntityId);
		bw.Write(Position);
		bw.Write(TargetPosition);
	}
}
