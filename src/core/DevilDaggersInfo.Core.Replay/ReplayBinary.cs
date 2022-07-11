using DevilDaggersInfo.Core.Spawnset;

namespace DevilDaggersInfo.Core.Replay;

public class ReplayBinary
{
	public ReplayBinary(byte[] contents)
	{
		using MemoryStream ms = new(contents);
		using BinaryReader br = new(ms);

		Header = ReplayBinaryHeader.CreateFromBinaryReader(br);

		int compressedDataLength = br.ReadInt32();
		CompressedEvents = br.ReadBytes(compressedDataLength);
		EventsPerTick = ReplayEventsParser.ParseCompressedEvents(CompressedEvents);
	}

	public ReplayBinary(ReplayBinaryHeader header, byte[] compressedEvents)
	{
		Header = header;
		CompressedEvents = compressedEvents;
		EventsPerTick = ReplayEventsParser.ParseCompressedEvents(CompressedEvents);
	}

	public ReplayBinaryHeader Header { get; }
	public byte[] CompressedEvents { get; }
	public List<List<IEvent>> EventsPerTick { get; }

	public static ReplayBinary CreateDefault()
	{
		SpawnsetBinary spawnset = SpawnsetBinary.CreateDefault();
		byte[] spawnsetBuffer = spawnset.ToBytes();
		ReplayBinaryHeader header = new(
			version: 1,
			timestampSinceGameRelease: 0, // TODO: Convert current time to timestamp.
			time: 0,
			startTime: 0,
			daggersFired: 0,
			deathType: 0,
			gems: 0,
			daggersHit: 0,
			kills: 0,
			playerId: 0,
			username: string.Empty,
			spawnsetBuffer: spawnsetBuffer);

		return new(
			header: header,
			compressedEvents: ReplayEventsParser.CompileEvents(new List<IEvent> { new EndEvent() }));
	}

	public byte[] Compile()
	{
		using MemoryStream ms = new();
		using BinaryWriter bw = new(ms);

		bw.Write(Header.ToBytes());
		bw.Write(CompressedEvents.Length);
		bw.Write(CompressedEvents);

		return ms.ToArray();
	}
}
