namespace DevilDaggersInfo.App.Engine.InterpolationStates;

public class Vector2State : AbstractInterpolationState<Vector2>
{
	public Vector2State(Vector2 start)
		: base(start)
	{
	}

	public override void PrepareRender()
		=> Render = Vector2.Lerp(PhysicsPrevious, Physics, EngineNodes.Game.SubFrame);
}
