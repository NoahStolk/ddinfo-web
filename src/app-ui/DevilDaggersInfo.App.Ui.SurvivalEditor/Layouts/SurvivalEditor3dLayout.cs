using DevilDaggersInfo.App.Ui.Base;
using DevilDaggersInfo.App.Ui.Base.DependencyPattern;
using DevilDaggersInfo.App.Ui.Base.DependencyPattern.Inversion.Layouts.SurvivalEditor;
using DevilDaggersInfo.App.Ui.Base.States;
using Silk.NET.GLFW;
using Silk.NET.OpenGL;
using Warp;
using Warp.Content;
using Warp.InterpolationStates;
using Warp.Ui;
using Shader=Warp.Content.Shader;
using Texture=Warp.Content.Texture;

namespace DevilDaggersInfo.App.Ui.SurvivalEditor.Layouts;

public class SurvivalEditor3dLayout : Layout, ISurvivalEditor3dLayout
{
	private readonly Camera _camera = new();
	private readonly Shader _meshShader;
	private readonly List<MeshObject> _tiles = new();

	public SurvivalEditor3dLayout(Shader meshShader)
		: base(Constants.Full)
	{
		_meshShader = meshShader;
	}

	public void InitializeScene()
	{
		const int tileDimension = 3;
		const int start = -tileDimension / 2;
		const int end = tileDimension / 2;
		for (int i = start; i <= end; i++)
		{
			for (int j = start; j <= end; j++)
			{
				_tiles.Add(new(ContentManager.Content.TileMesh, ContentManager.Content.TileTexture, _meshShader, Vector3.One, Quaternion.Identity, new(i * 4, 0, j * 4)));
			}
		}
	}

	public void Update()
	{
		_camera.Update();

		if (Input.IsKeyPressed(Keys.Escape))
			LayoutManager.ToSurvivalEditorMainLayout();
	}

	public void Render3d()
	{
		_camera.PreRender();

		_meshShader.Use();
		_meshShader.SetMatrix4x4("view", _camera.ViewMatrix);
		_meshShader.SetMatrix4x4("projection", _camera.Projection);

		foreach (MeshObject tile in _tiles)
			tile.Render();
	}

	public void Render()
	{
	}

	private sealed class MeshObject
	{
		private readonly Mesh _mesh;
		private readonly Texture _texture;
		private readonly Shader _shader;
		private readonly Matrix4x4 _modelMatrix;
		private readonly uint _vao;

		public unsafe MeshObject(Mesh mesh, Texture texture, Shader shader, Vector3 scale, Quaternion rotation, Vector3 position)
		{
			_mesh = mesh;
			_texture = texture;
			_shader = shader;

			Matrix4x4 scaleMatrix = Matrix4x4.CreateScale(scale);
			Matrix4x4 rotationMatrix = Matrix4x4.CreateFromQuaternion(rotation);
			Matrix4x4 translationMatrix = Matrix4x4.CreateTranslation(position);
			_modelMatrix = scaleMatrix * rotationMatrix * translationMatrix;

			_vao = Gl.GenVertexArray();
			Gl.BindVertexArray(_vao);

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
		}

		public unsafe void Render()
		{
			_shader.SetMatrix4x4("model", _modelMatrix);
			_shader.SetInt("textureDiffuse", 0);
			_texture.Use();

			Gl.BindVertexArray(_vao);
			fixed (uint* i = &_mesh.Indices[0])
				Gl.DrawElements(PrimitiveType.Triangles, (uint)_mesh.Indices.Length, DrawElementsType.UnsignedInt, i);
			Gl.BindVertexArray(0);
		}
	}

	private sealed class Camera
	{
		private readonly Vector3State _positionState = new(default);
		private readonly QuaternionState _rotationState = new(Quaternion.Identity);

		private Vector3 _axisAlignedSpeed;

		public Matrix4x4 Projection { get; private set; }
		public Matrix4x4 ViewMatrix { get; private set; }

		public void Update()
		{
			_positionState.PrepareUpdate();
			_rotationState.PrepareUpdate();

			const float moveSpeed = 20;
			const float acceleration = 20;
			const float friction = 20;
			const Keys forwardInput = Keys.W;
			const Keys leftInput = Keys.A;
			const Keys backwardInput = Keys.S;
			const Keys rightInput = Keys.D;
			const Keys upInput = Keys.Space;
			const Keys downInput = Keys.ShiftLeft;
			bool forwardHold = Input.IsKeyHeld(forwardInput);
			bool leftHold = Input.IsKeyHeld(leftInput);
			bool backwardHold = Input.IsKeyHeld(backwardInput);
			bool rightHold = Input.IsKeyHeld(rightInput);
			bool upHold = Input.IsKeyHeld(upInput);
			bool downHold = Input.IsKeyHeld(downInput);

			float accelerationDt = acceleration * Root.Game.Dt;
			float frictionDt = friction * Root.Game.Dt;

			if (leftHold)
				_axisAlignedSpeed.X += accelerationDt;
			if (rightHold)
				_axisAlignedSpeed.X -= accelerationDt;

			if (upHold)
				_axisAlignedSpeed.Y += accelerationDt;
			if (downHold)
				_axisAlignedSpeed.Y -= accelerationDt;

			if (forwardHold)
				_axisAlignedSpeed.Z += accelerationDt;
			if (backwardHold)
				_axisAlignedSpeed.Z -= accelerationDt;

			if (!leftHold && !rightHold)
				_axisAlignedSpeed.X -= _axisAlignedSpeed.X * frictionDt;

			if (!upHold && !downHold)
				_axisAlignedSpeed.Y -= _axisAlignedSpeed.Y * frictionDt;

			if (!forwardHold && !backwardHold)
				_axisAlignedSpeed.Z -= _axisAlignedSpeed.Z * frictionDt;

			_axisAlignedSpeed.X = Math.Clamp(_axisAlignedSpeed.X, -1, 1);
			_axisAlignedSpeed.Y = Math.Clamp(_axisAlignedSpeed.Y, -1, 1);
			_axisAlignedSpeed.Z = Math.Clamp(_axisAlignedSpeed.Z, -1, 1);

			Matrix4x4 rotMat = Matrix4x4.CreateFromQuaternion(_rotationState.Physics);
			Vector3 transformed = RotateVector(_axisAlignedSpeed, rotMat) + new Vector3(0, _axisAlignedSpeed.Y, 0);
			_positionState.Physics += transformed * moveSpeed * Root.Game.Dt;

			static Vector3 RotateVector(Vector3 vector, Matrix4x4 rotationMatrix)
			{
				Vector3 right = new(rotationMatrix.M11, rotationMatrix.M12, rotationMatrix.M13);
				Vector3 forward = -Vector3.Cross(Vector3.UnitY, right);
				return right * vector.X + forward * vector.Z;
			}
		}

		public void PreRender()
		{
			_positionState.PrepareRender();
			_rotationState.PrepareRender();

			Vector3 upDirection = Vector3.Transform(Vector3.UnitY, _rotationState.Render);
			Vector3 lookDirection = Vector3.Transform(Vector3.UnitZ, _rotationState.Render);
			ViewMatrix = Matrix4x4.CreateLookAt(_positionState.Render, _positionState.Render + lookDirection, upDirection);

			float aspectRatio = WindowWidth / (float)WindowHeight;

			const int fieldOfView = 2;
			const float nearPlaneDistance = 0.05f;
			const float farPlaneDistance = 10000f;
			Projection = Matrix4x4.CreatePerspectiveFieldOfView(MathF.PI / 4 * fieldOfView, aspectRatio, nearPlaneDistance, farPlaneDistance);
		}
	}
}
