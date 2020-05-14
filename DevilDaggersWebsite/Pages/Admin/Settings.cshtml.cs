using CoreBase3.Services;
using DevilDaggersWebsite.Code.PageModels;

namespace DevilDaggersWebsite.Pages.Admin
{
	public class SettingsModel : AdminFilePageModel
	{
		public SettingsModel(ICommonObjects commonObjects)
			: base(commonObjects, "settings")
		{
		}
	}
}