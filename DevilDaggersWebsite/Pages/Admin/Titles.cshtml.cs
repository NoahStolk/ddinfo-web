using CoreBase3.Services;
using DevilDaggersWebsite.Code.PageModels;
using DevilDaggersWebsite.Code.Users;

namespace DevilDaggersWebsite.Pages.Admin
{
	public class TitlesModel : AdminFilePageModel<UserTitleCollection>
	{
		public TitlesModel(ICommonObjects commonObjects)
			: base(commonObjects)
		{
		}
	}
}