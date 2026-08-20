using DevilDaggersInfo.Web.ApiSpec.Ddae.Assets;
using DevilDaggersInfo.Web.Server.Utils.AssetInfo;
using Microsoft.AspNetCore.Mvc;

namespace DevilDaggersInfo.Web.Server.Controllers.Ddae;

[Route("api/ddae/assets")]
[ApiController]
public sealed class AssetsController : ControllerBase
{
	[HttpGet("info")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public ActionResult<Dictionary<string, List<GetAssetInfo>>> GetAssetInfo()
	{
		List<GetAssetInfo> audioAudio = [.. AudioAudio.All.Select(ConvertAssetInfo)];
		List<GetAssetInfo> coreShaders = [.. CoreShaders.All.Select(ConvertAssetInfo)];
		List<GetAssetInfo> ddMeshes = [.. DdMeshes.All.Select(ConvertAssetInfo)];
		List<GetAssetInfo> ddObjectBindings = [.. DdObjectBindings.All.Select(ConvertAssetInfo)];
		List<GetAssetInfo> ddShaders = [.. DdShaders.All.Select(ConvertAssetInfo)];
		List<GetAssetInfo> ddTextures = [.. DdTextures.All.Select(ConvertAssetInfo)];

		return new Dictionary<string, List<GetAssetInfo>>
		{
			["audioAudio"] = audioAudio,
			["coreShaders"] = coreShaders,
			["ddModels"] = ddMeshes, // Use old naming for legacy asset editor.
			["ddModelBindings"] = ddObjectBindings, // Use old naming for legacy asset editor.
			["ddShaders"] = ddShaders,
			["ddTextures"] = ddTextures,
		};

		static GetAssetInfo ConvertAssetInfo(AssetInfoEntry assetInfoEntry)
		{
			return new GetAssetInfo
			{
				Description = assetInfoEntry.Description,
				Name = assetInfoEntry.Name,
				Tags = assetInfoEntry.Tags,
			};
		}
	}
}
