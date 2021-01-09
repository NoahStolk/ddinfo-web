using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.Razor.Pages.Admin
{
	public class IndexModel : PageModel
	{
		public IndexModel(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
		{
			RoleManager = roleManager;
			UserManager = userManager;
		}

		public RoleManager<IdentityRole> RoleManager { get; }
		public UserManager<IdentityUser> UserManager { get; }

		public IdentityUser? IdentityUser { get; private set; }

		public async Task<ActionResult?> OnGetAsync()
		{
			string? id = UserManager.GetUserId(User);
			IdentityUser = await UserManager.FindByIdAsync(id);

			if (id == null || IdentityUser == null)
				return RedirectToPage("/Error/404");

			return null;
		}
	}
}