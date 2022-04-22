using NSwag.Generation.Processors;
using NSwag.Generation.Processors.Contexts;

namespace DevilDaggersInfo.Web.BlazorWasm.Server.NSwag;

public class PublicApiOperationProcessor : IOperationProcessor
{
	public bool Process(OperationProcessorContext context)
	{
		return context.ControllerType.Namespace?.Contains("Admin") != true;
	}
}
