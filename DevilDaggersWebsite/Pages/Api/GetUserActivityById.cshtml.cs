using CoreBase3.Services;
using DevilDaggersWebsite.Code.Api;
using DevilDaggersWebsite.Code.PageModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Mime;

namespace DevilDaggersWebsite.Pages.Api
{
	[ApiFunction(Description = "Returns the current user activity corresponding to the given userId parameter.", ReturnType = MediaTypeNames.Application.Json)]
	public class GetUserActivityByIdModel : ApiPageModel
	{
		private readonly ICommonObjects commonObjects;

		public GetUserActivityByIdModel(ICommonObjects commonObjects)
		{
			this.commonObjects = commonObjects;
		}

		public FileResult OnGet(int userId = default, bool formatted = false) => JsonFile(ApiFunctions.GetUserActivity(commonObjects, userId), formatted ? Formatting.Indented : Formatting.None);
	}
}