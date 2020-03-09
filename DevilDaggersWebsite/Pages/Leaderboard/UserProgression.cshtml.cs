using DevilDaggersCore.Leaderboards;
using DevilDaggersWebsite.Code.Utils.Web;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.Pages.Leaderboard
{
	public class UserProgressionModel : PageModel
	{
		public int UserID { get; set; }
		public string Username { get; set; }
		public bool IsValid { get; set; }

		public async Task OnGetAsync(int userId)
		{
			UserID = userId;
			Username = string.Empty;

			IsValid = UserID > 0;
			if (IsValid)
			{
				Entry entry = await Hasmodai.GetUserById(userId);
				Username = entry.Username;
			}
		}
	}
}