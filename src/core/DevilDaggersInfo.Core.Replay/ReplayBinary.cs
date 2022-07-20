using DevilDaggersInfo.Common.Exceptions;

namespace DevilDaggersInfo.Core.Replay;

public class ReplayBinary<TReplayBinaryHeader>
	where TReplayBinaryHeader : IReplayBinaryHeader<TReplayBinaryHeader>
{
	public ReplayBinary(byte[] contents)
	{
		using MemoryStream ms = new(contents);
		using BinaryReader br = new(ms);

		Header = TReplayBinaryHeader.CreateFromBinaryReader(br);

		int compressedDataLength;
		if (TReplayBinaryHeader.UsesLengthPrefixedEvents)
			compressedDataLength = br.ReadInt32();
		else
			compressedDataLength = (int)(contents.Length - br.BaseStream.Position);

		EventsPerTick = ReplayEventsParser.ParseCompressedEvents(br.ReadBytes(compressedDataLength));
		EntityTypes = DetermineEntityTypes(EventsPerTick.SelectMany(e => e).ToList());
	}

	public ReplayBinary(TReplayBinaryHeader header, byte[] compressedEvents)
	{
		Header = header;
		EventsPerTick = ReplayEventsParser.ParseCompressedEvents(compressedEvents);
		EntityTypes = DetermineEntityTypes(EventsPerTick.SelectMany(e => e).ToList());
	}

	public TReplayBinaryHeader Header { get; }
	public List<List<IEvent>> EventsPerTick { get; }
	public List<EntityType> EntityTypes { get; }

	private static List<EntityType> DetermineEntityTypes(List<IEvent> events)
	{
		List<EntityType> entities = new() { EntityType.Player };

		foreach (IEvent e in events)
		{
			EntityType? entityType = e switch
			{
				BoidSpawnEvent bse => bse.BoidType switch
				{
					BoidType.Skull1 => EntityType.Skull1,
					BoidType.Skull2 => EntityType.Skull2,
					BoidType.Skull3 => EntityType.Skull3,
					BoidType.Spiderling => EntityType.Spiderling,
					BoidType.Skull4 => EntityType.Skull4,
					_ => throw new InvalidEnumConversionException(bse.BoidType),
				},
				DaggerSpawnEvent dse => dse.DaggerType switch
				{
					DaggerType.Level1 => EntityType.Level1Dagger,
					DaggerType.Level2 => EntityType.Level2Dagger,
					DaggerType.Level3 => EntityType.Level3Dagger,
					DaggerType.Level3Homing => EntityType.Level3HomingDagger,
					DaggerType.Level4 => EntityType.Level4Dagger,
					DaggerType.Level4Homing => EntityType.Level4HomingDagger,
					DaggerType.Level4HomingSplash => EntityType.Level4HomingSplash,
					_ => throw new InvalidEnumConversionException(dse.DaggerType),
				},
				LeviathanSpawnEvent => EntityType.Leviathan,
				PedeSpawnEvent pse => pse.PedeType switch
				{
					PedeType.Centipede => EntityType.Centipede,
					PedeType.Gigapede => EntityType.Gigapede,
					PedeType.Ghostpede => EntityType.Ghostpede,
					_ => throw new InvalidEnumConversionException(pse.PedeType),
				},
				SpiderEggSpawnEvent => EntityType.SpiderEgg,
				SpiderSpawnEvent sse => sse.SpiderType switch
				{
					SpiderType.Spider1 => EntityType.Spider1,
					SpiderType.Spider2 => EntityType.Spider2,
					_ => throw new InvalidEnumConversionException(sse.SpiderType),
				},
				SquidSpawnEvent sse => sse.SquidType switch
				{
					SquidType.Squid1 => EntityType.Squid1,
					SquidType.Squid2 => EntityType.Squid2,
					SquidType.Squid3 => EntityType.Squid3,
					_ => throw new InvalidEnumConversionException(sse.SquidType),
				},
				ThornSpawnEvent => EntityType.Thorn,
				_ => null,
			};

			if (entityType.HasValue)
				entities.Add(entityType.Value);
		}

		return entities;
	}

	public static ReplayBinary<TReplayBinaryHeader> CreateDefault()
	{
		return new(
			header: TReplayBinaryHeader.CreateDefault(),
			compressedEvents: ReplayEventsParser.CompileEvents(new List<IEvent> { new EndEvent() })); // TODO: Check if this is valid by saving this default empty replay and playing it in DD.
	}

	public byte[] Compile()
	{
		using MemoryStream ms = new();
		using BinaryWriter bw = new(ms);

		bw.Write(Header.ToBytes());

		byte[] compressedEvents = ReplayEventsParser.CompileEvents(EventsPerTick.SelectMany(e => e).ToList());
		bw.Write(compressedEvents.Length);
		bw.Write(compressedEvents);

		return ms.ToArray();
	}
}
