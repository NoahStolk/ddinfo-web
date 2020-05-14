using CoreBase3.Services;
using DevilDaggersWebsite.Code.PageModels;

namespace DevilDaggersWebsite.Pages.Admin
{
	public class ModsModel : AdminFilePageModel
	{
		public ModsModel(ICommonObjects commonObjects)
			: base(commonObjects, "mods")
		{
		}
	}
}