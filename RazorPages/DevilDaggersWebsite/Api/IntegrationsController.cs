using DevilDaggersWebsite.Dto;
using DevilDaggersWebsite.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace DevilDaggersWebsite.Api
{
	[Route("api/integrations")]
	[ApiController]
	public class IntegrationsController : ControllerBase
	{
		private readonly ApplicationDbContext _dbContext;

		public IntegrationsController(ApplicationDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		[HttpGet("ddstats-rust")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public ActionResult<DdstatsRustAccessInfo> GetDdstatsRustAccessInfo()
		{
			Entities.Tool? ddstatsRust = _dbContext.Tools.AsNoTracking().FirstOrDefault(t => t.Name == "ddstats-rust");
			if (ddstatsRust == null)
				throw new Exception("ddstats-rust not found in database.");

			return new DdstatsRustAccessInfo
			{
				RequiredVersion = Version.Parse(ddstatsRust.RequiredVersionNumber),
			};
		}
	}
}
