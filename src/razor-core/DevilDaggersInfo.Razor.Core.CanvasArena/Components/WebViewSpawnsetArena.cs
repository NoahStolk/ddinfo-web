using DevilDaggersInfo.Core.Spawnset;
using DevilDaggersInfo.Core.Spawnset.Enums;
using DevilDaggersInfo.Razor.Core.Canvas.JS;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace DevilDaggersInfo.Razor.Core.CanvasArena.Components;

public partial class WebViewSpawnsetArena
{
	// Currently only allow one arena.
	private const string _canvasId = "arena-canvas";

	private int _canvasSize;
	private float _tileSize;

	private WebViewCanvasArena? _context;
	private object? _canvasReference;

	private double _canvasMouseX;
	private double _canvasMouseY;

	private SpawnsetArenaHoverInfo _spawnsetArenaHoverInfo = null!;

	[Inject]
	public IJSRuntime JSRuntime { get; set; } = null!;

	[Parameter]
	[EditorRequired]
	public SpawnsetBinary SpawnsetBinary { get; set; } = null!;

	[Parameter]
	[EditorRequired]
	public float CurrentTime { get; set; }

	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		if (firstRender)
		{
			await JSRuntime.InvokeAsync<object>("initArena");
			await JSRuntime.InvokeAsync<object>("registerArena", DotNetObjectReference.Create(this));
			await JSRuntime.InvokeAsync<object>("arenaInitialResize");
		}

		_context = new WebViewCanvasArena(_canvasId, new(JSRuntime));

		await RenderAsync();
	}

	[JSInvokable]
	public async Task OnResize(double wrapperSize)
	{
		_canvasSize = (int)wrapperSize;
		_tileSize = _canvasSize / 51f;

		await RenderAsync();
	}

	[JSInvokable]
	public async ValueTask OnMouseMove(int mouseX, int mouseY)
	{
		BoundingClientRect canvasBoundingClientRect = await JSRuntime.InvokeAsync<BoundingClientRect>("getBoundingClientRect", _canvasReference);

		_canvasMouseX = mouseX - canvasBoundingClientRect.Left;
		_canvasMouseY = mouseY - canvasBoundingClientRect.Top;

		int x = Math.Clamp((int)(_canvasMouseX / _tileSize), 0, 50);
		int y = Math.Clamp((int)(_canvasMouseY / _tileSize), 0, 50);
		float height = SpawnsetBinary.ArenaTiles[x, y];
		float actualHeight = SpawnsetBinary.GetActualTileHeight(x, y, CurrentTime);

		_spawnsetArenaHoverInfo.Update(x, y, height, actualHeight);
	}

	private async Task RenderAsync()
	{
		if (_context == null)
			return;

		await _context.ClearRectAsync(0, 0, _canvasSize, _canvasSize);
		for (int i = 0; i < SpawnsetBinary.ArenaDimension; i++)
		{
			for (int j = 0; j < SpawnsetBinary.ArenaDimension; j++)
			{
				float tileHeight = SpawnsetBinary.ArenaTiles[i, j];
				if (tileHeight < -1)
					continue;

				float actualTileHeight = SpawnsetBinary.GetActualTileHeight(i, j, CurrentTime);
				Color color = Color.GetColorFromHeight(actualTileHeight);
				if (color.R == 0 && color.G == 0 && color.B == 0)
					continue;

				await _context.DrawTileAsync(i, j, color.R, color.G, color.B, _tileSize);
			}
		}

		const int tileUnit = 4; // Tiles are 4 units in width/length in the game.
		float shrinkEndTime = SpawnsetBinary.GetShrinkEndTime();
		float shrinkRadius = shrinkEndTime == 0 ? SpawnsetBinary.ShrinkStart : Math.Max(SpawnsetBinary.ShrinkStart - CurrentTime / shrinkEndTime * (SpawnsetBinary.ShrinkStart - SpawnsetBinary.ShrinkEnd), SpawnsetBinary.ShrinkEnd);
		if (shrinkRadius > 0 && shrinkRadius <= 100)
		{
			await _context.SetStrokeStyleAsync("#f08");
			await _context.SetLineWidthAsync(1);
			await _context.BeginPathAsync();
			await _context.CircleAsync(_canvasSize / 2f, _canvasSize / 2f, shrinkRadius / tileUnit * _tileSize);
			await _context.StrokeAsync();
		}

		if (SpawnsetBinary.GameMode == GameMode.Race)
			await RenderRaceDaggerAsync();

		await RenderPlayerAsync();

		async Task RenderRaceDaggerAsync()
		{
			(_, float? y, _) = SpawnsetBinary.GetRaceDaggerTilePosition();
			if (!y.HasValue)
				return;

			float daggerCenterX = _canvasSize / 2 + SpawnsetBinary.RaceDaggerPosition.X / tileUnit * _tileSize;
			float daggerCenterY = _canvasSize / 2 + SpawnsetBinary.RaceDaggerPosition.Y / tileUnit * _tileSize;

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
			float playerCenterX = _canvasSize / 2f;
			float playerCenterY = _canvasSize / 2f;

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
