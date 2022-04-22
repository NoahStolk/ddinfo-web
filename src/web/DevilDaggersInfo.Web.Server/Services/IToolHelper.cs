using DevilDaggersInfo.Web.Server.InternalModels;
using DevilDaggersInfo.Web.Server.InternalModels.Json;

namespace DevilDaggersInfo.Web.Server.Services;

public interface IToolHelper
{
	Dictionary<string, List<ChangelogEntry>> Changelogs { get; }

	Tool? GetToolByName(string name);
}
