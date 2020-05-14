using CoreBase3.Services;
using DevilDaggersWebsite.Code.PageModels;

namespace DevilDaggersWebsite.Pages.Admin
{
	public class FlagsModel : AdminFilePageModel
	{
		public FlagsModel(ICommonObjects commonObjects)
			: base(commonObjects, "flags")
		{
		}
	}
}