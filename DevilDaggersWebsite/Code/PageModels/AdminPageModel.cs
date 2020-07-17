using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;

namespace DevilDaggersWebsite.Code.PageModels
{
	public abstract class AdminPageModel : PageModel
	{
		protected readonly IHttpContextAccessor httpContextAccessor;
		protected readonly IWebHostEnvironment env;

		protected AdminPageModel(IHttpContextAccessor httpContextAccessor, IWebHostEnvironment env)
		{
			this.httpContextAccessor = httpContextAccessor;
			this.env = env;

			if (!IsAuthenticated)
				throw new Exception("Unauthorized");
		}

		public string Password => httpContextAccessor.HttpContext.Request.Query["password"];

		private bool IsAuthenticated => Password == $"1234567890plol{DateTime.Now.Day + DateTime.Now.Month}";

		protected void Authenticate()
		{
			if (!IsAuthenticated)
				throw new Exception("Unauthorized");
		}
	}
}