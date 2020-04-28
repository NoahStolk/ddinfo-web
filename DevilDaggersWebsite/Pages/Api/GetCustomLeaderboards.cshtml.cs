using DevilDaggersWebsite.Code.Api;
using DevilDaggersWebsite.Code.Database;
using DevilDaggersWebsite.Code.PageModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Mime;

namespace DevilDaggersWebsite.Pages.Api
{
	[ApiFunction(Description = "Returns the list of all available custom leaderboards on the site.", ReturnType = MediaTypeNames.Application.Json)]
	public class GetCustomLeaderboardsModel : ApiPageModel
	{
		private readonly ApplicationDbContext context;

		public GetCustomLeaderboardsModel(ApplicationDbContext context)
		{
			this.context = context;
		}

		public FileResult OnGet(bool formatted = false) => JsonFile(ApiFunctions.GetCustomLeaderboards(context), formatted ? Formatting.Indented : Formatting.None);
	}
}