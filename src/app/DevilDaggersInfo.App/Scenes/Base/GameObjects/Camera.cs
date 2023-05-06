using DevilDaggersInfo.App.Engine.Extensions;
using DevilDaggersInfo.App.Engine.InterpolationStates;
using DevilDaggersInfo.App.Engine.Intersections;
using DevilDaggersInfo.App.Engine.Maths;
using DevilDaggersInfo.App.Engine.Maths.Numerics;
using DevilDaggersInfo.App.Ui.Base.User.Settings;
using DevilDaggersInfo.App.Ui.Main;
using Silk.NET.Input;
using Silk.NET.Windowing;
using System.Numerics;

namespace DevilDaggersInfo.App.Scenes.Base.GameObjects;

public class Camera
{
	private const float _defaultYaw = MathF.PI;
	private const MouseButton _lookButton = MouseButton.Right;

	private readonly QuaternionState _rotationState = new(Quaternion.CreateFromYawPitchRoll(_defaultYaw, 0, 0));

	private readonly IWindow _window;
	private readonly IKeyboard _keyboard;
	private readonly IMouse _mouse;

	private Vector3 _axisAlignedSpeed;
	private float _yaw;
	private float _pitch;
	private Vector2i<int>? _lockedMousePosition;

	public Camera(IWindow window, IInputContext inputContext)
	{
		if (inputContext.Keyboards.Count == 0)
			throw new InvalidOperationException("No keyboard found.");

		if (inputContext.Mice.Count == 0)
			throw new InvalidOperationException("No mouse found.");

		_window = window;
		_keyboard = inputContext.Keyboards[0];
		_mouse = inputContext.Mice[0];

		_mouse.MouseDown += OnMouseDown;
		_mouse.MouseUp += OnMouseUp;
	}

	public Matrix4x4 Projection { get; private set; }
	public Matrix4x4 ViewMatrix { get; private set; }

	public Vector3State PositionState { get; } = new(default);

	public bool IsMenuCamera { get; set; } = true;

	public void Reset(Vector3 position)
	{
		PositionState.Physics = position;
		_rotationState.Physics = Quaternion.CreateFromYawPitchRoll(_defaultYaw, 0, 0);
		_yaw = _defaultYaw;
		_pitch = 0;
		_lockedMousePosition = null;
	}

	public void Update(float delta)
	{
		PositionState.PrepareUpdate();
		_rotationState.PrepareUpdate();

		if (IsMenuCamera)
		{
			float time = (float)_window.Time * 0.7f;
			PositionState.Physics = new(MathF.Sin(time) * 5, 6, MathF.Cos(time) * 5);
			_rotationState.Physics = Quaternion.CreateFromRotationMatrix(SetRotationFromDirectionalVector(new Vector3(0, 4, 0) - PositionState.Physics));
			return;
		}

		HandleKeys(delta);
		HandleMouse();

		const float moveSpeed = 25;

		Matrix4x4 rotMat = Matrix4x4.CreateFromQuaternion(_rotationState.Physics);
		Vector3 transformed = RotateVector(_axisAlignedSpeed, rotMat) + new Vector3(0, _axisAlignedSpeed.Y, 0);
		PositionState.Physics += transformed * moveSpeed * delta;

		static Vector3 RotateVector(Vector3 vector, Matrix4x4 rotationMatrix)
		{
			Vector3 right = new(rotationMatrix.M11, rotationMatrix.M12, rotationMatrix.M13);
			Vector3 forward = -Vector3.Cross(Vector3.UnitY, right);
			return right * vector.X + forward * vector.Z;
		}
	}

