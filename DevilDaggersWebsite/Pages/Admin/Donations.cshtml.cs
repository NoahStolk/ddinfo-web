using CoreBase3.Services;
using DevilDaggersWebsite.Code.PageModels;
using DevilDaggersWebsite.Code.Users;

namespace DevilDaggersWebsite.Pages.Admin
{
	public class DonationsModel : AdminFilePageModel<Donation>
	{
		public DonationsModel(ICommonObjects commonObjects)
			: base(commonObjects)
		{
		}
	}
}