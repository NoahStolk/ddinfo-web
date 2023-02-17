using Warp.NET.InterpolationStates;

namespace DevilDaggersInfo.App.Ui.Scene.GameObjects;

public class LightObject
{
	public LightObject(float radius, Vector3 position, Vector3 color)
	{
		RadiusState = new(radius);
		PositionState = new(position);
		ColorState = new(color);
	}

	public FloatState RadiusState { get; }

	public Vector3State PositionState { get; }

	public Vector3State ColorState { get; }
}
