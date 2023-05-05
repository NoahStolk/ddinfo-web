namespace DevilDaggersInfo.App.Ui.SurvivalEditor.Arena.EditorStates;

public interface IArenaState
{
	void Handle(ArenaMousePosition mousePosition);

	void HandleOutOfRange(ArenaMousePosition mousePosition);

	void Render(ArenaMousePosition mousePosition);
}
