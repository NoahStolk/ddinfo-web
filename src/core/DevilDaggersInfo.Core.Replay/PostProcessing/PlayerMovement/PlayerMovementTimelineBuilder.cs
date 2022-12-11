using DevilDaggersInfo.Core.Replay.Events.Interfaces;

namespace DevilDaggersInfo.Core.Replay.PostProcessing.PlayerMovement;

public static class PlayerMovementTimelineBuilder
{
	public static PlayerMovementTimeline Build(float spawnTileHeight, ReplayEventsData replayEventsData)
	{
		const float playerHeightForVisualization = 4;
		InitialInputsEvent initialInputsEvent = (InitialInputsEvent?)replayEventsData.Events.FirstOrDefault(e => e is InitialInputsEvent) ?? throw new InvalidOperationException("Replay does not contain an initial inputs event.");
		float lookSpeed = initialInputsEvent.LookSpeed;

		int ticks = 0;
		Quaternion rotation = Quaternion.CreateFromAxisAngle(Vector3.UnitY, MathF.PI);
		Vector3 position = new(0, spawnTileHeight + playerHeightForVisualization, 0);
		float velocityX = 0;
		float velocityZ = 0;

		List<PlayerMovementSnapshot> snapshots = new()
		{
			new(0, rotation, position),
		};

		foreach (IEvent e in replayEventsData.Events)
		{
			switch (e)
			{
				case EntityPositionEvent { EntityId: 0 } entityPositionEvent:
				{
					const float divisor = 16f;
					position = new()
					{
						X = entityPositionEvent.Position.X / divisor,
						Y = entityPositionEvent.Position.Y / divisor + playerHeightForVisualization,
						Z = entityPositionEvent.Position.Z / divisor,
					};
					break;
				}

				case IInputsEvent inputs:
				{
					// Orientation
					const float multiplier = 1; // Guess
					float yaw = lookSpeed * inputs.MouseX * multiplier;
					float pitch = lookSpeed * inputs.MouseY * multiplier;
					pitch = Math.Clamp(pitch, ToRadians(-89.9f), ToRadians(89.9f));

					rotation *= Quaternion.CreateFromYawPitchRoll(yaw, -pitch, 0);

					// Position
					const float moveSpeed = 12 / 60f;
					const float acceleration = 0.1f;
					const float friction = 10f;
					if (inputs.Right)
						velocityX += acceleration;
					else if (inputs.Left)
						velocityX -= acceleration;
					else
						velocityX -= velocityX / 10f;

					if (inputs.Forward)
						velocityZ += acceleration;
					else if (inputs.Backward)
						velocityZ -= acceleration;
					else
						velocityZ -= velocityZ / friction;

					velocityX = Math.Clamp(velocityX, -1, 1);
					velocityZ = Math.Clamp(velocityZ, -1, 1);

					Vector3 axisAlignedMovement = new(velocityX, 0, velocityZ); // TODO: Set Y to jump/gravity velocity.
					Matrix4x4 rotMat = Matrix4x4.CreateFromQuaternion(rotation);
					Vector3 transformed = RotateVector(axisAlignedMovement, rotMat) + new Vector3(0, axisAlignedMovement.Y, 0);
					position += transformed * moveSpeed;

					static Vector3 RotateVector(Vector3 vector, Matrix4x4 rotationMatrix)
					{
						Vector3 right = new(rotationMatrix.M11, rotationMatrix.M12, rotationMatrix.M13);
						Vector3 forward = -Vector3.Cross(Vector3.UnitY, right);
						return right * vector.X + forward * vector.Z;
					}

					ticks++;
					break;
				}

				default: continue;
			}

			snapshots.Add(new(ticks / 60f, rotation, position));
		}

		return new(snapshots);
	}

	private static float ToRadians(float degrees) => degrees * (MathF.PI / 180f);
}
