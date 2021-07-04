using DevilDaggersWebsite.Api.Attributes;
using DevilDaggersWebsite.Dto.Mods;
using DevilDaggersWebsite.Entities;
using DevilDaggersWebsite.Transients;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Net.Mime;
using Io = System.IO;

namespace DevilDaggersWebsite.Api
{
	[Route("api/mods")]
	[ApiController]
	public class ModsController : ControllerBase
	{
		private readonly IWebHostEnvironment _environment;
		private readonly ApplicationDbContext _dbContext;
		private readonly ModHelper _modHelper;

		public ModsController(IWebHostEnvironment environment, ApplicationDbContext dbContext, ModHelper modHelper)
		{
			_environment = environment;
			_dbContext = dbContext;
			_modHelper = modHelper;
		}

		// TODO: Re-route to /public.
		[HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[EndpointConsumer(EndpointConsumers.Ddae)]
		public List<GetPublicMod> GetPublicMods(string? authorFilter = null, string? nameFilter = null, bool? isHostedFilter = null)
			=> _modHelper.GetPublicMods(authorFilter, nameFilter, isHostedFilter);

		// TODO: Remove private.
		[HttpGet("private")]
		//[Authorize(Policies.AssetModsPolicy)]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[EndpointConsumer(EndpointConsumers.Admin)]
		public List<GetMod> GetMods()
		{
			List<AssetMod> mods = _dbContext.AssetMods.AsNoTracking().ToList();

			return mods.ConvertAll(ce => new GetMod
			{
				Id = ce.Id,
				AssetModTypes = ce.AssetModTypes,
				HtmlDescription = ce.HtmlDescription,
				IsHidden = ce.IsHidden,
				LastUpdated = ce.LastUpdated,
				Name = ce.Name,
				PlayerIds = ce.PlayerAssetMods.ConvertAll(pam => pam.PlayerId),
				TrailerUrl = ce.TrailerUrl,
				Url = ce.Url,
			});
		}

		[HttpGet("{modName}/file")]
		[ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[EndpointConsumer(EndpointConsumers.Ddae | EndpointConsumers.Website)]
		public ActionResult GetModFile([Required] string modName)
		{
			if (!_dbContext.AssetMods.Any(am => am.Name == modName))
				return new NotFoundObjectResult(new ProblemDetails { Title = $"Mod '{modName}' was not found." });

			string fileName = $"{modName}.zip";
			string path = Path.Combine("mods", fileName);
			if (!Io.File.Exists(Path.Combine(_environment.WebRootPath, path)))
				return new BadRequestObjectResult(new ProblemDetails { Title = $"Mod file '{fileName}' does not exist." });

			return File(Io.File.ReadAllBytes(Path.Combine(_environment.WebRootPath, path)), MediaTypeNames.Application.Zip, fileName);
		}
	}
}
