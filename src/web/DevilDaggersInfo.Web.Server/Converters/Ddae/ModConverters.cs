using DevilDaggersInfo.Web.Server.InternalModels.Mods;
using DdaeApi = DevilDaggersInfo.Api.Ddae.Mods;

namespace DevilDaggersInfo.Web.Server.Converters.Ddae;

public static class ModConverters
{
	public static DdaeApi.GetModDdae ToGetModDdae(this ModEntity mod, ModFileSystemData modFileSystemData) => new()
	{
		Authors = mod.PlayerMods.ConvertAll(pm => pm.Player.PlayerName),
		ContainsProhibitedAssets = modFileSystemData.ModArchive?.ContainsProhibitedAssets(),
		HtmlDescription = mod.HtmlDescription,
		IsHostedOnDdInfo = modFileSystemData.ModArchive != null,
		LastUpdated = mod.LastUpdated,
		ModArchive = modFileSystemData.ModArchive == null ? null : new()
		{
			Binaries = modFileSystemData.ModArchive.Binaries.ConvertAll(b => new DdaeApi.GetModBinaryDdae
			{
				ModBinaryType = b.ModBinaryType.ToApi(),
				Name = b.Name,
				Size = b.Size,
			}),
			FileSize = modFileSystemData.ModArchive.FileSize,
			FileSizeExtracted = modFileSystemData.ModArchive.FileSizeExtracted,
		},
		AssetModTypes = (modFileSystemData.ModArchive?.GetModTypes() ?? mod.ModTypes).ToApi(),
		Name = mod.Name,
		ScreenshotFileNames = modFileSystemData.ScreenshotFileNames ?? new(),
		TrailerUrl = mod.TrailerUrl,
	};

	// TODO: Source-generate this.
	private static DdaeApi.ModBinaryType ToApi(this ModBinaryType modBinaryType) => modBinaryType switch
	{
		ModBinaryType.Audio => DdaeApi.ModBinaryType.Audio,
		ModBinaryType.Dd => DdaeApi.ModBinaryType.Dd,
		_ => throw new NotSupportedException($"Mod binary type '{modBinaryType}' cannot be converted to a DDAE API model."),
	};

	// TODO: Source-generate this. For now just cast the underlying integer.
	private static DdaeApi.ModTypes ToApi(this ModTypes modTypes) => (DdaeApi.ModTypes)modTypes;
}
