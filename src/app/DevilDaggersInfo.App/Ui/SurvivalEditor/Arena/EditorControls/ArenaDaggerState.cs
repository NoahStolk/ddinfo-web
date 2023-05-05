using DevilDaggersInfo.App.Engine.Maths.Numerics;
using DevilDaggersInfo.App.Ui.Base.StateManagement;
using DevilDaggersInfo.App.Ui.SurvivalEditor.State;
using DevilDaggersInfo.App.Ui.SurvivalEditor.Utils;
using DevilDaggersInfo.Core.Spawnset;
using ImGuiNET;
using System.Numerics;

namespace DevilDaggersInfo.App.Ui.SurvivalEditor.Arena.EditorControls;

public class ArenaDaggerState : IArenaState
{
	private bool _settingRaceDagger;
	private Vector2? _position;

	public void Handle(ArenaMousePosition mousePosition)
	{
		if (SpawnsetState.Spawnset.GameMode != GameMode.Race)
			return;

		if (ArenaChild.LeftMouse.JustPressed)
		{
			_settingRaceDagger = true;
			_position = GetSnappedDaggerPosition();
		}
		else if (ArenaChild.LeftMouse.Down)
		{
			if (_settingRaceDagger)
				_position = GetSnappedDaggerPosition();
		}
		else if (ArenaChild.LeftMouse.JustReleased)
		{
			if (!_position.HasValue)
				return;

			Vector2 tileCoordinate = _position.Value / ArenaChild.TileSize;
			Vector2 daggerPosition = new(SpawnsetState.Spawnset.TileToWorldCoordinate(tileCoordinate.X), SpawnsetState.Spawnset.TileToWorldCoordinate(tileCoordinate.Y));

			SpawnsetState.Spawnset = SpawnsetState.Spawnset with { RaceDaggerPosition = daggerPosition };
			SpawnsetHistoryUtils.Save(SpawnsetEditType.RaceDagger);

			Reset();
		}

		Vector2 GetSnappedDaggerPosition()
		{
			return ArenaEditingUtils.Snap(mousePosition.Real, StateManager.ArenaDaggerState.Snap * ArenaChild.TileSize);
		}
	}

	public void HandleOutOfRange(ArenaMousePosition mousePosition)
	{
		Reset();
	}

	private void Reset()
	{
		_settingRaceDagger = false;
		_position = null;
	}

	public void Render(ArenaMousePosition mousePosition)
	{
		if (!_position.HasValue)
			return;

		ImDrawListPtr drawList = ImGui.GetWindowDrawList();
		Vector2 origin = ImGui.GetCursorScreenPos();
		Vector2 center = origin + _position.Value + ArenaChild.HalfTileSizeAsVector2;
		drawList.AddCircle(center, 3, ImGui.GetColorU32(Color.HalfTransparentWhite));
		//Root.Game.SpriteRenderer.Schedule(new(-8, -8), origin.ToVector2() + _position.Value + Arena.HalfTileAsVector2, depth, ContentManager.Content.IconDaggerTexture, Color.HalfTransparentWhite);
	}
}
