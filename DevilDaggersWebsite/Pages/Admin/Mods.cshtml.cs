using DevilDaggersWebsite.Code.PageModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace DevilDaggersWebsite.Pages.Admin
{
	public class ModsModel : AdminFilePageModel<Code.Users.AssetMod>
	{
		public ModsModel(IHttpContextAccessor httpContextAccessor, IWebHostEnvironment env)
			: base(httpContextAccessor, env)
		{
		}
	}
}