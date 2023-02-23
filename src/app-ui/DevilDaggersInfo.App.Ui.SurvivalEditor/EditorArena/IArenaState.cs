using DevilDaggersInfo.App.Ui.SurvivalEditor.EditorArena.Data;

namespace DevilDaggersInfo.App.Ui.SurvivalEditor.EditorArena;

public interface IArenaState
{
	void Handle(ArenaMousePosition mousePosition);

	void Reset();

	void Render(ArenaMousePosition mousePosition, Vector2i<int> origin, float depth);
}
