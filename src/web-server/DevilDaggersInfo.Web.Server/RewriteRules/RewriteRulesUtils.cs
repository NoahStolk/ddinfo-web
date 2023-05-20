namespace DevilDaggersInfo.Web.Server.RewriteRules;

public static class RewriteRulesUtils
{
	public static string TrimStart(string str, params string[] values)
	{
		if (values.Length == 0)
			return str;

		string? sub = Array.Find(values, str.StartsWith);
		return sub == null ? str : str[sub.Length..];
	}
}
