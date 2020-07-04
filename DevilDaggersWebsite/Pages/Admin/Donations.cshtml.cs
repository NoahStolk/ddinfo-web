using CoreBase3.Services;
using DevilDaggersWebsite.Code.PageModels;

namespace DevilDaggersWebsite.Pages.Admin
{
	public class DonationsModel : AdminFilePageModel
	{
		public DonationsModel(ICommonObjects commonObjects)
			: base(commonObjects, "donations")
		{
		}
	}
}