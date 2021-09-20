using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.Tools;

namespace DevilDaggersInfo.Web.BlazorWasm.Server.Transients;

public interface IToolHelper
{
	List<Tool> Tools { get; }

	Tool GetToolByName(string name);
}
