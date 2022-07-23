using DevilDaggersInfo.Core.Replay.Events.Interfaces;
using System.IO.Compression;

namespace DevilDaggersInfo.Core.Replay;

public static class ReplayEventsCompiler
{
	public static byte[] CompileEvents(List<IEvent> events)
	{
		using MemoryStream ms = new();
		using BinaryWriter bw = new(ms);

		foreach (IEvent e in events)
			e.Write(bw);

		return Compress(ms.ToArray());
	}

	private static byte[] Compress(byte[] data)
	{
		using MemoryStream memoryStream = new();
		using (DeflateStream deflateStream = new(memoryStream, CompressionLevel.SmallestSize))
		{
			deflateStream.Write(data, 0, data.Length);
		}

		byte[] compressedData = memoryStream.ToArray();

		byte[] compressedDataWithHeader = new byte[2 + compressedData.Length];
		Buffer.BlockCopy(new byte[] { 120, 1 }, 0, compressedDataWithHeader, 0, 2);
		Buffer.BlockCopy(compressedData, 0, compressedDataWithHeader, 2, compressedData.Length);
		return compressedDataWithHeader;
	}
}
