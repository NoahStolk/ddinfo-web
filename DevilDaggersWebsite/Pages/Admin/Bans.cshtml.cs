using CoreBase3.Services;
using DevilDaggersWebsite.Code.PageModels;

namespace DevilDaggersWebsite.Pages.Admin
{
	public class BansModel : AdminFilePageModel
	{
		public BansModel(ICommonObjects commonObjects)
			: base(commonObjects, "bans")
		{
		}
	}
}