using DevilDaggersCore.Leaderboards;
using DevilDaggersWebsite.Code.Utils.Web;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.Pages.Leaderboard
{
	public class UserProgressionModel : PageModel
	{
		public int UserId { get; set; }
		public string Username { get; set; }
		public bool IsValid { get; set; }

		public async Task OnGetAsync(int userId)
		{
			UserId = userId;
			Username = string.Empty;

			IsValid = UserId > 0;
			if (IsValid)
			{
				Entry entry = await Hasmodai.GetUserById(userId);
				Username = entry.Username;
			}
		}
	}
}