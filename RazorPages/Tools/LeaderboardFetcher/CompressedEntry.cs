using System;

namespace LeaderboardFetcher
{
	public class CompressedEntry
	{
		public uint Time { get; init; }
		public ushort Kills { get; init; }
		public ushort Gems { get; init; }
		public ushort DaggersHit { get; init; }
		public uint DaggersFired { get; init; }
		public byte DeathType { get; init; }

		public byte[] ToBytes()
		{
			byte[] bytes = new byte[15];
			Buffer.BlockCopy(BitConverter.GetBytes(Time), 0, bytes, 0, sizeof(uint));
			Buffer.BlockCopy(BitConverter.GetBytes(Kills), 0, bytes, 4, sizeof(ushort));
			Buffer.BlockCopy(BitConverter.GetBytes(Gems), 0, bytes, 6, sizeof(ushort));
			Buffer.BlockCopy(BitConverter.GetBytes(DaggersHit), 0, bytes, 8, sizeof(ushort));
			Buffer.BlockCopy(BitConverter.GetBytes(DaggersFired), 0, bytes, 10, sizeof(uint));
			Buffer.BlockCopy(BitConverter.GetBytes(DeathType), 0, bytes, 14, sizeof(byte));
			return bytes;
		}
	}
}
