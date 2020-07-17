using DevilDaggersWebsite.Code.Api;
using DevilDaggersWebsite.Code.PageModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Mime;

namespace DevilDaggersWebsite.Pages.Api
{
	[ApiFunction(Description = "Returns the user progression found in the leaderboard history section of the site corresponding to the given userId parameter.", ReturnType = MediaTypeNames.Application.Json)]
	public class GetUserProgressionByIdModel : ApiPageModel
	{
		private readonly IWebHostEnvironment env;

		public GetUserProgressionByIdModel(IWebHostEnvironment env)
		{
			this.env = env;
		}

		public FileResult OnGet(int userId = default, bool formatted = false) => JsonFile(ApiFunctions.GetUserProgressionById(env, userId), formatted ? Formatting.Indented : Formatting.None);
	}
}