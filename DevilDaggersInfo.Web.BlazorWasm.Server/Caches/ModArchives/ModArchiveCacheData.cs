namespace DevilDaggersInfo.Web.BlazorWasm.Server.Caches.ModArchives;

public class ModArchiveCacheData
{
	public long FileSize { get; set; }
	public long FileSizeExtracted { get; set; }
	public List<ModBinaryCacheData> Binaries { get; set; } = new();

	public ModTypes GetModTypes()
	{
		ModBinaryCacheData? ddBinary = Binaries.Find(md => md.ModBinaryType == ModBinaryType.Dd);

		ModTypes modTypes = ModTypes.None;
		if (Binaries.Any(md => md.ModBinaryType == ModBinaryType.Audio))
			modTypes |= ModTypes.Audio;
		if (Binaries.Any(md => md.ModBinaryType == ModBinaryType.Core) || ddBinary?.Chunks.Any(mad => mad.AssetType == AssetType.Shader) == true)
			modTypes |= ModTypes.Shader;
		if (ddBinary?.Chunks.Any(mad => mad.AssetType == AssetType.ModelBinding || mad.AssetType == AssetType.Model) == true)
			modTypes |= ModTypes.Model;
		if (ddBinary?.Chunks.Any(mad => mad.AssetType == AssetType.Texture) == true)
			modTypes |= ModTypes.Texture;

		return modTypes;
	}

	public bool ContainsProhibitedAssets()
		=> Binaries.Any(md => md.Chunks.Any(mad => mad.IsProhibited));
}
