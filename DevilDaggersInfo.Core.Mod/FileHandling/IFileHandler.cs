namespace DevilDaggersInfo.Core.Mod.FileHandling;

internal interface IFileHandler
{
	int HeaderSize { get; }

	byte[] Compile(byte[] buffer);

	byte[] Extract(byte[] buffer);
}
