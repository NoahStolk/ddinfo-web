using DevilDaggersInfo.Core.Replay.Events.Interfaces;
using DevilDaggersInfo.Core.Spawnset;
using DevilDaggersInfo.Types.Core.Replays;

namespace DevilDaggersInfo.Core.Replay.PostProcessing.ReplaySimulation;

public static class ReplaySimulationBuilder
{
	public static ReplaySimulation Build(SpawnsetBinary spawnset, ReplayBinary<LocalReplayBinaryHeader> replay)
	{
		InitialInputsEvent initialInputsEvent = (InitialInputsEvent?)replay.EventsData.Events.FirstOrDefault(e => e is InitialInputsEvent) ?? throw new InvalidOperationException("Replay does not contain an initial inputs event.");
		float lookSpeed = initialInputsEvent.LookSpeed;

		int ticks = 0;
		PlayerContext playerContext = new(spawnset.ArenaTiles[SpawnsetBinary.ArenaDimensionMax / 2, SpawnsetBinary.ArenaDimensionMax / 2]);

		List<PlayerMovementSnapshot> playerMovementSnapshots = new() { new(playerContext.Rotation, playerContext.Position, true) };
		List<PlayerInputSnapshot> playerInputSnapshots = new();
		List<SoundSnapshot> soundSnapshots = new();

		foreach (IEvent e in replay.EventsData.Events)
		{
			switch (e)
			{
				case EntityPositionEvent { EntityId: 0 } entityPositionEvent:
				{
					const float divisor = 16f;
					playerContext.Position = new()
					{
						X = entityPositionEvent.Position.X / divisor,
						Y = entityPositionEvent.Position.Y / divisor,
						Z = entityPositionEvent.Position.Z / divisor,
					};
					break;
				}

				case IInputsEvent inputs:
				{
					ProcessInputs(spawnset, lookSpeed, inputs, playerContext, ticks);
					playerInputSnapshots.Add(new(inputs.Left, inputs.Right, inputs.Forward, inputs.Backward, inputs.Jump, inputs.Shoot, inputs.ShootHoming, inputs.MouseX, inputs.MouseY));
					ticks++;
					break;
				}

				default: continue;
			}

			playerMovementSnapshots.Add(new(playerContext.Rotation, playerContext.Position, playerContext.IsOnGround));
		}

		return new(playerMovementSnapshots, playerInputSnapshots, soundSnapshots);
	}

	private static void ProcessInputs(SpawnsetBinary spawnset, float lookSpeed, IInputsEvent inputs, PlayerContext playerContext, int ticks)
	{
		// Player movement constants
		const float velocityEpsilon = 0.01f;
		const float moveSpeed = 11.676f / 60f;
		const float gravityForce = 0.16f / 60f;

		// Orientation
		float yaw = lookSpeed * -inputs.MouseX;
		float pitch = lookSpeed * inputs.MouseY;
		pitch = Math.Clamp(pitch, ToRadians(-89.999f), ToRadians(89.999f));

		playerContext.Rotation *= Quaternion.CreateFromYawPitchRoll(yaw, -pitch, 0);

		// Position

		// Jumping

		// TODO: Dagger-jumping.

		// Find the highest of all 4 tiles.
		const float playerSize = 1f; // Guess
		float topLeft = GetTileHeightAtWorldPosition(playerContext.Position.X - playerSize, playerContext.Position.Z - playerSize);
		float topRight = GetTileHeightAtWorldPosition(playerContext.Position.X + playerSize, playerContext.Position.Z - playerSize);
		float bottomLeft = GetTileHeightAtWorldPosition(playerContext.Position.X - playerSize, playerContext.Position.Z + playerSize);
		float bottomRight = GetTileHeightAtWorldPosition(playerContext.Position.X + playerSize, playerContext.Position.Z + playerSize);

		float GetTileHeightAtWorldPosition(float positionX, float positionZ)
		{
			int arenaX = spawnset.WorldToTileCoordinate(positionX);
			int arenaZ = spawnset.WorldToTileCoordinate(positionZ);
			return arenaX is < 0 or > SpawnsetBinary.ArenaDimensionMax - 1 || arenaZ is < 0 or > SpawnsetBinary.ArenaDimensionMax - 1 ? -1000 : spawnset.GetActualTileHeight(arenaX, arenaZ, ticks / 60f);
		}

		float currentTileHeight = Math.Max(Math.Max(topLeft, topRight), Math.Max(bottomLeft, bottomRight));
		playerContext.IsOnGround = playerContext.Position.Y <= currentTileHeight;

		if (playerContext.IsOnGround)
		{
			playerContext.Position = playerContext.Position with { Y = currentTileHeight };

			playerContext.Gravity = 0;
			playerContext.VelocityY = 0;

			if (playerContext.JumpCooldown <= 0 && inputs.Jump is JumpType.StartedPress or JumpType.Hold)
			{
				playerContext.JumpCooldown = 10;// Guess
				playerContext.VelocityY = 0.35f;// Guess
				playerContext.SpeedBoost = 1.5f;// Guess

				// TODO: Use Jump2 when jump was not precise.
				// ReplaySound replaySound = ReplaySound.Jump3;
				// soundSnapshots.Add(new(ticks, replaySound, position));
			}
		}
		else
		{
			playerContext.Gravity -= gravityForce;
			playerContext.VelocityY += playerContext.Gravity;
		}

		playerContext.SpeedBoost += (1 - playerContext.SpeedBoost) / 10f; // Guess
		playerContext.JumpCooldown--;

		// WASD movement
		Vector2 GetWishDirection()
		{
			Vector3 axisAlignedWishDirection = new(Convert.ToInt32(inputs.Left) - Convert.ToInt32(inputs.Right), 0, Convert.ToInt32(inputs.Forward) - Convert.ToInt32(inputs.Backward));
			Vector3 wishDirection3d = Vector3.Transform(axisAlignedWishDirection, Matrix4x4.CreateFromQuaternion(playerContext.Rotation));
			Vector2 wishDirection = new(wishDirection3d.X, wishDirection3d.Z);

			return axisAlignedWishDirection.Length() < velocityEpsilon ? Vector2.Zero : Vector2.Normalize(wishDirection);
		}

		Vector2 wishDirection = GetWishDirection();

		float horizontalSpeed = playerContext.Velocity.Length();

		// TODO: When switching directions quickly, decrease accelerationAir by a lot. Air control should be controllable but this control should be lost when changing direction quickly.
		float addSpeed = Math.Clamp(moveSpeed - horizontalSpeed, 0, moveSpeed);
		playerContext.Velocity += addSpeed * wishDirection;

		// Update state
		playerContext.Position += new Vector3(playerContext.Velocity.X, playerContext.VelocityY, playerContext.Velocity.Y);
	}

	private static float ToRadians(float degrees) => degrees * (MathF.PI / 180f);

	private sealed class PlayerContext
	{
		public PlayerContext(float spawnHeight)
		{
			Position = new(0, spawnHeight, 0);
			Rotation = Quaternion.CreateFromAxisAngle(Vector3.UnitY, MathF.PI);
			IsOnGround = false;
			VelocityY = 0;
			Velocity = default;
			Gravity = 0;
			SpeedBoost = 1;
			JumpCooldown = 0;
		}

		public Quaternion Rotation { get; set; }
		public Vector3 Position { get; set; }
		public bool IsOnGround { get; set; }
		public float VelocityY { get; set; }
		public Vector2 Velocity { get; set; }
		public float Gravity { get; set; }
		public float SpeedBoost { get; set; }
		public int JumpCooldown { get; set; }
	}
}
