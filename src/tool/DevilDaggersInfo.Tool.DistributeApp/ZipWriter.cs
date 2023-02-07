using System.IO.Compression;

namespace DevilDaggersInfo.Tool.DistributeApp;

public static class ZipWriter
{
	public static void ZipAndDelete(string zipPath, string dirPath)
	{
		Console.WriteLine($"Deleting previous .zip file '{zipPath}' if present");
		File.Delete(zipPath);

		Console.WriteLine($"Creating '{zipPath}' from temporary directory '{dirPath}'");
		ZipFile.CreateFromDirectory(dirPath, zipPath);

		Console.WriteLine($"Deleting temporary directory '{dirPath}'");
		Directory.Delete(dirPath, true);
	}
}
