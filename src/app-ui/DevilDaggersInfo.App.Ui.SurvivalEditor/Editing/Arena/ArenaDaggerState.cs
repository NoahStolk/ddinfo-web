using DevilDaggersInfo.App.Ui.Base.StateManagement.SurvivalEditor.Data;
using DevilDaggersInfo.App.Ui.SurvivalEditor.Editing.Arena.Data;
using DevilDaggersInfo.App.Ui.SurvivalEditor.States;
using DevilDaggersInfo.Types.Core.Spawnsets;
using Silk.NET.GLFW;
using Warp.NET;

namespace DevilDaggersInfo.App.Ui.SurvivalEditor.Editing.Arena;

public class ArenaDaggerState : IArenaState
{
	private bool _settingRaceDagger;

	public void Handle(ArenaMousePosition mousePosition)
	{
		if (StateManager.SpawnsetState.Spawnset.GameMode != GameMode.Race)
			return;

		if (Input.IsButtonPressed(MouseButton.Left))
		{
			_settingRaceDagger = true;
		}
		else if (Input.IsButtonHeld(MouseButton.Left))
		{
			if (_settingRaceDagger)
			{
				StateManager.SetSpawnset(StateManager.SpawnsetState.Spawnset with
				{
					RaceDaggerPosition = new(StateManager.SpawnsetState.Spawnset.TileToWorldCoordinate(mousePosition.Tile.X), StateManager.SpawnsetState.Spawnset.TileToWorldCoordinate(mousePosition.Tile.Y)),
				});
			}
		}
		else if (Input.IsButtonReleased(MouseButton.Left))
		{
			SpawnsetHistoryManager.Save(SpawnsetEditType.RaceDagger);
			_settingRaceDagger = false;
		}
	}

	public void Reset()
	{
		_settingRaceDagger = false;
	}

	public void Render(ArenaMousePosition mousePosition, Vector2i<int> origin, float depth)
	{
	}
}
