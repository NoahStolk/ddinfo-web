namespace DevilDaggersInfo.Core.Mod.FileHandling;

public interface IFileHandler
{
	int HeaderSize { get; }

	byte[] ToBinary(byte[] fileBuffer);

	byte[] ToFile(byte[] binaryBuffer);
}
