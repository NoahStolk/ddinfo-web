using DevilDaggersWebsite.Code.Api;
using DevilDaggersWebsite.Code.PageModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Mime;

namespace DevilDaggersWebsite.Pages.Api
{
	[ApiFunction(Description = "Returns the current user activity corresponding to the given userId parameter.", ReturnType = MediaTypeNames.Application.Json)]
	public class GetUserActivityByIdModel : ApiPageModel
	{
		private readonly IWebHostEnvironment env;

		public GetUserActivityByIdModel(IWebHostEnvironment env)
		{
			this.env = env;
		}

		public FileResult OnGet(int userId = default, bool formatted = false) => JsonFile(ApiFunctions.GetUserActivity(env, userId), formatted ? Formatting.Indented : Formatting.None);
	}
}