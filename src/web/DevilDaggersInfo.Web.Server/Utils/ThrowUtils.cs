namespace DevilDaggersInfo.Web.Server.Utils;

public static class ThrowUtils
{
	public static void ThrowIf(bool condition, [CallerArgumentExpression("condition")] string conditionExpression = "", [CallerMemberName] string caller = "")
	{
		if (condition)
			throw new InvalidOperationException($"Throwing due to condition {conditionExpression} in {caller}.");
	}
}
