namespace DevilDaggersInfo.Core.Mod.FileHandling;

public interface IFileHandler
{
	int HeaderSize { get; }

	byte[] Compile(byte[] buffer);

	byte[] Extract(byte[] buffer);
}
