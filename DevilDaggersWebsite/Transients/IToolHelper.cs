using DevilDaggersWebsite.Dto;
using System.Collections.Generic;

namespace DevilDaggersWebsite.Transients
{
	public interface IToolHelper
	{
		Dictionary<string, List<ChangelogEntry>> Changelogs { get; }

		Tool GetToolByName(string name);

		Tool GetToolFromEntity(Entities.Tool tool);
	}
}
