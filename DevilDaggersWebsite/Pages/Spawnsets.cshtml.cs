using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DevilDaggersWebsite.Pages
{
	public class SpawnsetsModel : PageModel
	{
		public IHostingEnvironment Env { get; private set; }

		public SpawnsetsModel(IHostingEnvironment env)
		{
			Env = env;
		}
    }
}