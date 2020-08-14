using DevilDaggersWebsite.Code.Database;
using DevilDaggersWebsite.Code.PageModels;
using DevilDaggersWebsite.Code.Users;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace DevilDaggersWebsite.Pages.Admin
{
	public class DonationsModel : AdminFilePageModel<Donation>
	{
		public DonationsModel(IHttpContextAccessor httpContextAccessor, IWebHostEnvironment env)
			: base(httpContextAccessor, env)
		{
		}
	}
}