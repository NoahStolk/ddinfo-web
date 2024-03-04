using DevilDaggersInfo.Web.ApiSpec.Ddae.Assets;
using DevilDaggersInfo.Web.Server.Utils.AssetInfo;
using Microsoft.AspNetCore.Mvc;

namespace DevilDaggersInfo.Web.Server.Controllers.Ddae;

[Route("api/ddae/assets")]
[ApiController]
public class AssetsController : ControllerBase
{
	[HttpGet("info")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public ActionResult<Dictionary<string, List<GetAssetInfo>>> GetAssetInfo()
	{
		List<GetAssetInfo> audioAudio = AudioAudio.All.Select(ConvertAssetInfo).ToList();
		List<GetAssetInfo> coreShaders = CoreShaders.All.Select(ConvertAssetInfo).ToList();
		List<GetAssetInfo> ddMeshes = DdMeshes.All.Select(ConvertAssetInfo).ToList();
		List<GetAssetInfo> ddObjectBindings = DdObjectBindings.All.Select(ConvertAssetInfo).ToList();
		List<GetAssetInfo> ddShaders = DdShaders.All.Select(ConvertAssetInfo).ToList();
		List<GetAssetInfo> ddTextures = DdTextures.All.Select(ConvertAssetInfo).ToList();

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
			return new()
			{
				Description = assetInfoEntry.Description,
				Name = assetInfoEntry.Name,
				Tags = assetInfoEntry.Tags,
			};
		}
	}
}
