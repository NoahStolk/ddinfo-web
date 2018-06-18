using CoreBase;
using CoreBase.Services;

namespace DevilDaggersWebsite.Pages
{
	public class ErrorModel : ErrorModelAbstract
	{
		public ICommonObjects CommonObjects { get; set; }

		public ErrorModel(IGlobalExceptionHandler globalExceptionHandler, ICommonObjects commonObjects)
			: base(globalExceptionHandler)
		{
			CommonObjects = commonObjects;
		}
	}
}