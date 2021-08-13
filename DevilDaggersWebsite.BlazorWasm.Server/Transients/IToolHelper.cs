using DevilDaggersWebsite.BlazorWasm.Shared.Dto.Public.Tools;
using System.Collections.Generic;

namespace DevilDaggersWebsite.BlazorWasm.Server.Transients
{
	public interface IToolHelper
	{
		List<GetTool> Tools { get; }

		GetTool GetToolByName(string name);
	}
}
