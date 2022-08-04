using DevilDaggersInfo.Core.Spawnset;
using DevilDaggersInfo.Razor.Core.Canvas.JS;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace DevilDaggersInfo.Razor.Core.CanvasArena.Components;

public abstract class SpawnsetArena : ComponentBase
{
	// Currently only allow one arena.
	protected const string _canvasId = "arena-canvas";

	protected int CanvasSize { get; private set; }
	protected float TileSize { get; private set; }

	protected object? CanvasReference { get; set; }

	protected SpawnsetArenaHoverInfo SpawnsetArenaHoverInfo { get; set; } = null!;

	[Inject]
	public IJSRuntime JSRuntime { get; set; } = null!;

	[Parameter]
	[EditorRequired]
	public SpawnsetBinary SpawnsetBinary { get; set; } = null!;

	[Parameter]
	[EditorRequired]
	public float CurrentTime { get; set; }

	protected void Resize(double wrapperSize)
	{
		CanvasSize = (int)wrapperSize;
		TileSize = CanvasSize / (float)SpawnsetBinary.ArenaDimension;
	}

	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		if (firstRender)
		{
			await JSRuntime.InvokeAsync<object>("initArena");
			await JSRuntime.InvokeAsync<object>("registerArena", DotNetObjectReference.Create(this));
			await JSRuntime.InvokeAsync<object>("arenaInitialResize");
		}
	}

	[JSInvokable]
	public async ValueTask OnMouseMove(int mouseX, int mouseY)
	{
		BoundingClientRect canvasBoundingClientRect = await JSRuntime.InvokeAsync<BoundingClientRect>("getBoundingClientRect", CanvasReference);

		double canvasMouseX = mouseX - canvasBoundingClientRect.Left;
		double canvasMouseY = mouseY - canvasBoundingClientRect.Top;

		int x = Math.Clamp((int)(canvasMouseX / TileSize), 0, SpawnsetBinary.ArenaDimension - 1);
		int y = Math.Clamp((int)(canvasMouseY / TileSize), 0, SpawnsetBinary.ArenaDimension - 1);
		float height = SpawnsetBinary.ArenaTiles[x, y];
		float actualHeight = SpawnsetBinary.GetActualTileHeight(x, y, CurrentTime);

		SpawnsetArenaHoverInfo.Update(x, y, height, actualHeight);
	}
}
