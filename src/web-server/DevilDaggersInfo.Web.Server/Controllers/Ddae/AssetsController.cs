using DevilDaggersInfo.Api.Ddae.Assets;
using DevilDaggersInfo.Web.Server.Domain.Models.FileSystem;
using DevilDaggersInfo.Web.Server.Domain.Services;

namespace DevilDaggersInfo.Web.Server.Controllers.Ddae;

[Route("api/ddae/assets")]
[ApiController]
public class AssetsController : ControllerBase
{
	private readonly IFileSystemService _fileSystemService;

	public AssetsController(IFileSystemService fileSystemService)
	{
		_fileSystemService = fileSystemService;
	}

	[HttpGet("/api/assets/ddae/info")]
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
