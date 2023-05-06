using DevilDaggersInfo.App.Ui.Base;
using DevilDaggersInfo.Core.Replay.PostProcessing.ReplaySimulation;
using System.Numerics;

namespace DevilDaggersInfo.App.Scenes.Base.GameObjects;

public class Player
{
	private static readonly Quaternion _rotationOffset = Quaternion.CreateFromAxisAngle(Vector3.UnitX, MathF.PI / 2) * Quaternion.CreateFromAxisAngle(Vector3.UnitY, MathF.PI) * Quaternion.CreateFromAxisAngle(Vector3.UnitZ, MathF.PI);

	private static uint _vao;

	private readonly ReplaySimulation _movementTimeline;
	private readonly PlayerMovement _mesh;

	public Player(ReplaySimulation movementTimeline)
	{
		_movementTimeline = movementTimeline;
		_mesh = new(_vao, ContentManager.Content.Hand4Mesh, default, default);
		Light = new(6, default, new(1, 0.5f, 0));
	}

	public LightObject Light { get; }

	public static void Initialize()
	{
		if (_vao != 0)
			throw new InvalidOperationException("Player is already initialized.");

		_vao = MeshShaderUtils.CreateVao(ContentManager.Content.Hand4Mesh);
	}

	public void Update(int currentTick)
	{
		const float offsetY = 3.3f;

		_mesh.PrepareUpdate();

		PlayerMovementSnapshot snapshot = _movementTimeline.GetPlayerMovementSnapshot(currentTick);
		_mesh.RotationState.Physics = snapshot.Rotation * _rotationOffset;
		_mesh.PositionState.Physics = snapshot.Position + new Vector3(0, offsetY, 0);

		Light.PositionState.Physics = _mesh.PositionState.Physics;
	}

	public void Render()
	{
		Root.GameResources.Hand4Texture.Bind();
		_mesh.Render();
	}
}
