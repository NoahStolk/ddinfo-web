using DevilDaggersWebsite.Code.Api;
using DevilDaggersWebsite.Code.PageModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Mime;

namespace DevilDaggersWebsite.Pages.Api
{
	[ApiFunction(Description = "Returns website-related statistics.", ReturnType = MediaTypeNames.Application.Json)]
	public class GetWebStatsModel : ApiPageModel
	{
		public FileResult OnGet(bool formatted = false) => JsonFile(ApiFunctions.GetWebStats(), formatted ? Formatting.Indented : Formatting.None);
	}
}