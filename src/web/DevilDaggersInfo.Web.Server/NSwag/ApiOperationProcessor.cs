using NSwag.Generation.Processors;
using NSwag.Generation.Processors.Contexts;

namespace DevilDaggersInfo.Web.Server.NSwag;

public class ApiOperationProcessor : IOperationProcessor
{
	private readonly string _apiName;

	public ApiOperationProcessor(string apiName)
	{
		_apiName = apiName;
	}

	public bool Process(OperationProcessorContext context)
	{
		return context.ControllerType.Namespace?.EndsWith($"Controllers.{_apiName}") == true;
	}
}
