using DevilDaggersInfo.Types.Core.Assets;
using DevilDaggersInfo.Types.Core.Mods;
using DevilDaggersInfo.Types.Web;

namespace DevilDaggersInfo.Web.Server.Domain.Models.ModArchives;

public class ModArchiveCacheData
{
	public long FileSize { get; set; }
	public long FileSizeExtracted { get; set; }
	public List<ModBinaryCacheData> Binaries { get; set; } = new();

	public ModTypes ModTypes()
	{
		ModBinaryCacheData? ddBinary = Binaries.Find(b => b.ModBinaryType == ModBinaryType.Dd);

		ModTypes modTypes = Types.Web.ModTypes.None;
		if (Binaries.Any(b => b.ModBinaryType == ModBinaryType.Audio))
			modTypes |= Types.Web.ModTypes.Audio;
		if (ddBinary?.Chunks.Any(c => c.AssetType == AssetType.Shader) == true)
			modTypes |= Types.Web.ModTypes.Shader;
		if (ddBinary?.Chunks.Any(c => c.AssetType == AssetType.ObjectBinding || c.AssetType == AssetType.Mesh) == true)
			modTypes |= Types.Web.ModTypes.Mesh;
		if (ddBinary?.Chunks.Any(c => c.AssetType == AssetType.Texture) == true)
			modTypes |= Types.Web.ModTypes.Texture;

		return modTypes;
	}

	public bool ContainsProhibitedAssets()
		=> Binaries.Any(b => b.ContainsProhibitedAssets());
}
