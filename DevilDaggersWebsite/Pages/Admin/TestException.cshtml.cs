using DevilDaggersWebsite.Code.PageModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;

namespace DevilDaggersWebsite.Pages.Admin
{
	public class TestExceptionModel : AdminPageModel
	{
		public TestExceptionModel(IHttpContextAccessor httpContextAccessor, IWebHostEnvironment env)
			: base(httpContextAccessor, env)
		{
			throw new Exception("Admin test exception", new Exception("Inner exception message", new Exception("Another inner exception message")));
		}
	}
}