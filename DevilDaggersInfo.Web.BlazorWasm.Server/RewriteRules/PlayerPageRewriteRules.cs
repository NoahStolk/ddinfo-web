using Microsoft.AspNetCore.Rewrite;
using Microsoft.Net.Http.Headers;

namespace DevilDaggersInfo.Web.BlazorWasm.Server.RewriteRules;

public class PlayerPageRewriteRules : IRule
{
	public void ApplyRule(RewriteContext context)
	{
		HttpRequest request = context.HttpContext.Request;
		if (request.Path.Value == null || request.QueryString.Value == null || RewriteRuleUtils.EndsWithContent(request.Path.Value))
			return;

		if (!request.Path.Value.StartsWith("/Leaderboard/Player", StringComparison.OrdinalIgnoreCase))
			return;

		const string queryStringKey = "?id=";
		if (!request.QueryString.Value.StartsWith(queryStringKey, StringComparison.OrdinalIgnoreCase))
			return;

		if (!int.TryParse(request.QueryString.Value.TrimStart(queryStringKey), out int id))
			return;

		context.Result = RuleResult.EndResponse;

		context.HttpContext.Response.StatusCode = StatusCodes.Status301MovedPermanently;
		context.HttpContext.Response.Headers[HeaderNames.Location] = $"{request.Scheme}://{request.Host}/leaderboard/player/{id}";
	}
}
