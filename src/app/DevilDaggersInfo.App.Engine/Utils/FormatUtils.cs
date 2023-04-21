namespace Warp.NET.Utils;

internal static class FormatUtils
{
	public static string FormatAxis(string axisName, float axisValue, int digits)
		=> $"{axisName}:{axisValue.ToString($"{(float.IsNegative(axisValue) ? string.Empty : "+")}0.{new string('0', digits)}")}";
}
