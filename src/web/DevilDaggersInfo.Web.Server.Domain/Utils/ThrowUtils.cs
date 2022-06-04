using System.Runtime.CompilerServices;

namespace DevilDaggersInfo.Web.Server.Domain.Utils;

public static class ThrowUtils
{
	public static void ThrowIf(bool condition, [CallerArgumentExpression("condition")] string conditionExpression = "", [CallerMemberName] string caller = "")
	{
		if (condition)
			throw new InvalidOperationException($"Throwing due to condition {conditionExpression} in {caller}.");
	}
}
