using DevilDaggersInfo.Core.Spawnset;
using DevilDaggersInfo.Types.Core.Spawnsets;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace DevilDaggersInfo.Razor.Core.CanvasArena.Components;

public partial class WebAssemblySpawnsetArena
{
	private WebAssemblyCanvasArena? _context;

	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		await base.OnAfterRenderAsync(firstRender);

		_context = new(_canvasId);

		Render();
	}

	[JSInvokable]
	public void OnResize(double wrapperSize)
	{
		Resize(wrapperSize);
		Render();
	}

	private void Render()
	{
		if (_context == null)
			return;

		int[] colors = new int[SpawnsetBinary.ArenaDimension * SpawnsetBinary.ArenaDimension];
		for (int i = 0; i < SpawnsetBinary.ArenaDimension; i++)
		{
			for (int j = 0; j < SpawnsetBinary.ArenaDimension; j++)
			{
				float tileHeight = SpawnsetBinary.ArenaTiles[i, j];
				if (tileHeight < -1)
					continue;

				float actualTileHeight = SpawnsetBinary.GetActualTileHeight(i, j, CurrentTime);
				colors[i * SpawnsetBinary.ArenaDimension + j] = Color.GetColorFromHeight(actualTileHeight).ToInt();
			}
		}

		float shrinkEndTime = SpawnsetBinary.GetShrinkEndTime();
		float shrinkRadius = shrinkEndTime == 0 ? SpawnsetBinary.ShrinkStart : Math.Max(SpawnsetBinary.ShrinkStart - CurrentTime / shrinkEndTime * (SpawnsetBinary.ShrinkStart - SpawnsetBinary.ShrinkEnd), SpawnsetBinary.ShrinkEnd);

		(_, float? y, _) = SpawnsetBinary.GetRaceDaggerTilePosition();
		_context.DrawArena(colors, CanvasSize, shrinkRadius, SpawnsetBinary.GameMode == GameMode.Race && y.HasValue, SpawnsetBinary.RaceDaggerPosition.X, SpawnsetBinary.RaceDaggerPosition.Y);
	}
}
