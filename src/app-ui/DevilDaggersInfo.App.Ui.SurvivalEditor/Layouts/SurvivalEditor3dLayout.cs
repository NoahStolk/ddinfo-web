using DevilDaggersInfo.App.Ui.Base;
using DevilDaggersInfo.App.Ui.Base.DependencyPattern;
using DevilDaggersInfo.App.Ui.Base.DependencyPattern.Inversion.Layouts.SurvivalEditor;
using DevilDaggersInfo.App.Ui.Base.States;
using DevilDaggersInfo.App.Ui.SurvivalEditor.States;
using Silk.NET.GLFW;
using Silk.NET.OpenGL;
using Warp;
using Warp.Content;
using Warp.Extensions;
using Warp.InterpolationStates;
using Warp.Ui;
using Warp.Utils;
using Shader = Warp.Content.Shader;

namespace DevilDaggersInfo.App.Ui.SurvivalEditor.Layouts;

public class SurvivalEditor3dLayout : Layout, ISurvivalEditor3dLayout
{
	private readonly Camera _camera = new();
	private readonly List<MeshObject> _tiles = new();
	private readonly List<MeshObject> _pillars = new();

	private uint _tileVao;
	private uint _pillarVao;

	public SurvivalEditor3dLayout()
		: base(Constants.Full)
	{
	}

	public unsafe void Initialize()
	{
		_tileVao = CreateVao(ContentManager.Content.TileMesh);
		_pillarVao = CreateVao(ContentManager.Content.PillarMesh);

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

	public void BuildScene()
	{
		_tiles.Clear();
		_pillars.Clear();

		int halfSize = StateManager.SpawnsetState.Spawnset.ArenaDimension / 2;
		for (int i = 0; i < StateManager.SpawnsetState.Spawnset.ArenaDimension; i++)
		{
			for (int j = 0; j < StateManager.SpawnsetState.Spawnset.ArenaDimension; j++)
			{
				float y = StateManager.SpawnsetState.Spawnset.ArenaTiles[i, j];
				if (y < -2)
					continue;

				float x = (i - halfSize) * 4;
				float z = (j - halfSize) * 4;
				_tiles.Add(new(_tileVao, ContentManager.Content.TileMesh, Vector3.One, Quaternion.Identity, new(x, y, z)));
				_pillars.Add(new(_pillarVao, ContentManager.Content.PillarMesh, Vector3.One, Quaternion.Identity, new(x, y, z)));
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

		Shaders.Mesh.Use();
		Shader.SetMatrix4x4(MeshUniforms.View, _camera.ViewMatrix);
		Shader.SetMatrix4x4(MeshUniforms.Projection, _camera.Projection);
		Shader.SetInt(MeshUniforms.TextureDiffuse, 0);

		ContentManager.Content.TileTexture.Use();
		foreach (MeshObject tile in _tiles)
			tile.Render();

		ContentManager.Content.PillarTexture.Use();
		foreach (MeshObject pillar in _pillars)
			pillar.Render();
	}

	public void Render()
	{
	}

	private sealed class MeshObject
	{
		private readonly uint _vao;
		private readonly Mesh _mesh;
		private readonly Matrix4x4 _modelMatrix;

		public MeshObject(uint vao, Mesh mesh, Vector3 scale, Quaternion rotation, Vector3 position)
		{
			_vao = vao;
			_mesh = mesh;

			Matrix4x4 scaleMatrix = Matrix4x4.CreateScale(scale);
			Matrix4x4 rotationMatrix = Matrix4x4.CreateFromQuaternion(rotation);
			Matrix4x4 translationMatrix = Matrix4x4.CreateTranslation(position);
			_modelMatrix = scaleMatrix * rotationMatrix * translationMatrix;
		}

		public unsafe void Render()
		{
			Shader.SetMatrix4x4(MeshUniforms.Model, _modelMatrix);

			Gl.BindVertexArray(_vao);
			fixed (uint* i = &_mesh.Indices[0])
				Gl.DrawElements(PrimitiveType.Triangles, (uint)_mesh.Indices.Length, DrawElementsType.UnsignedInt, i);
			Gl.BindVertexArray(0);
		}
	}

	private sealed class Camera
	{
		private readonly Vector3State _positionState = new(new(0, 4, 0));
		private readonly QuaternionState _rotationState = new(Quaternion.Identity);

		private Vector3 _axisAlignedSpeed;
		private float _yaw;
		private float _pitch;
		private Vector2i<int> _lockedMousePosition;

		public Matrix4x4 Projection { get; private set; }
		public Matrix4x4 ViewMatrix { get; private set; }

		public void Update()
		{
			_positionState.PrepareUpdate();
			_rotationState.PrepareUpdate();

			HandleKeys();
			HandleMouse();

			const float moveSpeed = 25;

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

		private void HandleKeys()
		{
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
		}

		private unsafe void HandleMouse()
		{
			Vector2i<int> mousePosition = Input.GetMousePosition().FloorToVector2Int32();
			if (Input.IsButtonPressed(MouseButton.Left))
			{
				_lockedMousePosition = mousePosition;
				Graphics.Glfw.SetInputMode(Window, CursorStateAttribute.Cursor, CursorModeValue.CursorHidden);
			}
			else if (Input.IsButtonReleased(MouseButton.Left))
			{
				Graphics.Glfw.SetInputMode(Window, CursorStateAttribute.Cursor, CursorModeValue.CursorNormal);
			}
			else if (Input.IsButtonHeld(MouseButton.Left) && mousePosition != _lockedMousePosition)
			{
				const float lookSpeed = 20;

				Vector2i<int> delta = mousePosition - _lockedMousePosition;
				_yaw -= lookSpeed * delta.X * 0.0001f;
				_pitch -= lookSpeed * delta.Y * 0.0001f;

				_pitch = Math.Clamp(_pitch, MathUtils.ToRadians(-89.9f), MathUtils.ToRadians(89.9f));
				_rotationState.Physics = Quaternion.CreateFromYawPitchRoll(_yaw, -_pitch, 0);

				Graphics.Glfw.SetCursorPos(Window, _lockedMousePosition.X, _lockedMousePosition.Y);
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
