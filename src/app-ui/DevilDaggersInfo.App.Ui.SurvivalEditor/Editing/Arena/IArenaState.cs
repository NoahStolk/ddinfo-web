using DevilDaggersInfo.App.Ui.SurvivalEditor.Editing.Arena.Data;

namespace DevilDaggersInfo.App.Ui.SurvivalEditor.Editing.Arena;

public interface IArenaState
{
	void Handle(ArenaMousePosition mousePosition);

	void Reset();

	void Render(ArenaMousePosition mousePosition, Vector2i<int> origin, float depth);
}
