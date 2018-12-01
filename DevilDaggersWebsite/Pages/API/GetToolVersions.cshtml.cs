using CoreBase.Services;
using DevilDaggersWebsite.Models.API;
using DevilDaggersWebsite.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Net.Mime;
using System.Text;

namespace DevilDaggersWebsite.Pages.API
{
	[ApiFunction(Description = "Returns the list of tools including their version number.", ReturnType = MediaTypeNames.Application.Json)]
	public class GetToolVersionsModel : PageModel
	{
		private readonly ICommonObjects _commonObjects;

		public GetToolVersionsModel(ICommonObjects commonObjects)
		{
			_commonObjects = commonObjects;
		}

		public FileResult OnGet()
		{
			string jsonResult = JsonConvert.SerializeObject(ToolUtils.Tools, Formatting.Indented);
			return File(Encoding.UTF8.GetBytes(jsonResult), MediaTypeNames.Application.Json, $"{GetType().Name.Replace("Model", "")}.json");
		}
	}
}