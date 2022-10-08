namespace DevilDaggersInfo.App.Ui.SurvivalEditor.Editing.Arena;

public interface IArenaState
{
	void Handle(int relMouseX, int relMouseY, int x, int y);

	void Reset();
}
