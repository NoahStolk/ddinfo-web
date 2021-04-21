using DevilDaggersWebsite.Caches.ModArchive;
using System.Collections.Generic;

namespace DevilDaggersWebsite.Caches.Mod
{
	public class ModArchiveCacheData
	{
		public long FileSize { get; set; }
		public long FileSizeExtracted { get; set; }
		public List<ModBinaryCacheData> Binaries { get; set; } = new();
	}
}
