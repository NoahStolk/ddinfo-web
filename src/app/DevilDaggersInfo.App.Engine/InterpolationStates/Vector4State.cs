namespace Warp.NET.InterpolationStates;

public class Vector4State : AbstractInterpolationState<Vector4>
{
	public Vector4State(Vector4 start)
		: base(start)
	{
	}

	public override void PrepareRender()
		=> Render = Vector4.Lerp(PhysicsPrevious, Physics, WarpBase.Game.SubFrame);
}
