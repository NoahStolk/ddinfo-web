using CoreBase3.Services;
using DevilDaggersWebsite.Code.PageModels;
using DevilDaggersWebsite.Code.Users;

namespace DevilDaggersWebsite.Pages.Admin
{
	public class DonatorsModel : AdminFilePageModel<Donator>
	{
		public DonatorsModel(ICommonObjects commonObjects)
			: base(commonObjects)
		{
		}
	}
}