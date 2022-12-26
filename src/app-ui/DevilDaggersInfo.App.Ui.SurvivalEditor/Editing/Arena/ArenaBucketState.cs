using DevilDaggersInfo.App.Ui.Base.StateManagement;
using DevilDaggersInfo.App.Ui.Base.StateManagement.SurvivalEditor.Actions;
using DevilDaggersInfo.App.Ui.Base.StateManagement.SurvivalEditor.Data;
using DevilDaggersInfo.App.Ui.SurvivalEditor.Editing.Arena.Data;
using Silk.NET.GLFW;
using Warp.NET;

namespace DevilDaggersInfo.App.Ui.SurvivalEditor.Editing.Arena;

public class ArenaBucketState : IArenaState
{
	public void Handle(ArenaMousePosition mousePosition)
	{
		if (!Input.IsButtonPressed(MouseButton.Left))
			return;

		List<Vector2i<int>> done = new();
		float[,] tiles = StateManager.SpawnsetState.Spawnset.ArenaTiles.GetMutableClone();
		int dimension = StateManager.SpawnsetState.Spawnset.ArenaDimension;
		float targetHeight = StateManager.SpawnsetState.Spawnset.ArenaTiles[mousePosition.Tile.X, mousePosition.Tile.Y];
		FillNeighbors(mousePosition.Tile.X, mousePosition.Tile.Y);

		Components.SpawnsetArena.Arena.UpdateArena(tiles);
		StateManager.Dispatch(new SaveHistory(SpawnsetEditType.ArenaBucket));

		void FillNeighbors(int x, int y)
		{
			tiles[x, y] = StateManager.ArenaEditorState.SelectedHeight;
			done.Add(new(x, y));

			int leftX = x - 1;
			int rightX = x + 1;
			int topY = y - 1;
			int bottomY = y + 1;

			if (leftX >= 0)
				FillIfApplicable(leftX, y);
			if (rightX < dimension)
				FillIfApplicable(rightX, y);
			if (topY >= 0)
				FillIfApplicable(x, topY);
			if (bottomY < dimension)
				FillIfApplicable(x, bottomY);

			void FillIfApplicable(int newX, int newY)
			{
				if (done.Contains(new(newX, newY)))
					return;

				float tileHeight = tiles[newX, newY];

				float clampedTargetHeight = targetHeight;
				if (targetHeight < StateManager.ArenaEditorState.BucketVoidHeight)
					clampedTargetHeight = StateManager.ArenaEditorState.BucketVoidHeight;
				if (tileHeight < StateManager.ArenaEditorState.BucketVoidHeight)
					tileHeight = StateManager.ArenaEditorState.BucketVoidHeight;

				if (MathF.Abs(tileHeight - clampedTargetHeight) < StateManager.ArenaEditorState.BucketTolerance)
					FillNeighbors(newX, newY);
			}
		}
	}

	public void Reset()
	{
	}

	public void Render(ArenaMousePosition mousePosition, Vector2i<int> origin, float depth)
	{
	}
}
