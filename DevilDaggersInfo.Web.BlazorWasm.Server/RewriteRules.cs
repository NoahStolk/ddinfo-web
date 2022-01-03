using Microsoft.AspNetCore.Rewrite;
using Microsoft.Net.Http.Headers;

namespace DevilDaggersInfo.Web.BlazorWasm.Server;

public class RewriteRules : IRule
{
	public void ApplyRule(RewriteContext context)
	{
		HttpRequest request = context.HttpContext.Request;

		if (request.Path.Value == null || request.QueryString.Value == null)
			return;

		if (request.Path.Value.EndsWith(".js") ||
			request.Path.Value.EndsWith(".css") ||
			request.Path.Value.EndsWith(".json") ||
			request.Path.Value.EndsWith(".png") ||
			request.Path.Value.EndsWith(".ttf"))
		{
			return;
		}

		if (!request.Path.Value.StartsWith("/Leaderboard/Player", StringComparison.OrdinalIgnoreCase))
			return;

		const string queryStringKey = "?id=";
		if (!request.QueryString.Value.StartsWith(queryStringKey, StringComparison.OrdinalIgnoreCase))
			return;

		if (!int.TryParse(request.QueryString.Value.TrimStart(queryStringKey), out int id))
			return;

		HttpResponse response = context.HttpContext.Response;
		//response.StatusCode = StatusCodes.Status301MovedPermanently;
		 context.Result = RuleResult.EndResponse;

		response.Headers[HeaderNames.Location] = $"./leaderboard/player/{id}";
	}
}
