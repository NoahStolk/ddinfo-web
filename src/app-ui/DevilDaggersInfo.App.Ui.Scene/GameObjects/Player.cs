using DevilDaggersInfo.Core.Replay.PostProcessing.ReplaySimulation;
using Silk.NET.OpenGL;
using Warp.NET.Content;
using Warp.NET.Debugging;

namespace DevilDaggersInfo.App.Ui.Scene.GameObjects;

public class Player
{
	private static uint _vao;

	private readonly ReplaySimulation _movementTimeline;
	private readonly MeshObject _mesh;

	public Player(ReplaySimulation movementTimeline)
	{
		_movementTimeline = movementTimeline;
		_mesh = new(_vao, WarpModels.PlayerMovement.MainMesh, Quaternion.Identity, default);
	}

	public static unsafe void Initialize()
	{
		_vao = CreateVao(WarpModels.PlayerMovement.MainMesh);

		static uint CreateVao(Mesh mesh)
		{
			uint vao = Gl.GenVertexArray();
			Gl.BindVertexArray(vao);

			uint vbo = Gl.GenBuffer();
			Gl.BindBuffer(BufferTargetARB.ArrayBuffer, vbo);

			fixed (Vertex* v = &mesh.Vertices[0])
				Gl.BufferData(BufferTargetARB.ArrayBuffer, (nuint)(mesh.Vertices.Length * sizeof(Vertex)), v, BufferUsageARB.StaticDraw);

			Gl.EnableVertexAttribArray(0);
			Gl.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, (uint)sizeof(Vertex), (void*)0);

			Gl.EnableVertexAttribArray(1);
			Gl.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, (uint)sizeof(Vertex), (void*)(3 * sizeof(float)));

			// TODO: We don't do anything with normals here.
			Gl.EnableVertexAttribArray(2);
			Gl.VertexAttribPointer(2, 3, VertexAttribPointerType.Float, false, (uint)sizeof(Vertex), (void*)(5 * sizeof(float)));

			Gl.BindVertexArray(0);

			Gl.BindBuffer(BufferTargetARB.ArrayBuffer, 0);

			return vao;
		}
	}

	public void Update(int currentTick)
	{
		const float offsetY = 3.3f;

		_mesh.PrepareUpdate();

		PlayerMovementSnapshot snapshot = _movementTimeline.GetPlayerMovementSnapshot(currentTick);
		_mesh.RotationState.Physics = snapshot.Rotation;
		_mesh.PositionState.Physics = snapshot.Position + new Vector3(0, offsetY, 0);

		DebugStack.Add($"Player position: {snapshot.IsOnGround} {snapshot.Position}");
	}

	public void Render()
	{
		WarpTextures.Blank.Use();

		_mesh.Render();
	}
}
