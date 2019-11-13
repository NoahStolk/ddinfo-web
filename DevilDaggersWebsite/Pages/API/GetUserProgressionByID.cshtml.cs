using CoreBase.Services;
using DevilDaggersWebsite.Code.API;
using DevilDaggersWebsite.Code.PageModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Mime;

namespace DevilDaggersWebsite.Pages.API
{
	[ApiFunction(Description = "Returns the user progression found in the leaderboard history section of the site corresponding to the given userID parameter.", ReturnType = MediaTypeNames.Application.Json)]
	public class GetUserProgressionByIDModel : ApiPageModel
	{
		private readonly ICommonObjects _commonObjects;

		public GetUserProgressionByIDModel(ICommonObjects commonObjects)
		{
			_commonObjects = commonObjects;
		}

		public FileResult OnGet(int userID = default, bool formatted = false) => JsonFile(ApiFunctions.GetUserProgression(_commonObjects, userID), formatted ? Formatting.Indented : Formatting.None);
	}
}