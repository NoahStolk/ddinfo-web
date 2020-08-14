using DevilDaggersWebsite.Code.PageModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace DevilDaggersWebsite.Pages.Admin
{
	public class DonationsModel : AdminFilePageModel<Code.Users.Donation>
	{
		public DonationsModel(IHttpContextAccessor httpContextAccessor, IWebHostEnvironment env)
			: base(httpContextAccessor, env)
		{
		}
	}
}