using DevilDaggersWebsite.Dto;
using System.Collections.Generic;

namespace DevilDaggersWebsite.Transients
{
	public interface IToolHelper
	{
		List<Tool> Tools { get; }

		Tool GetToolByName(string name);
	}
}
