using CoreBase3.Services;
using DevilDaggersWebsite.Code.PageModels;
using System;

namespace DevilDaggersWebsite.Pages.Admin
{
	public class TestExceptionModel : AdminPageModel
	{
		public TestExceptionModel(ICommonObjects commonObjects)
			: base(commonObjects)
		{
			throw new Exception("Admin test exception", new Exception("Inner exception message", new Exception("Another inner exception message")));
		}
	}
}