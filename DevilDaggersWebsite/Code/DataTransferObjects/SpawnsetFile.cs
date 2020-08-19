using DevilDaggersCore.Spawnsets;

namespace DevilDaggersWebsite.Code.DataTransferObjects
{
	public class SpawnsetFile
	{
#pragma warning disable CA1051 // Do not declare visible instance fields
#pragma warning disable SA1307 // Accessible fields should begin with upper-case letter
#pragma warning disable SA1401 // Fields should be private
		public SpawnsetFileSettings settings;

		public SpawnsetData spawnsetData;
#pragma warning restore CA1051 // Do not declare visible instance fields
#pragma warning restore SA1307 // Accessible fields should begin with upper-case letter
#pragma warning restore SA1401 // Fields should be private

		public string Path { get; set; }

		public string FileName => System.IO.Path.GetFileName(Path);

		public string Name => GetName(FileName);

		public string Author => GetAuthor(FileName);

		public bool HasCustomLeaderboard { get; set; }

		public static string GetName(string fileName) => fileName.Substring(0, fileName.LastIndexOf('_'));

		public static string GetAuthor(string fileName) => fileName.Substring(fileName.LastIndexOf('_') + 1);
	}
}