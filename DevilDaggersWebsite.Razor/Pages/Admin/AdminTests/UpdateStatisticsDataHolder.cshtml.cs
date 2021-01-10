using DevilDaggersWebsite.LeaderboardStatistics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DevilDaggersWebsite.Razor.Pages.Admin.AdminTests
{
	public class UpdateStatisticsDataHolderModel : PageModel
	{
		private readonly IWebHostEnvironment _env;

		public UpdateStatisticsDataHolderModel(IWebHostEnvironment env)
		{
			_env = env;
		}

		public void OnGet()
		{
			StatisticsDataHolder.Instance.Update(_env);
		}
	}
}