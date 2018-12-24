using CoreBase.Services;
using DevilDaggersWebsite.Models.PageModels;
using Microsoft.AspNetCore.Mvc;
using NetBase.Utils;

namespace DevilDaggersWebsite.Pages.Admin
{
	public class IndexModel : AdminPageModel
	{
		private ICommonObjects _commonObjects;

		public string Pass { get; private set; }

		public string BansFileContents { get; set; }
		public string DonatorsFileContents { get; set; }
		public string FlagsFileContents { get; set; }

		public IndexModel(ICommonObjects commonObjects)
		{
			_commonObjects = commonObjects;
		}

		public ActionResult OnGet(string pass)
		{
			if (!Authenticate(pass))
				return RedirectToPage("/Error/404");

			Pass = pass;

			BansFileContents = FileUtils.GetContents(System.IO.Path.Combine(_commonObjects.Env.WebRootPath, "user", "bans"));
			DonatorsFileContents = FileUtils.GetContents(System.IO.Path.Combine(_commonObjects.Env.WebRootPath, "user", "donators"));
			FlagsFileContents = FileUtils.GetContents(System.IO.Path.Combine(_commonObjects.Env.WebRootPath, "user", "flags"));

			return null;
		}
	}
}