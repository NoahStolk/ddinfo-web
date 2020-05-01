using CoreBase3.Services;
using DevilDaggersCore.Leaderboards;
using DevilDaggersWebsite.Code.Utils;
using DevilDaggersWebsite.Code.Utils.Web;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.Pages.Leaderboard
{
	public class UserProgressionModel : PageModel
	{
		private readonly ICommonObjects commonObjects;

		public int UserId { get; set; }
		public string Username { get; set; }
		public bool IsValid { get; set; }

		public UserProgressionModel(ICommonObjects commonObjects)
		{
			this.commonObjects = commonObjects;
		}

		public async Task OnGetAsync(int userId)
		{
			UserId = userId;
			Username = string.Empty;

			IsValid = UserId > 0;
			if (IsValid)
			{
				Entry entry = await Hasmodai.GetUserById(userId);
				Username = entry.Username;

				IsValid = entry.ExistsInHistory(commonObjects);
			}
		}
	}
}