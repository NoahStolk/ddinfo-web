using DevilDaggersWebsite.Code.Api;
using DevilDaggersWebsite.Code.PageModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Mime;

namespace DevilDaggersWebsite.Pages.Api
{
	[ApiFunction(Description = "Returns all game versions and their release dates.", ReturnType = MediaTypeNames.Application.Json)]
	public class GetGameVersionsModel : ApiPageModel
	{
		public FileResult OnGet(bool formatted = false) => JsonFile(ApiFunctions.GetGameVersions(), formatted ? Formatting.Indented : Formatting.None);
	}
}