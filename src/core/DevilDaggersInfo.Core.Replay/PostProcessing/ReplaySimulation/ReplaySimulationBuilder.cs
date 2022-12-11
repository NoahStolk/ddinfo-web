using DevilDaggersInfo.Core.Replay.Events.Interfaces;
using DevilDaggersInfo.Core.Spawnset;
using DevilDaggersInfo.Types.Core.Replays;

namespace DevilDaggersInfo.Core.Replay.PostProcessing.ReplaySimulation;

public static class ReplaySimulationBuilder
{
	public static ReplaySimulation Build(SpawnsetBinary spawnset, ReplayBinary<LocalReplayBinaryHeader> replay)
	{
		float spawnTileHeight = spawnset.ArenaTiles[SpawnsetBinary.ArenaDimensionMax / 2, SpawnsetBinary.ArenaDimensionMax / 2];

		const float playerHeightForVisualization = 4;
		Quaternion rotation = Quaternion.CreateFromAxisAngle(Vector3.UnitY, MathF.PI);
		Vector3 position = new(0, spawnTileHeight + playerHeightForVisualization, 0);

		int ticks = 0;
		float velocityY = 0;
		float velocityX = 0;
		float velocityZ = 0;
		float gravity = 0;
		float speedBoost = 1;
		int jumpCooldown = 0;

		InitialInputsEvent initialInputsEvent = (InitialInputsEvent?)replay.EventsData.Events.FirstOrDefault(e => e is InitialInputsEvent) ?? throw new InvalidOperationException("Replay does not contain an initial inputs event.");
		float lookSpeed = initialInputsEvent.LookSpeed;

		List<PlayerMovementSnapshot> playerMovementSnapshots = new() { new(rotation, position) };
		List<SoundSnapshot> soundSnapshots = new();

		foreach (IEvent e in replay.EventsData.Events)
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
					float yaw = lookSpeed * -inputs.MouseX;
					float pitch = lookSpeed * inputs.MouseY;
					pitch = Math.Clamp(pitch, ToRadians(-89.999f), ToRadians(89.999f));

					rotation *= Quaternion.CreateFromYawPitchRoll(yaw, -pitch, 0);

					// Position

					// Jumping

					// TODO: Dagger-jumping.

					// Find the highest of all 4 tiles.
					const float playerSize = 1f; // Guess
					float topLeft = GetTileHeightAtWorldPosition(position.X - playerSize, position.Z - playerSize);
					float topRight = GetTileHeightAtWorldPosition(position.X + playerSize, position.Z - playerSize);
					float bottomLeft = GetTileHeightAtWorldPosition(position.X - playerSize, position.Z + playerSize);
					float bottomRight = GetTileHeightAtWorldPosition(position.X + playerSize, position.Z + playerSize);

					float GetTileHeightAtWorldPosition(float positionX, float positionZ)
					{
						int arenaX = spawnset.WorldToTileCoordinate(positionX);
						int arenaZ = spawnset.WorldToTileCoordinate(positionZ);
						return arenaX is < 0 or > SpawnsetBinary.ArenaDimensionMax - 1 || arenaZ is < 0 or > SpawnsetBinary.ArenaDimensionMax - 1 ? -1000 : spawnset.GetActualTileHeight(arenaX, arenaZ, ticks / 60f);
					}

					float currentTileHeight = Math.Max(Math.Max(topLeft, topRight), Math.Max(bottomLeft, bottomRight));
					bool isOnGround = currentTileHeight + 4 > position.Y;

					if (isOnGround)
					{
						gravity = 0;
						velocityY = 0;

						if (jumpCooldown <= 0 && inputs.Jump is JumpType.StartedPress or JumpType.Hold)
						{
							jumpCooldown = 10;
							velocityY = 1;
							speedBoost = 1.5f;
							ReplaySound replaySound = ReplaySound.Jump3;
							soundSnapshots.Add(new(ticks, replaySound, position));
						}
					}
					else
					{
						gravity -= 0.0025f;
						velocityY += gravity;
					}

					speedBoost += (1 - speedBoost) / 10f;
					jumpCooldown--;

					// WASD movement
					const float moveSpeed = 12 / 60f;
					const float acceleration = 0.1f;
					const float friction = 10f;
					const float airAcceleration = 0.01f;
					const float airFriction = 100f;
					if (inputs.Right)
						velocityX -= isOnGround ? acceleration : airAcceleration;
					else if (inputs.Left)
						velocityX += isOnGround ? acceleration : airAcceleration;
					else
						velocityX -= velocityX / (isOnGround ? friction : airFriction);

					if (inputs.Forward)
						velocityZ += isOnGround ? acceleration : airAcceleration;
					else if (inputs.Backward)
						velocityZ -= isOnGround ? acceleration : airAcceleration;
					else
						velocityZ -= velocityZ / (isOnGround ? friction : airFriction);

					velocityX = Math.Clamp(velocityX, -speedBoost, speedBoost);
					velocityZ = Math.Clamp(velocityZ, -speedBoost, speedBoost);

					// Update state
					Vector3 axisAlignedMovement = new(velocityX, velocityY, velocityZ);
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

			playerMovementSnapshots.Add(new(rotation, position));
		}

		return new(playerMovementSnapshots, soundSnapshots);
	}

	private static float ToRadians(float degrees) => degrees * (MathF.PI / 180f);
}
