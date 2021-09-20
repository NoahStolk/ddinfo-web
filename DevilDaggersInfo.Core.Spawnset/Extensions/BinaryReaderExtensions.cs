namespace DevilDaggersInfo.Core.Spawnset.Extensions;

public static class BinaryReaderExtensions
{
	public static void Seek(this BinaryReader binaryReader, long count)
		=> binaryReader.BaseStream.Seek(count, SeekOrigin.Current);
}
