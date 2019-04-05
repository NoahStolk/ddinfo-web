using CoreBase.Services;
using DevilDaggersCore.Leaderboard;
using DevilDaggersWebsite.Utils.Web;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.Pages.Leaderboard
{
	public class UserProgressionModel : PageModel
	{
		private readonly ICommonObjects _commonObjects;

		public string Username { get; set; }

		public UserProgressionModel(ICommonObjects commonObjects)
		{
			_commonObjects = commonObjects;
		}

		public async Task OnGetAsync(int userID)
		{
			Entry entry = await Hasmodai.GetUserByID(userID);
			Username = entry.Username;
		}
	}
}