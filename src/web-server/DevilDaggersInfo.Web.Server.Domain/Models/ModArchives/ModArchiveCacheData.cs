using DevilDaggersInfo.Core.Asset;
using DevilDaggersInfo.Types.Core.Mods;
using DevilDaggersInfo.Web.Server.Domain.Entities.Enums;

namespace DevilDaggersInfo.Web.Server.Domain.Models.ModArchives;

public class ModArchiveCacheData
{
	public long FileSize { get; set; }
	public long FileSizeExtracted { get; set; }
	public List<ModBinaryCacheData> Binaries { get; set; } = new();

	public ModTypes ModTypes()
	{
		ModBinaryCacheData? ddBinary = Binaries.Find(b => b.ModBinaryType == ModBinaryType.Dd);

		ModTypes modTypes = Entities.Enums.ModTypes.None;
		if (Binaries.Any(b => b.ModBinaryType == ModBinaryType.Audio))
			modTypes |= Entities.Enums.ModTypes.Audio;
		if (ddBinary?.Chunks.Any(c => c.AssetType == AssetType.Shader) == true)
			modTypes |= Entities.Enums.ModTypes.Shader;
		if (ddBinary?.Chunks.Any(c => c.AssetType == AssetType.ObjectBinding || c.AssetType == AssetType.Mesh) == true)
			modTypes |= Entities.Enums.ModTypes.Mesh;
		if (ddBinary?.Chunks.Any(c => c.AssetType == AssetType.Texture) == true)
			modTypes |= Entities.Enums.ModTypes.Texture;

		return modTypes;
	}

	public bool ContainsProhibitedAssets()
		=> Binaries.Any(b => b.ContainsProhibitedAssets());
}
