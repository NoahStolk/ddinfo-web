using CoreBase3.Services;
using DevilDaggersWebsite.Code.PageModels;

namespace DevilDaggersWebsite.Pages.Admin
{
	public class DonatorsModel : AdminFilePageModel
	{
		public DonatorsModel(ICommonObjects commonObjects)
			: base(commonObjects, "donators")
		{
		}
	}
}