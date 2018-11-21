using CoreBase.Services;
using DevilDaggersWebsite.Utils;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace DevilDaggersWebsite.Pages.API
{
	public class GetToolVersionsModel : PageModel
	{
		private readonly ICommonObjects _commonObjects;

		public string JsonResult { get; set; }

		public GetToolVersionsModel(ICommonObjects commonObjects)
		{
			_commonObjects = commonObjects;
		}

		public void OnGet()
		{
			JsonResult = JsonConvert.SerializeObject(ToolUtils.Tools, Formatting.Indented);
		}
	}
}