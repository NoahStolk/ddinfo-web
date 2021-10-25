using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using Io = System.IO;

namespace DevilDaggersWebsite.Api
{
	[Route("api/assets")]
	[ApiController]
	public class AssetsController : ControllerBase
	{
		private readonly IWebHostEnvironment _environment;

		public AssetsController(IWebHostEnvironment environment)
		{
			_environment = environment;
		}

		[ApiExplorerSettings(IgnoreApi = true)]
		[Obsolete("Use " + nameof(GetAssetInfoForDdae) + " instead. This is still in use by DDAE 1.0.0.0.")]
		[HttpGet("info")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public ActionResult<Dictionary<string, List<Dto.AssetInfo>>> GetAssetInfo()
			=> GetAssetInfoPrivate();

		[HttpGet("ddae/info")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public ActionResult<Dictionary<string, List<Dto.AssetInfo>>> GetAssetInfoForDdae()
			=> GetAssetInfoPrivate();

		private ActionResult<Dictionary<string, List<Dto.AssetInfo>>> GetAssetInfoPrivate()
		{
			return Io.Directory.GetFiles(Io.Path.Combine(_environment.WebRootPath, "asset-info"))
				.Select(p =>
				{
					string fileName = Io.Path.GetFileNameWithoutExtension(p);
					List<Dto.AssetInfo> assetInfo = JsonConvert.DeserializeObject<List<Dto.AssetInfo>?>(Io.File.ReadAllText(p)) ?? throw new($"Could not deserialize asset info from file '{fileName}'.");
					return (fileName, assetInfo);
				}).
				ToDictionary(t => t.fileName, t => t.assetInfo);
		}
	}
}
