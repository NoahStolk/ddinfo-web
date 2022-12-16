using DevilDaggersInfo.Core.Spawnset;
using DevilDaggersInfo.Razor.Core.Canvas.JS;
using DevilDaggersInfo.Types.Core.Spawnsets;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace DevilDaggersInfo.Razor.Core.CanvasArena.Components;

public partial class SpawnsetArena
{
	// Currently only allow one arena.
	private const string CanvasId = "arena-canvas";

	private WebAssemblyCanvasArena? _context;

	private int _canvasSize;
	private float _tileSize;

	private object? _canvasReference;

	// ! Blazor ref
	private SpawnsetArenaHoverInfo _spawnsetArenaHoverInfo = null!;

	[Inject]
	public required IJSRuntime JSRuntime { get; set; }

	[Parameter]
	[EditorRequired]
	public required SpawnsetBinary SpawnsetBinary { get; set; }

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

		_context = new(CanvasId);

		Render();
	}

	[JSInvokable]
	public void OnResize(double wrapperSize)
	{
		_canvasSize = (int)wrapperSize;
		_tileSize = _canvasSize / (float)SpawnsetBinary.ArenaDimension;
		Render();
	}

	[JSInvokable]
	public async ValueTask OnMouseMove(int mouseX, int mouseY)
	{
		BoundingClientRect canvasBoundingClientRect = await JSRuntime.InvokeAsync<BoundingClientRect>("getBoundingClientRect", _canvasReference);

		double canvasMouseX = mouseX - canvasBoundingClientRect.Left;
		double canvasMouseY = mouseY - canvasBoundingClientRect.Top;

		int x = Math.Clamp((int)(canvasMouseX / _tileSize), 0, SpawnsetBinary.ArenaDimension - 1);
		int y = Math.Clamp((int)(canvasMouseY / _tileSize), 0, SpawnsetBinary.ArenaDimension - 1);
		float height = SpawnsetBinary.ArenaTiles[x, y];
		float actualHeight = SpawnsetBinary.GetActualTileHeight(x, y, CurrentTime);

		_spawnsetArenaHoverInfo.Update(x, y, height, actualHeight);
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
		_context.DrawArena(colors, _canvasSize, shrinkRadius, SpawnsetBinary.GameMode == GameMode.Race && y.HasValue, SpawnsetBinary.RaceDaggerPosition.X, SpawnsetBinary.RaceDaggerPosition.Y);
	}
}
