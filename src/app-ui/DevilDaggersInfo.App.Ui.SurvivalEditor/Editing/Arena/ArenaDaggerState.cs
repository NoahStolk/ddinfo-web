using DevilDaggersInfo.App.Ui.Base;
using DevilDaggersInfo.App.Ui.Base.DependencyPattern;
using DevilDaggersInfo.App.Ui.Base.StateManagement;
using DevilDaggersInfo.App.Ui.Base.StateManagement.SurvivalEditor.Actions;
using DevilDaggersInfo.App.Ui.SurvivalEditor.Editing.Arena.Data;
using DevilDaggersInfo.App.Ui.SurvivalEditor.Utils;
using DevilDaggersInfo.Types.Core.Spawnsets;
using Silk.NET.GLFW;
using Warp.NET;
using Warp.NET.Extensions;

namespace DevilDaggersInfo.App.Ui.SurvivalEditor.Editing.Arena;

public class ArenaDaggerState : IArenaState
{
	private bool _settingRaceDagger;
	private Vector2? _position;

	public void Handle(ArenaMousePosition mousePosition)
	{
		if (StateManager.SpawnsetState.Spawnset.GameMode != GameMode.Race)
			return;

		if (Input.IsButtonPressed(MouseButton.Left))
		{
			_settingRaceDagger = true;
			_position = GetDaggerWorldPositionFromMouse();
		}
		else if (Input.IsButtonHeld(MouseButton.Left))
		{
			if (_settingRaceDagger)
				_position = GetDaggerWorldPositionFromMouse();
		}
		else if (Input.IsButtonReleased(MouseButton.Left))
		{
			if (!_position.HasValue)
				return;

			StateManager.Dispatch(new UpdateRaceDaggerPosition(_position.Value));

			Reset();
		}

		Vector2 GetDaggerWorldPositionFromMouse()
		{
			const float displayTileSize = Components.SpawnsetArena.Arena.TileSize;
			Vector2 fractionTile = ArenaEditingUtils.Snap(new Vector2(mousePosition.Real.X / displayTileSize, mousePosition.Real.Y / displayTileSize) - new Vector2(0.5f), StateManager.ArenaDaggerState.Snap);

			int arenaMiddle = StateManager.SpawnsetState.Spawnset.ArenaDimension / 2;
			return new((fractionTile.X - arenaMiddle) * 4, (fractionTile.Y - arenaMiddle) * 4);
		}
	}

	public void Reset()
	{
		_settingRaceDagger = false;
		_position = null;
	}

	public void Render(ArenaMousePosition mousePosition, Vector2i<int> origin, float depth)
	{
		if (!_position.HasValue)
			return;

		int arenaMiddle = StateManager.SpawnsetState.Spawnset.ArenaDimension / 2;
		float realRaceX = _position.Value.X / 4f + arenaMiddle;
		float realRaceZ = _position.Value.Y / 4f + arenaMiddle;

		const int tileSize = Components.SpawnsetArena.Arena.TileSize;
		const int halfSize = tileSize / 2;

		Root.Game.SpriteRenderer.Schedule(new(-8, -8), origin.ToVector2() + new Vector2(realRaceX * tileSize + halfSize, realRaceZ * tileSize + halfSize), depth, ContentManager.Content.IconDaggerTexture, Color.HalfTransparentWhite);
	}
}
