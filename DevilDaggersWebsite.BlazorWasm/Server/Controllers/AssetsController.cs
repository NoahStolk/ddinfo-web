using DevilDaggersWebsite.BlazorWasm.Server.Controllers.Attributes;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using Io = System.IO;

namespace DevilDaggersWebsite.BlazorWasm.Server.Controllers
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

		[HttpGet("info")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[EndpointConsumer(EndpointConsumers.Ddae)]
		public ActionResult<Dictionary<string, List<Dto.AssetInfo>>> GetAssetInfo()
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
