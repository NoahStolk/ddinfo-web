using DevilDaggersInfo.Common.Exceptions;
using DevilDaggersInfo.Core.Replay.Events.Interfaces;

namespace DevilDaggersInfo.Core.Replay.Events;

public readonly record struct SpiderSpawnEvent(int EntityId, SpiderType SpiderType, int A, Vector3 Position) : IEntitySpawnEvent
{
	public EntityType EntityType => SpiderType switch
	{
		SpiderType.Spider1 => EntityType.Spider1,
		SpiderType.Spider2 => EntityType.Spider2,
		_ => throw new InvalidEnumConversionException(SpiderType),
	};

	public void Write(BinaryWriter bw)
	{
		bw.Write((byte)0x00);
		bw.Write((byte)(SpiderType switch
		{
			SpiderType.Spider1 => 0x08,
			SpiderType.Spider2 => 0x09,
			_ => throw new InvalidEnumConversionException(SpiderType),
		}));

		bw.Write(A);
		bw.Write(Position);
	}
}
