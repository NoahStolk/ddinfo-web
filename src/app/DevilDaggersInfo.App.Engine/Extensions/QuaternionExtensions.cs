using DevilDaggersInfo.App.Engine.Maths;
using DevilDaggersInfo.App.Engine.Utils;

namespace DevilDaggersInfo.App.Engine.Extensions;

public static class QuaternionExtensions
{
	public static string ToString(this Quaternion quaternion, int digits)
		=> $"{{{FormatUtils.FormatAxis(nameof(Quaternion.X), quaternion.X, digits)} {FormatUtils.FormatAxis(nameof(Quaternion.Y), quaternion.Y, digits)} {FormatUtils.FormatAxis(nameof(Quaternion.Z), quaternion.Z, digits)} {FormatUtils.FormatAxis(nameof(Quaternion.W), quaternion.W, digits)}}}";

	public static void Randomize(this ref Quaternion quaternion, float randomizeAmount, Random? random = null)
	{
		random ??= Random.Shared;
		quaternion.X += random.RandomFloat(-randomizeAmount, randomizeAmount);
		quaternion.Y += random.RandomFloat(-randomizeAmount, randomizeAmount);
		quaternion.Z += random.RandomFloat(-randomizeAmount, randomizeAmount);
		quaternion.W += random.RandomFloat(-randomizeAmount, randomizeAmount);
	}

	public static bool ContainsNaN(this Quaternion quaternion)
		=> !MathUtils.IsFloatReal(quaternion.X) || !MathUtils.IsFloatReal(quaternion.Y) || !MathUtils.IsFloatReal(quaternion.Z) || !MathUtils.IsFloatReal(quaternion.W);
}
