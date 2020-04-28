using CoreBase3.ErrorHandling;
using CoreBase3.Services;

namespace DevilDaggersWebsite.Pages
{
	public class ErrorModel : AbstractErrorModel
	{
		public ErrorModel(IGlobalExceptionHandler globalExceptionHandler)
			: base(globalExceptionHandler)
		{
		}
	}
}