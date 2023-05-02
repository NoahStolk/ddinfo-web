using DevilDaggersInfo.App.Engine.Content;
using DevilDaggersInfo.App.Ui.Base;
using DevilDaggersInfo.Core.Replay.PostProcessing.ReplaySimulation;
using Silk.NET.OpenGL;
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

	public static unsafe void Initialize(GL gl)
	{
		_vao = CreateVao(gl, ContentManager.Content.Hand4Mesh);

		static uint CreateVao(GL gl, MeshContent mesh)
		{
			uint vao = gl.GenVertexArray();
			gl.BindVertexArray(vao);

			uint vbo = gl.GenBuffer();
			gl.BindBuffer(BufferTargetARB.ArrayBuffer, vbo);

			fixed (Vertex* v = &mesh.Vertices[0])
				gl.BufferData(BufferTargetARB.ArrayBuffer, (nuint)(mesh.Vertices.Length * sizeof(Vertex)), v, BufferUsageARB.StaticDraw);

			gl.EnableVertexAttribArray(0);
			gl.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, (uint)sizeof(Vertex), (void*)0);

			gl.EnableVertexAttribArray(1);
			gl.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, (uint)sizeof(Vertex), (void*)(3 * sizeof(float)));

			// TODO: We don't do anything with normals here.
			gl.EnableVertexAttribArray(2);
			gl.VertexAttribPointer(2, 3, VertexAttribPointerType.Float, false, (uint)sizeof(Vertex), (void*)(5 * sizeof(float)));

			gl.BindVertexArray(0);
			gl.BindBuffer(BufferTargetARB.ArrayBuffer, 0);
			gl.DeleteBuffer(vbo);

			return vao;
		}
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
		GlobalContext.GameResources.Hand4Texture.Bind();
		_mesh.Render();
	}
}
