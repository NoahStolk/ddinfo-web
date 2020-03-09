using CoreBase.Services;
using DevilDaggersWebsite.Code.Api;
using DevilDaggersWebsite.Code.PageModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Mime;

namespace DevilDaggersWebsite.Pages.Api
{
	[ApiFunction(Description = "Returns the user progression found in the leaderboard history section of the site corresponding to the given userId parameter.", ReturnType = MediaTypeNames.Application.Json)]
	public class GetUserProgressionByIdModel : ApiPageModel
	{
		private readonly ICommonObjects _commonObjects;

		public GetUserProgressionByIdModel(ICommonObjects commonObjects)
		{
			_commonObjects = commonObjects;
		}

		public FileResult OnGet(int userId = default, bool formatted = false) => JsonFile(ApiFunctions.GetUserProgressionById(_commonObjects, userId), formatted ? Formatting.Indented : Formatting.None);
	}
}