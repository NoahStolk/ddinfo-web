using CoreBase3.Services;
using DevilDaggersWebsite.Code.PageModels;
using DevilDaggersWebsite.Code.Users;

namespace DevilDaggersWebsite.Pages.Admin
{
	public class FlagsModel : AdminFilePageModel<Flag>
	{
		public FlagsModel(ICommonObjects commonObjects)
			: base(commonObjects)
		{
		}
	}
}