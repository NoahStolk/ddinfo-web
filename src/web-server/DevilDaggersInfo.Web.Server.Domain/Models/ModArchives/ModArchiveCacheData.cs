using DevilDaggersInfo.Types.Core;
using DevilDaggersInfo.Types.Web;

namespace DevilDaggersInfo.Web.Server.Domain.Models.ModArchives;

public class ModArchiveCacheData
{
	public long FileSize { get; set; }
	public long FileSizeExtracted { get; set; }
	public List<ModBinaryCacheData> Binaries { get; set; } = new();

	public ModTypes GetModTypes()
	{
		ModBinaryCacheData? ddBinary = Binaries.Find(mbcd => mbcd.ModBinaryType == ModBinaryType.Dd);

		ModTypes modTypes = ModTypes.None;
		if (Binaries.Any(mbcd => mbcd.ModBinaryType == ModBinaryType.Audio))
			modTypes |= ModTypes.Audio;
		if (ddBinary?.Chunks.Any(mccd => mccd.AssetType == AssetType.Shader) == true)
			modTypes |= ModTypes.Shader;
		if (ddBinary?.Chunks.Any(mccd => mccd.AssetType == AssetType.ObjectBinding || mccd.AssetType == AssetType.Mesh) == true)
			modTypes |= ModTypes.Mesh;
		if (ddBinary?.Chunks.Any(mccd => mccd.AssetType == AssetType.Texture) == true)
			modTypes |= ModTypes.Texture;

		return modTypes;
	}

	public bool ContainsProhibitedAssets()
		=> Binaries.Any(mbcd => mbcd.ContainsProhibitedAssets());
}
