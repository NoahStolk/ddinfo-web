using CoreBase.Services;
using DevilDaggersWebsite.Code.API;
using DevilDaggersWebsite.Code.PageModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Net.Mime;

namespace DevilDaggersWebsite.Pages.API
{
	[ApiFunction(Description = "Returns the world record found in the leaderboard history section of the site at the time of the given date parameter. Returns all the world records if no date parameter was specified or if the parameter was incorrect.", ReturnType = MediaTypeNames.Application.Json)]
	public class GetWorldRecordsModel : ApiPageModel
	{
		private readonly ICommonObjects _commonObjects;

		public GetWorldRecordsModel(ICommonObjects commonObjects)
		{
			_commonObjects = commonObjects;
		}

		public FileResult OnGet(DateTime? date = null, bool formatted = false) => JsonFile(ApiFunctions.GetWorldRecords(_commonObjects, date), formatted ? Formatting.Indented : Formatting.None);
	}
}