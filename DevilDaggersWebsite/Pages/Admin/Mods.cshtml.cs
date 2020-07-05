using CoreBase3.Services;
using DevilDaggersWebsite.Code.PageModels;
using DevilDaggersWebsite.Code.Users;

namespace DevilDaggersWebsite.Pages.Admin
{
	public class ModsModel : AdminFilePageModel<AssetMod>
	{
		public ModsModel(ICommonObjects commonObjects)
			: base(commonObjects)
		{
		}
	}
}