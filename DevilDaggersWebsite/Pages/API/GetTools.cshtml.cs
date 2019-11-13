using DevilDaggersWebsite.Code.API;
using DevilDaggersWebsite.Code.PageModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Mime;

namespace DevilDaggersWebsite.Pages.API
{
	[ApiFunction(Description = "Returns the list of available tools on the website.", ReturnType = MediaTypeNames.Application.Json)]
	public class GetToolsModel : ApiPageModel
	{
		public FileResult OnGet(bool formatted = false)
		{
			return JsonFile(ApiFunctions.GetTools(), formatted ? Formatting.Indented : Formatting.None);
		}
	}
}