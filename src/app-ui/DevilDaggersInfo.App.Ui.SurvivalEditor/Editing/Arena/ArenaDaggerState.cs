using DevilDaggersInfo.App.Ui.SurvivalEditor.Enums;
using DevilDaggersInfo.App.Ui.SurvivalEditor.States;
using DevilDaggersInfo.Types.Core.Spawnsets;
using Silk.NET.GLFW;
using Warp;

namespace DevilDaggersInfo.App.Ui.SurvivalEditor.Editing.Arena;

public class ArenaDaggerState : IArenaState
{
	private bool _settingRaceDagger;

	public void Handle(int relMouseX, int relMouseY, int x, int y)
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
					RaceDaggerPosition = new(StateManager.SpawnsetState.Spawnset.TileToWorldCoordinate(x), StateManager.SpawnsetState.Spawnset.TileToWorldCoordinate(y)),
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
}
