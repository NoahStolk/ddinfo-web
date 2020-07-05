using CoreBase3.Services;
using DevilDaggersWebsite.Code.PageModels;
using DevilDaggersWebsite.Code.Users;

namespace DevilDaggersWebsite.Pages.Admin
{
	public class SettingsModel : AdminFilePageModel<PlayerSetting>
	{
		public SettingsModel(ICommonObjects commonObjects)
			: base(commonObjects)
		{
		}
	}
}