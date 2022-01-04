namespace DevilDaggersInfo.Web.BlazorWasm.Server.Utils;

public static class RewriteRuleUtils
{
	public static bool EndsWithContent(string pathString)
	{
		return
			pathString.EndsWith(".js") ||
			pathString.EndsWith(".css") ||
			pathString.EndsWith(".json") ||
			pathString.EndsWith(".png") ||
			pathString.EndsWith(".ttf") ||
			pathString.EndsWith(".ico") ||
			pathString.EndsWith(".dll");
	}
}
