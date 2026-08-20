using NSwag.Generation.Processors;
using NSwag.Generation.Processors.Contexts;

namespace DevilDaggersInfo.Web.Server.NSwag;

internal sealed class ApiOperationProcessor(string apiName) : IOperationProcessor
{
	public bool Process(OperationProcessorContext context)
	{
		return context.ControllerType.Namespace?.EndsWith($"Controllers.{apiName}") == true;
	}
}
