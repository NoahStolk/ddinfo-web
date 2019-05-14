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
		public string Name => FileName.Substring(0, FileName.LastIndexOf('_'));
		[JsonProperty]
		public string Author => FileName.Substring(FileName.LastIndexOf('_') + 1);

		public SpawnsetFile(string path)
		{
			Path = path;
		}
	}
}