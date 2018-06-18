using CoreBase;
using CoreBase.Services;

namespace DevilDaggersWebsite.Pages
{
	public class ErrorModel : ErrorModelAbstract
	{
		public ErrorModel(IGlobalExceptionHandler globalExceptionHandler)
			: base(globalExceptionHandler)
		{
		}
	}
}