namespace DevilDaggersInfo.Core.Mod.FileHandling;

public interface IFileHandler
{
	int HeaderSize { get; }

	byte[] Compile(byte[] fileBuffer);

	byte[] Extract(byte[] binaryBuffer);
}
