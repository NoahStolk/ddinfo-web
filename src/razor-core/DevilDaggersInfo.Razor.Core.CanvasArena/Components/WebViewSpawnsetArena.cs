using DevilDaggersInfo.Core.Spawnset;
using DevilDaggersInfo.Types.Core.Spawnsets;
using Microsoft.JSInterop;

namespace DevilDaggersInfo.Razor.Core.CanvasArena.Components;

public partial class WebViewSpawnsetArena
{
	private CancellationTokenSource _renderCts = new();

	private WebViewCanvasArena? _context;

	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		await base.OnAfterRenderAsync(firstRender);

		_context = new WebViewCanvasArena(_canvasId, new(JSRuntime));

		await RenderAsync(_renderCts.Token);
	}

	[JSInvokable]
	public async Task OnResize(double wrapperSize)
	{
		_renderCts.Cancel();
		_renderCts = new();

		Resize(wrapperSize);
		await RenderAsync(_renderCts.Token);
	}

	private async Task RenderAsync(CancellationToken cancellationToken)
	{
		if (_context == null)
			return;

		await _context.ClearRectAsync(0, 0, CanvasSize, CanvasSize);

		if (cancellationToken.IsCancellationRequested)
			return;

		for (int i = 0; i < SpawnsetBinary.ArenaDimension; i++)
		{
			for (int j = 0; j < SpawnsetBinary.ArenaDimension; j++)
			{
				if (cancellationToken.IsCancellationRequested)
					return;

				float tileHeight = SpawnsetBinary.ArenaTiles[i, j];
				if (tileHeight < -1)
					continue;

				float actualTileHeight = SpawnsetBinary.GetActualTileHeight(i, j, CurrentTime);
				Color color = Color.GetColorFromHeight(actualTileHeight);
				if (color.R == 0 && color.G == 0 && color.B == 0)
					continue;

				await _context.DrawTileAsync(i, j, color.R, color.G, color.B, TileSize);
			}
		}

		const int tileUnit = 4; // Tiles are 4 units in width/length in the game.
		float shrinkEndTime = SpawnsetBinary.GetShrinkEndTime();
		float shrinkRadius = shrinkEndTime == 0 ? SpawnsetBinary.ShrinkStart : Math.Max(SpawnsetBinary.ShrinkStart - CurrentTime / shrinkEndTime * (SpawnsetBinary.ShrinkStart - SpawnsetBinary.ShrinkEnd), SpawnsetBinary.ShrinkEnd);
		if (shrinkRadius is > 0 and <= 100)
		{
			await _context.SetStrokeStyleAsync("#f08");
			await _context.SetLineWidthAsync(1);
			await _context.BeginPathAsync();
			await _context.CircleAsync(CanvasSize / 2f, CanvasSize / 2f, shrinkRadius / tileUnit * TileSize);
			await _context.StrokeAsync();
		}

		if (cancellationToken.IsCancellationRequested)
			return;

		if (SpawnsetBinary.GameMode == GameMode.Race)
			await RenderRaceDaggerAsync();

		await RenderPlayerAsync();

		async Task RenderRaceDaggerAsync()
		{
			(_, float? y, _) = SpawnsetBinary.GetRaceDaggerTilePosition();
			if (!y.HasValue)
				return;

			float daggerCenterX = CanvasSize / 2f + SpawnsetBinary.RaceDaggerPosition.X / tileUnit * TileSize;
			float daggerCenterY = CanvasSize / 2f + SpawnsetBinary.RaceDaggerPosition.Y / tileUnit * TileSize;

			await _context.BeginPathAsync();
			await _context.MoveToAsync(daggerCenterX, daggerCenterY + 6);
			await _context.LineToAsync(daggerCenterX + 4, daggerCenterY - 6);
			await _context.LineToAsync(daggerCenterX + 1, daggerCenterY - 6);
			await _context.LineToAsync(daggerCenterX + 1, daggerCenterY - 10);
			await _context.LineToAsync(daggerCenterX - 1, daggerCenterY - 10);
			await _context.LineToAsync(daggerCenterX - 1, daggerCenterY - 6);
			await _context.LineToAsync(daggerCenterX - 4, daggerCenterY - 6);
			await _context.ClosePathAsync();

			await _context.SetFillStyleAsync("#444");
			await _context.FillAsync();

			await _context.SetStrokeStyleAsync("#fff");
			await _context.StrokeAsync();
		}

		async Task RenderPlayerAsync()
		{
			float playerCenterX = CanvasSize / 2f;
			float playerCenterY = CanvasSize / 2f;

			await _context.BeginPathAsync();
			await _context.MoveToAsync(playerCenterX, playerCenterY + 3);
			await _context.LineToAsync(playerCenterX + 3, playerCenterY);
			await _context.LineToAsync(playerCenterX, playerCenterY - 3);
			await _context.LineToAsync(playerCenterX - 3, playerCenterY);
			await _context.ClosePathAsync();

			await _context.SetFillStyleAsync("#f00");
			await _context.FillAsync();

			await _context.SetStrokeStyleAsync("#fff");
			await _context.StrokeAsync();
		}
	}
}
