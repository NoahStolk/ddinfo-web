using DevilDaggersInfo.App.Ui.SurvivalEditor.Enums;
using DevilDaggersInfo.App.Ui.SurvivalEditor.States;
using Silk.NET.GLFW;
using Warp;

namespace DevilDaggersInfo.App.Ui.SurvivalEditor.Editing.Arena;

public class ArenaBucketState : IArenaState
{
	public void Handle(int relMouseX, int relMouseY, int x, int y)
	{
		if (!Input.IsButtonPressed(MouseButton.Left))
			return;

		List<Vector2i<int>> done = new();
		float[,] tiles = StateManager.SpawnsetState.Spawnset.ArenaTiles.GetMutableClone();
		int dimension = StateManager.SpawnsetState.Spawnset.ArenaDimension;
		float targetHeight = StateManager.SpawnsetState.Spawnset.ArenaTiles[x, y];
		FillNeighbors(x, y);

		Components.SpawnsetArena.Arena.UpdateArena(tiles);
		SpawnsetHistoryManager.Save(SpawnsetEditType.ArenaBucket);

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
}
