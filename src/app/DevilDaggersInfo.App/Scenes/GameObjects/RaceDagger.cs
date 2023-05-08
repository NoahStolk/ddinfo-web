using DevilDaggersInfo.App.Engine.InterpolationStates;
using DevilDaggersInfo.Core.Spawnset;
using Silk.NET.OpenGL;
using System.Numerics;

namespace DevilDaggersInfo.App.Scenes.GameObjects;

public class RaceDagger
{
	private const float _yOffset = 4;

	private static uint _vao;

	private static readonly Vector3 _hiddenPosition = new(-1000, -1000, -1000);

	private readonly Vector3State _meshPosition;
	private readonly QuaternionState _meshRotation;

	public RaceDagger()
	{
		_meshPosition = new(default);
		_meshRotation = new(Quaternion.CreateFromAxisAngle(Vector3.UnitX, MathF.PI * 0.5f));
	}

	public static void Initialize()
	{
		if (_vao != 0)
			throw new InvalidOperationException("Race dagger is already initialized.");

		_vao = MeshShaderUtils.CreateVao(ContentManager.Content.DaggerMesh);
	}

	public void Update(SpawnsetBinary spawnset, int currentTick)
	{
		_meshPosition.PrepareUpdate();
		_meshRotation.PrepareUpdate();

		float currentTime = currentTick / 60f;
		(int X, float? Y, int Z) raceDaggerPosition = spawnset.GetRaceDaggerTilePosition();
		Vector3 basePosition = spawnset.GameMode != GameMode.Race ? _hiddenPosition : _meshPosition.Physics with
		{
			X = spawnset.TileToWorldCoordinate(raceDaggerPosition.X),
			Y = spawnset.GetActualTileHeight(raceDaggerPosition.X, raceDaggerPosition.Z, currentTime) + _yOffset,
			Z = spawnset.TileToWorldCoordinate(raceDaggerPosition.Z),
		};
		_meshPosition.Physics = basePosition + new Vector3(0, 0.15f + MathF.Sin(currentTime) * 0.15f, 0);
		_meshRotation.Physics = _meshRotation.Start * Quaternion.CreateFromAxisAngle(Vector3.UnitZ, currentTime);
	}

	public unsafe void Render()
	{
		_meshPosition.PrepareRender();
		_meshRotation.PrepareRender();

		Root.GameResources.DaggerSilverTexture.Bind();

		Root.InternalResources.MeshShader.SetUniform("model", Matrix4x4.CreateScale(8) * Matrix4x4.CreateFromQuaternion(_meshRotation.Render) * Matrix4x4.CreateTranslation(_meshPosition.Render));

		Root.Gl.BindVertexArray(_vao);
		fixed (uint* i = &ContentManager.Content.DaggerMesh.Indices[0])
			Root.Gl.DrawElements(PrimitiveType.Triangles, (uint)ContentManager.Content.DaggerMesh.Indices.Length, DrawElementsType.UnsignedInt, i);
		Root.Gl.BindVertexArray(0);
	}
}
