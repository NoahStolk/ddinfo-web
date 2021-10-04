using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.Assets;

namespace DevilDaggersInfo.Web.BlazorWasm.Server.Controllers.Public;

[Route("api/assets")]
[ApiController]
public class AssetsController : ControllerBase
{
	private readonly IFileSystemService _fileSystemService;

	public AssetsController(IFileSystemService fileSystemService)
	{
		_fileSystemService = fileSystemService;
	}

	[HttpGet("ddae/info")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public ActionResult<Dictionary<string, List<GetAssetInfo>>> GetAssetInfoForDdae()
	{
		return Directory.GetFiles(_fileSystemService.GetPath(DataSubDirectory.AssetInfo))
			.Select(p =>
			{
				string fileName = Path.GetFileNameWithoutExtension(p);
				List<GetAssetInfo> assetInfo = JsonConvert.DeserializeObject<List<GetAssetInfo>?>(IoFile.ReadAllText(p)) ?? throw new($"Could not deserialize asset info from file '{fileName}'.");
				return (fileName, assetInfo);
			}).
			ToDictionary(t => t.fileName, t => t.assetInfo);
	}
}
