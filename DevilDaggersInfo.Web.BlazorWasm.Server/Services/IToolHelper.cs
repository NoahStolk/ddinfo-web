using DevilDaggersInfo.Web.BlazorWasm.Server.InternalModels;
using DevilDaggersInfo.Web.BlazorWasm.Server.InternalModels.Json;

namespace DevilDaggersInfo.Web.BlazorWasm.Server.Services;

public interface IToolHelper
{
	Dictionary<string, List<ChangelogEntry>> Changelogs { get; }

	Tool GetToolByName(string name);
}
