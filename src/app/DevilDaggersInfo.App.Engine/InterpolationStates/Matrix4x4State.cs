namespace Warp.NET.InterpolationStates;

public class Matrix4x4State : AbstractInterpolationState<Matrix4x4>
{
	public Matrix4x4State(Matrix4x4 start)
		: base(start)
	{
	}

	public override void PrepareRender()
		=> Render = Matrix4x4.Lerp(PhysicsPrevious, Physics, WarpBase.Game.SubFrame);
}
