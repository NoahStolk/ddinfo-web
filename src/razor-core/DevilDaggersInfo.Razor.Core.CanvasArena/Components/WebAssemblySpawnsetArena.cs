using DevilDaggersInfo.Core.Spawnset;
using DevilDaggersInfo.Core.Spawnset.Enums;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace DevilDaggersInfo.Razor.Core.CanvasArena.Components;

public partial class WebAssemblySpawnsetArena
{
	private WebAssemblyCanvasArena? _context;

	[Inject]
	public IJSUnmarshalledRuntime JSUnmarshalledRuntime { get; set; } = null!;

	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		await base.OnAfterRenderAsync(firstRender);

		_context = new WebAssemblyCanvasArena(_canvasId, new(JSUnmarshalledRuntime));

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

		_context.ClearRect(0, 0, CanvasSize, CanvasSize);
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

				_context.DrawTile(i, j, color.R, color.G, color.B, TileSize);
			}
		}

		const int tileUnit = 4; // Tiles are 4 units in width/length in the game.
		float shrinkEndTime = SpawnsetBinary.GetShrinkEndTime();
		float shrinkRadius = shrinkEndTime == 0 ? SpawnsetBinary.ShrinkStart : Math.Max(SpawnsetBinary.ShrinkStart - CurrentTime / shrinkEndTime * (SpawnsetBinary.ShrinkStart - SpawnsetBinary.ShrinkEnd), SpawnsetBinary.ShrinkEnd);
		if (shrinkRadius > 0 && shrinkRadius <= 100)
		{
			_context.StrokeStyle = "#f08";
			_context.LineWidth = 1;
			_context.BeginPath();
			_context.Circle(CanvasSize / 2f, CanvasSize / 2f, shrinkRadius / tileUnit * TileSize);
			_context.Stroke();
		}

		if (SpawnsetBinary.GameMode == GameMode.Race)
			RenderRaceDagger();

		RenderPlayer();

		void RenderRaceDagger()
		{
			(_, float? y, _) = SpawnsetBinary.GetRaceDaggerTilePosition();
			if (!y.HasValue)
				return;

			float daggerCenterX = CanvasSize / 2 + SpawnsetBinary.RaceDaggerPosition.X / tileUnit * TileSize;
			float daggerCenterY = CanvasSize / 2 + SpawnsetBinary.RaceDaggerPosition.Y / tileUnit * TileSize;

			_context.BeginPath();
			_context.MoveTo(daggerCenterX, daggerCenterY + 6);
			_context.LineTo(daggerCenterX + 4, daggerCenterY - 6);
			_context.LineTo(daggerCenterX + 1, daggerCenterY - 6);
			_context.LineTo(daggerCenterX + 1, daggerCenterY - 10);
			_context.LineTo(daggerCenterX - 1, daggerCenterY - 10);
			_context.LineTo(daggerCenterX - 1, daggerCenterY - 6);
			_context.LineTo(daggerCenterX - 4, daggerCenterY - 6);
			_context.ClosePath();

			_context.FillStyle = "#444";
			_context.Fill();

			_context.StrokeStyle = "#fff";
			_context.Stroke();
		}

		void RenderPlayer()
		{
			float playerCenterX = CanvasSize / 2f;
			float playerCenterY = CanvasSize / 2f;

			_context.BeginPath();
			_context.MoveTo(playerCenterX, playerCenterY + 3);
			_context.LineTo(playerCenterX + 3, playerCenterY);
			_context.LineTo(playerCenterX, playerCenterY - 3);
			_context.LineTo(playerCenterX - 3, playerCenterY);
			_context.ClosePath();

			_context.FillStyle = "#f00";
			_context.Fill();

			_context.StrokeStyle = "#fff";
			_context.Stroke();
		}
	}
}
