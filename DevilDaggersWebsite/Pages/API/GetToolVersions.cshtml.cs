using CoreBase.Services;
using DevilDaggersWebsite.Code.API;
using DevilDaggersWebsite.Code.PageModels;
using DevilDaggersWebsite.Code.Utils;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Mime;

namespace DevilDaggersWebsite.Pages.API
{
	[ApiFunction(Description = "Returns the list of tools including their version number.", ReturnType = MediaTypeNames.Application.Json)]
	public class GetToolVersionsModel : ApiPageModel
	{
		public FileResult OnGet(bool formatted = false)
		{
			return JsonFile(ToolUtils.Tools, formatted ? Formatting.Indented : Formatting.None);
		}
	}
}