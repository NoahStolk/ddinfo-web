using DevilDaggersCore.Game;
using DevilDaggersWebsite.Code.API;
using DevilDaggersWebsite.Code.PageModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Mime;

namespace DevilDaggersWebsite.Pages.API
{
	[ApiFunction(Description = "Returns all game versions and their release dates.", ReturnType = MediaTypeNames.Application.Json)]
	public class GetGameVersionsModel : ApiPageModel
	{
		public FileResult OnGet(bool formatted = false)
		{
			return JsonFile(GameInfo.GameVersions, formatted ? Formatting.Indented : Formatting.None);
		}
	}
}