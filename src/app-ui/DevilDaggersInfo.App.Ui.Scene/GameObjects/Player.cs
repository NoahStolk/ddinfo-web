using DevilDaggersInfo.App.Ui.Base;
using DevilDaggersInfo.Core.Replay.PostProcessing.PlayerMovement;
using Silk.NET.OpenGL;
using Warp.NET.Content;

namespace DevilDaggersInfo.App.Ui.Scene.GameObjects;

public class Player
{
	private static uint _vao;

	private readonly PlayerMovementTimeline _movementTimeline;
	private readonly MeshObject _mesh;

	public Player(PlayerMovementTimeline movementTimeline)
	{
		_movementTimeline = movementTimeline;
		_mesh = new(_vao, ContentManager.Content.Skull4Mesh, Quaternion.Identity, default);
	}

	public static unsafe void Initialize()
	{
		_vao = CreateVao(ContentManager.Content.Skull4Mesh);

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

	public void Update(float currentTime)
	{
		_mesh.PrepareUpdate();
		_mesh.PositionState.Physics = _movementTimeline.GetPositionAtTime(currentTime);
	}

	public void Render()
	{
		ContentManager.Content.Skull4Texture.Use();

		_mesh.Render();
	}
}
