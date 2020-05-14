using CoreBase3.Services;
using DevilDaggersWebsite.Code.PageModels;

namespace DevilDaggersWebsite.Pages.Admin
{
	public class TitlesModel : AdminFilePageModel
	{
		public TitlesModel(ICommonObjects commonObjects)
			: base(commonObjects, "titles")
		{
		}
	}
}