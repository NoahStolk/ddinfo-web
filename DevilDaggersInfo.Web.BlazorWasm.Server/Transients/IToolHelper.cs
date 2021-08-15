using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.Tools;

namespace DevilDaggersInfo.Web.BlazorWasm.Server.Transients;

public interface IToolHelper
{
	List<GetTool> Tools { get; }

	GetTool GetToolByName(string name);
}
