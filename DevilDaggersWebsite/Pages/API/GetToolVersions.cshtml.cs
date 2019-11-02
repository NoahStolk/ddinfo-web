using DevilDaggersWebsite.Code.API;
using DevilDaggersWebsite.Code.PageModels;
using DevilDaggersWebsite.Code.Utils;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Mime;

namespace DevilDaggersWebsite.Pages.API
{
	[ApiFunction(Description = "Returns the list of available tools on the website.", ReturnType = MediaTypeNames.Application.Json, DeprecationMessage = "Use GetTools instead.")]
	public class GetToolVersionsModel : ApiPageModel
	{
		public FileResult OnGet(bool formatted = false)
		{
			return JsonFile(ToolList.Tools, formatted ? Formatting.Indented : Formatting.None);
		}
	}
}