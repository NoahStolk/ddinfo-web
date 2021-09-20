using NSwag;
using NSwag.Generation.Processors;
using NSwag.Generation.Processors.Contexts;

namespace DevilDaggersInfo.Web.BlazorWasm.Server.NSwag;

public class PublicApiDocumentProcessor : IDocumentProcessor
{
	public void Process(DocumentProcessorContext context)
	{
		foreach (KeyValuePair<string, OpenApiPathItem> path in context.Document.Paths.Where(p => p.Key.Contains("admin")))
			context.Document.Paths.Remove(path);
	}
}
