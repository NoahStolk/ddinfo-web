using DevilDaggersInfo.App.Engine.Maths.Numerics;

namespace DevilDaggersInfo.App.Engine.InterpolationStates;

public class SpinorState : AbstractInterpolationState<Spinor>
{
	public SpinorState(Spinor start)
		: base(start)
	{
	}

	public override void PrepareRender()
		=> Render = Spinor.Slerp(PhysicsPrevious, Physics, WarpBase.Game.SubFrame);
}
