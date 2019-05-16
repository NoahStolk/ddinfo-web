using DevilDaggersCore.Spawnset;
using Newtonsoft.Json;
using System;
using System.IO;

namespace DevilDaggersWebsite.Models.Spawnset
{
	[JsonObject(MemberSerialization.OptIn)]
	public class SpawnsetFile
	{
		public string Path { get; set; }

		public string FileName => System.IO.Path.GetFileName(Path);
		public DateTime LastUpdated => new FileInfo(Path).LastWriteTime;
		public SpawnsetData SpawnData
		{
			get
			{
				using (FileStream fs = new FileStream(Path, FileMode.Open, FileAccess.Read))
				{
					if (DevilDaggersCore.Spawnset.Spawnset.TryGetSpawnData(fs, out SpawnsetData spawnData))
						return spawnData;

					throw new Exception($"Could not retrieve spawn data for spawnset: {FileName}");
				}
			}
		}

		[JsonProperty]
		public string Name => GetName(FileName);
		[JsonProperty]
		public string Author => GetAuthor(FileName);

		public SpawnsetFile(string path)
		{
			Path = path;
		}

		public static string GetName(string fileNameOrPath)
		{
			return fileNameOrPath.Substring(0, fileNameOrPath.LastIndexOf('_'));
		}

		public static string GetAuthor(string fileNameOrPath)
		{
			return fileNameOrPath.Substring(fileNameOrPath.LastIndexOf('_') + 1);
		}
	}
}