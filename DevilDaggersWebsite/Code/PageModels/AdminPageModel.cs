using CoreBase3.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;

namespace DevilDaggersWebsite.Code.PageModels
{
	public abstract class AdminPageModel : PageModel
	{
		protected readonly ICommonObjects commonObjects;

		protected AdminPageModel(ICommonObjects commonObjects)
		{
			this.commonObjects = commonObjects;

			if (!IsAuthenticated)
				throw new Exception("Unauthorized");
			//return RedirectToPage("/Error/404");
		}

		public string Password => commonObjects.HttpContextAccessor.HttpContext.Request.Query["password"];

		private bool IsAuthenticated => Password == $"1234567890plol{DateTime.Now.Day + DateTime.Now.Month}";

		protected void Authenticate()
		{
			if (!IsAuthenticated)
				throw new Exception("Unauthorized");
			//return RedirectToPage("/Error/404");
		}
	}
}