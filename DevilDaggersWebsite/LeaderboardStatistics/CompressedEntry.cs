using System;

namespace DevilDaggersWebsite.LeaderboardStatistics
{
	public class CompressedEntry
	{
		public uint Time { get; set; }
		public ushort Kills { get; set; }
		public ushort Gems { get; set; }
		public ushort DaggersHit { get; set; }
		public uint DaggersFired { get; set; }
		public byte DeathType { get; set; }

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

		public static CompressedEntry FromBytes(byte[] bytes) => new CompressedEntry
		{
			Time = BitConverter.ToUInt32(bytes, 0),
			Kills = BitConverter.ToUInt16(bytes, 4),
			Gems = BitConverter.ToUInt16(bytes, 6),
			DaggersHit = BitConverter.ToUInt16(bytes, 8),
			DaggersFired = BitConverter.ToUInt32(bytes, 10),
			DeathType = bytes[14],
		};
	}
}