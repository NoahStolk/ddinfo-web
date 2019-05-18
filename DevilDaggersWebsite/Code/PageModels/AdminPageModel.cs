using Microsoft.AspNetCore.Mvc.RazorPages;
using System;

namespace DevilDaggersWebsite.Code.PageModels
{
	public abstract class AdminPageModel : PageModel
	{
		public bool Authenticate(string pass)
		{
			return pass == $"1234567890plol{DateTime.Now.Day + DateTime.Now.Month}";
		}
	}
}