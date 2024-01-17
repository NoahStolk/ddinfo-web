using DevilDaggersInfo.Web.ApiSpec.Ddae.Assets;
using DevilDaggersInfo.Web.Server.Domain.Models.FileSystem;
using DevilDaggersInfo.Web.Server.Domain.Services.Inversion;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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

	[Obsolete("Support for DDAE 1.4.0 will be dropped.")]
	[HttpGet("/api/assets/ddae/info")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public ActionResult<Dictionary<string, List<GetAssetInfo>>> GetAssetInfoObsolete()
	{
		return GetAssetInfoImpl();
	}

	[HttpGet("info")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public ActionResult<Dictionary<string, List<GetAssetInfo>>> GetAssetInfo()
	{
		return GetAssetInfoImpl();
	}

	private ActionResult<Dictionary<string, List<GetAssetInfo>>> GetAssetInfoImpl()
	{
		return Directory.GetFiles(_fileSystemService.GetPath(DataSubDirectory.AssetInfo))
			.Select(p =>
			{
				string fileName = Path.GetFileNameWithoutExtension(p);
				List<GetAssetInfo> assetInfo = JsonConvert.DeserializeObject<List<GetAssetInfo>?>(IoFile.ReadAllText(p)) ?? throw new InvalidOperationException($"Could not deserialize asset info from file '{fileName}'.");
				return (fileName, assetInfo);
			}).
			ToDictionary(t => t.fileName, t => t.assetInfo);
	}
}