	private void HandleKeys(float delta)
	{
		const float acceleration = 20;
		const float friction = 20;
		const Key forwardInput = Key.W;
		const Key leftInput = Key.A;
		const Key backwardInput = Key.S;
		const Key rightInput = Key.D;
		const Key upInput = Key.Space;
		const Key downInput = Key.ShiftLeft;
		bool forwardHold = _keyboard.IsKeyPressed(forwardInput);
		bool leftHold = _keyboard.IsKeyPressed(leftInput);
		bool backwardHold = _keyboard.IsKeyPressed(backwardInput);
		bool rightHold = _keyboard.IsKeyPressed(rightInput);
		bool upHold = _keyboard.IsKeyPressed(upInput);
		bool downHold = _keyboard.IsKeyPressed(downInput);

		float accelerationDt = acceleration * delta;
		float frictionDt = friction * delta;

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

	private void OnMouseDown(IMouse mouse, MouseButton mouseButton)
	{
		if (mouseButton != _lookButton)
			return;

		_lockedMousePosition = _mouse.Position.FloorToVector2Int32();
		_mouse.Cursor.CursorMode = CursorMode.Hidden;
	}

	private void OnMouseUp(IMouse mouse, MouseButton mouseButton)
	{
		_lockedMousePosition = null;
		_mouse.Cursor.CursorMode = CursorMode.Normal;
	}

	private void HandleMouse()
	{
		Vector2i<int> mousePosition = _mouse.Position.FloorToVector2Int32();
		if (!_mouse.IsButtonPressed(_lookButton) || !_lockedMousePosition.HasValue || mousePosition == _lockedMousePosition)
			return;

		float lookSpeed = UserSettings.Model.LookSpeed;

		Vector2i<int> delta = mousePosition - _lockedMousePosition.Value;
		_yaw -= lookSpeed * delta.X * 0.0001f;
		_pitch -= lookSpeed * delta.Y * 0.0001f;

		_pitch = Math.Clamp(_pitch, MathUtils.ToRadians(-89.9f), MathUtils.ToRadians(89.9f));
		_rotationState.Physics = Quaternion.CreateFromYawPitchRoll(_yaw, -_pitch, 0);

		_mouse.Position = new(_lockedMousePosition.Value.X, _lockedMousePosition.Value.Y);
	}

	public void PreRender()
	{
		PositionState.PrepareRender();
		_rotationState.PrepareRender();

		Vector3 upDirection = Vector3.Transform(Vector3.UnitY, _rotationState.Render);
		Vector3 lookDirection = Vector3.Transform(Vector3.UnitZ, _rotationState.Render);
		ViewMatrix = Matrix4x4.CreateLookAt(PositionState.Render, PositionState.Render + lookDirection, upDirection);

		float aspectRatio = _window.Size.X / (float)_window.Size.Y;

		const float nearPlaneDistance = 0.05f;
		const float farPlaneDistance = 10000f;
		Projection = Matrix4x4.CreatePerspectiveFieldOfView(MathUtils.ToRadians(UserSettings.Model.FieldOfView), aspectRatio, nearPlaneDistance, farPlaneDistance);
	}

	private static Matrix4x4 SetRotationFromDirectionalVector(Vector3 direction)
	{
		Vector3 m3 = Vector3.Normalize(direction);
		Vector3 m1 = Vector3.Normalize(Vector3.Cross(Vector3.UnitY, m3));
		Vector3 m2 = Vector3.Normalize(Vector3.Cross(m3, m1));

		Matrix4x4 matrix = Matrix4x4.Identity;

		matrix.M11 = m1.X;
		matrix.M12 = m1.Y;
		matrix.M13 = m1.Z;

		matrix.M21 = m2.X;
		matrix.M22 = m2.Y;
		matrix.M23 = m2.Z;

		matrix.M31 = m3.X;
		matrix.M32 = m3.Y;
		matrix.M33 = m3.Z;

		return matrix;
	}

	public Ray ScreenToWorldPoint()
	{
		int screenWidth = _window.Size.X;
		int screenHeight = _window.Size.Y;
		float aspectRatio = _window.Size.X / (float)_window.Size.Y;

		// Remap so (0, 0) is the center of the window and the edges are at -0.5 and +0.5.
		Vector2 mousePosition = _mouse.Position;
		Vector2 relative = -new Vector2(mousePosition.X / screenWidth - 0.5f, mousePosition.Y / screenHeight - 0.5f);

		// Angle in radians from the view axis to the top plane of the view pyramid.
		float verticalAngle = 0.5f * MathUtils.ToRadians(UserSettings.Model.FieldOfView);

		// World space height of the view pyramid measured at 1m depth from the camera.
		float worldHeight = 2f * MathF.Tan(verticalAngle);

		// Convert relative position to world units.
		Vector2 temp = relative * worldHeight;
		Vector3 worldUnits = new(temp.X * aspectRatio, temp.Y, 1);

		// Rotate to match camera orientation.
		Vector3 direction = Vector3.Transform(worldUnits, _rotationState.Physics);

		// Output a ray from camera position, along this direction.
		return new(PositionState.Physics, direction);
	}
}
