using System.Collections.Generic;

namespace DevilDaggersWebsite.Caches.ModArchive
{
	public class ModArchiveCacheData
	{
		public long FileSize { get; set; }
		public long FileSizeExtracted { get; set; }
		public List<ModBinaryCacheData> Binaries { get; set; } = new();
	}
}
