using DevilDaggersWebsite.BlazorWasm.Shared.Dto.Tools;
using System.Collections.Generic;

namespace DevilDaggersWebsite.BlazorWasm.Server.Transients
{
	public interface IToolHelper
	{
		List<GetToolPublic> Tools { get; }

		GetToolPublic GetToolByName(string name);
	}
}
