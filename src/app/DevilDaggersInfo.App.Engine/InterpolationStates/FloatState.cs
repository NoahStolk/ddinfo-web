using Warp.NET.Maths;

namespace Warp.NET.InterpolationStates;

public class FloatState : AbstractInterpolationState<float>
{
	public FloatState(float start)
		: base(start)
	{
	}

	public override void PrepareRender()
		=> Render = MathUtils.Lerp(PhysicsPrevious, Physics, WarpBase.Game.SubFrame);
}
