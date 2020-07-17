using DevilDaggersWebsite.Code.PageModels;
using DevilDaggersWebsite.Code.Users;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace DevilDaggersWebsite.Pages.Admin
{
	public class DonatorsModel : AdminFilePageModel<Donator>
	{
		public DonatorsModel(IHttpContextAccessor httpContextAccessor, IWebHostEnvironment env)
			: base(httpContextAccessor, env)
		{
		}
	}
}