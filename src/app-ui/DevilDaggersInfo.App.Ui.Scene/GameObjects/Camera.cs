using DevilDaggersInfo.App.Ui.Base.DependencyPattern;
using Silk.NET.GLFW;
using Warp.NET;
using Warp.NET.Extensions;
using Warp.NET.InterpolationStates;
using Warp.NET.Utils;

namespace DevilDaggersInfo.App.Ui.Scene.GameObjects;

public class Camera
{
	private const float _defaultYaw = MathF.PI;
	private const MouseButton _lookButton = MouseButton.Right;

	private readonly QuaternionState _rotationState = new(Quaternion.CreateFromYawPitchRoll(_defaultYaw, 0, 0));

	private Vector3 _axisAlignedSpeed;
	private float _yaw;
	private float _pitch;
	private Vector2i<int>? _lockedMousePosition;

	public Matrix4x4 Projection { get; private set; }
	public Matrix4x4 ViewMatrix { get; private set; }

	public Vector3State PositionState { get; } = new(default);

	public void Reset(Vector3 position)
	{
		PositionState.Physics = position;
		_rotationState.Physics = Quaternion.CreateFromYawPitchRoll(_defaultYaw, 0, 0);
		_yaw = _defaultYaw;
		_pitch = 0;
		_lockedMousePosition = null;
	}

	public void Update()
	{
		PositionState.PrepareUpdate();
		_rotationState.PrepareUpdate();

		HandleKeys();
		HandleMouse();

		const float moveSpeed = 25;

		Matrix4x4 rotMat = Matrix4x4.CreateFromQuaternion(_rotationState.Physics);
		Vector3 transformed = RotateVector(_axisAlignedSpeed, rotMat) + new Vector3(0, _axisAlignedSpeed.Y, 0);
		PositionState.Physics += transformed * moveSpeed * Root.Game.Dt;

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
		if (Input.IsButtonPressed(_lookButton))
		{
			_lockedMousePosition = mousePosition;
			Graphics.Glfw.SetInputMode(Window, CursorStateAttribute.Cursor, CursorModeValue.CursorHidden);
		}
		else if (Input.IsButtonReleased(_lookButton))
		{
			_lockedMousePosition = null;
			Graphics.Glfw.SetInputMode(Window, CursorStateAttribute.Cursor, CursorModeValue.CursorNormal);
		}
		else if (Input.IsButtonHeld(_lookButton) && _lockedMousePosition.HasValue && mousePosition != _lockedMousePosition)
		{
			const float lookSpeed = 20;

			Vector2i<int> delta = mousePosition - _lockedMousePosition.Value;
			_yaw -= lookSpeed * delta.X * 0.0001f;
			_pitch -= lookSpeed * delta.Y * 0.0001f;

			_pitch = Math.Clamp(_pitch, MathUtils.ToRadians(-89.9f), MathUtils.ToRadians(89.9f));
			_rotationState.Physics = Quaternion.CreateFromYawPitchRoll(_yaw, -_pitch, 0);

			Graphics.Glfw.SetCursorPos(Window, _lockedMousePosition.Value.X, _lockedMousePosition.Value.Y);
		}
	}

	public void PreRender()
	{
		PositionState.PrepareRender();
		_rotationState.PrepareRender();

		Vector3 upDirection = Vector3.Transform(Vector3.UnitY, _rotationState.Render);
		Vector3 lookDirection = Vector3.Transform(Vector3.UnitZ, _rotationState.Render);
		ViewMatrix = Matrix4x4.CreateLookAt(PositionState.Render, PositionState.Render + lookDirection, upDirection);

		float aspectRatio = CurrentWindowState.Width / (float)CurrentWindowState.Height;

		const int fieldOfView = 2;
		const float nearPlaneDistance = 0.05f;
		const float farPlaneDistance = 10000f;
		Projection = Matrix4x4.CreatePerspectiveFieldOfView(MathF.PI / 4 * fieldOfView, aspectRatio, nearPlaneDistance, farPlaneDistance);
	}
}