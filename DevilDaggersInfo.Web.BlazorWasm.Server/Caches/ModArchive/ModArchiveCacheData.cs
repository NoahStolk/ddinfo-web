namespace DevilDaggersInfo.Web.BlazorWasm.Server.Caches.ModArchive;

public class ModArchiveCacheData
{
	public long FileSize { get; set; }
	public long FileSizeExtracted { get; set; }
	public List<ModBinaryCacheData> Binaries { get; set; } = new();
}
