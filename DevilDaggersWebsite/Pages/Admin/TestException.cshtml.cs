﻿using CoreBase3.Services;
using DevilDaggersWebsite.Code.PageModels;
using System;

namespace DevilDaggersWebsite.Pages.Admin
{
	public class TestExceptionModel : AdminPageModel
	{
		public TestExceptionModel(ICommonObjects commonObjects)
			: base(commonObjects)
		{
			throw new Exception("ADMIN TEST EXCEPTION");
		}
	}
}