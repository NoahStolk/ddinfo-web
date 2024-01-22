using DevilDaggersInfo.Core.Asset;
using DevilDaggersInfo.Core.Mod;
using DevilDaggersInfo.Web.Server.Domain.Entities.Enums;

namespace DevilDaggersInfo.Web.Server.Domain.Models.ModArchives;

public class ModArchiveCacheData
{
	public long FileSize { get; set; }
	public long FileSizeExtracted { get; set; }
	public List<ModBinaryCacheData> Binaries { get; set; } = [];

	public ModTypes ModTypes()
	{
		ModBinaryCacheData? ddBinary = Binaries.Find(b => b.ModBinaryType == ModBinaryType.Dd);

		ModTypes modTypes = Entities.Enums.ModTypes.None;
		if (Binaries.Exists(b => b.ModBinaryType == ModBinaryType.Audio))
			modTypes |= Entities.Enums.ModTypes.Audio;
		if (ddBinary?.TocEntries.Exists(c => c.AssetType == AssetType.Shader) == true)
			modTypes |= Entities.Enums.ModTypes.Shader;
		if (ddBinary?.TocEntries.Exists(c => c.AssetType == AssetType.ObjectBinding || c.AssetType == AssetType.Mesh) == true)
			modTypes |= Entities.Enums.ModTypes.Mesh;
		if (ddBinary?.TocEntries.Exists(c => c.AssetType == AssetType.Texture) == true)
			modTypes |= Entities.Enums.ModTypes.Texture;

		return modTypes;
	}

	public bool ContainsProhibitedAssets()
	{
		return Binaries.Exists(b => b.ContainsProhibitedAssets());
	}
}
