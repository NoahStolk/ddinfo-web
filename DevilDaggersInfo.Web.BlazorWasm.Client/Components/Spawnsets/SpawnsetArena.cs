using DevilDaggersInfo.Core.Spawnset;
using DevilDaggersInfo.Core.Spawnset.Enums;
using DevilDaggersInfo.Web.BlazorWasm.Client.Core.CanvasChart.Data;
using DevilDaggersInfo.Web.BlazorWasm.Client.Core.CanvasChart.Options.LineChart;
using DevilDaggersInfo.Web.BlazorWasm.Client.Core.CanvasChart.Utils;
using DevilDaggersInfo.Web.BlazorWasm.Client.Core.Unmarshalled;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop;
using System.Data;
using DevilDaggersInfo.Web.BlazorWasm.Client.Core.CanvasChart.JsRuntime;
using System.Numerics;

namespace DevilDaggersInfo.Web.BlazorWasm.Client.Components.Spawnsets;

public partial class SpawnsetArena
{
	private const int _canvasSize = _tileSize * 51;
	private const int _tileSize = 8;

	private Canvas2d? _context;
	private object? _canvasReference;

	private double _canvasMouseX;
	private double _canvasMouseY;

	private float _sliderMax;

	private float _currentTime;

	private SpawnsetArenaHoverInfo _spawnsetArenaHoverInfo = null!;

	private float CurrentTime
	{
		get => _currentTime;
		set
		{
			_currentTime = value;
			Render();
		}
	}

	[Inject]
	public IJSRuntime JsRuntime { get; set; } = null!;

	[Parameter, EditorRequired]
	public SpawnsetBinary SpawnsetBinary { get; set; } = null!;

	protected override void OnInitialized()
	{
		// Determine the max tile height to add additional time to the slider.
		// For example, when the shrink ends at 200, but there is a tile at height 20, we want to add another 88 seconds ((20 + 2) * 4) to the slider so the shrink transition is always fully visible for all tiles.
		// Add 2 heights to make sure it is still visible until the height is -2 (the palette should still show something until a height of at least -1 or lower).
		// Multiply by 4 because a tile falls by 1 unit every 4 seconds.
		float maxTileHeight = 0;
		for (int i = 0; i < SpawnsetBinary.ArenaDimension; i++)
		{
			for (int j = 0; j < SpawnsetBinary.ArenaDimension; j++)
			{
				float tileHeight = SpawnsetBinary.ArenaTiles[i, j];
				if (maxTileHeight < tileHeight)
					maxTileHeight = tileHeight;
			}
		}

		_sliderMax = SpawnsetBinary.GetShrinkEndTime() + (maxTileHeight + 2) * 4;
	}

	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		if (firstRender)
		{
			await JsRuntime.InvokeAsync<object>("init");
			await JsRuntime.InvokeAsync<object>("registerArena", DotNetObjectReference.Create(this), "arena");
		}

		_context = new Canvas2d("arena-canvas");

		Render();
	}

	private void Render()
	{
		if (_context == null)
			return;

		_context.ClearRect(0, 0, _canvasSize, _canvasSize);
		for (int i = 0; i < SpawnsetBinary.ArenaDimension; i++)
		{
			for (int j = 0; j < SpawnsetBinary.ArenaDimension; j++)
			{
				float tileHeight = SpawnsetBinary.ArenaTiles[i, j];
				if (tileHeight < -1)
					continue;

				float actualTileHeight = SpawnsetBinary.GetActualTileHeight(i, j, _currentTime);
				Color color = GetColorFromHeight(actualTileHeight);
				if (color.R == 0 && color.G == 0 && color.B == 0)
					continue;

				_context.DrawTile(i, j, color.R, color.G, color.B);
			}
		}

		const int tileUnit = 4; // Tiles are 4 units in width/length in the game.
		float shrinkEndTime = SpawnsetBinary.GetShrinkEndTime();
		float shrinkRadius = shrinkEndTime == 0 ? SpawnsetBinary.ShrinkStart : Math.Max((SpawnsetBinary.ShrinkStart - _currentTime / shrinkEndTime * (SpawnsetBinary.ShrinkStart - SpawnsetBinary.ShrinkEnd)), SpawnsetBinary.ShrinkEnd);
		if (shrinkRadius > 0 && shrinkRadius <= 100)
		{
			_context.StrokeStyle = "#f08";
			_context.LineWidth = 1;
			_context.BeginPath();
			_context.Circle(_canvasSize / 2, _canvasSize / 2, shrinkRadius / tileUnit * _tileSize);
			_context.Stroke();
		}

		if (SpawnsetBinary.GameMode == GameMode.Race)
			RenderRaceDagger();

		RenderPlayer();

		void RenderRaceDagger()
		{
			(int x, float? y, int z) = SpawnsetBinary.GetRaceDaggerTilePosition();
			if (!y.HasValue)
				return;

			float daggerCenterX = _canvasSize / 2 + SpawnsetBinary.RaceDaggerPosition.X / tileUnit * _tileSize;
			float daggerCenterY = _canvasSize / 2 + SpawnsetBinary.RaceDaggerPosition.Y / tileUnit * _tileSize;

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
			(int x, float? y, int z) = SpawnsetBinary.GetRaceDaggerTilePosition();
			if (!y.HasValue)
				return;

			float playerCenterX = _canvasSize / 2;
			float playerCenterY = _canvasSize / 2;

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

	[JSInvokable]
	public async ValueTask OnMouseMove(int mouseX, int mouseY)
	{
		BoundingClientRect canvasBoundingClientRect = await JsRuntime.InvokeAsync<BoundingClientRect>("getBoundingClientRect", _canvasReference);

		_canvasMouseX = mouseX - canvasBoundingClientRect.Left;
		_canvasMouseY = mouseY - canvasBoundingClientRect.Top;

		int x = Math.Clamp((int)_canvasMouseX / _tileSize, 0, 50);
		int y = Math.Clamp((int)_canvasMouseY / _tileSize, 0, 50);
		float height = SpawnsetBinary.ArenaTiles[x, y];
		float actualHeight = SpawnsetBinary.GetActualTileHeight(x, y, _currentTime);

		_spawnsetArenaHoverInfo.Update(x, y, height, actualHeight);
	}

	private static Color GetColorFromHeight(float tileHeight)
	{
		float h = tileHeight * 3 + 12;
		float s = (tileHeight + 1.5f) * 0.25f;
		float v = (tileHeight + 2) * 0.2f;
		return FromHsv(h, s, v);
	}

	private static Color FromHsv(float hue, float saturation, float value)
	{
		saturation = Math.Clamp(saturation, 0, 1);
		value = Math.Clamp(value, 0, 1);

		int hi = (int)(MathF.Floor(hue / 60)) % 6;
		float f = hue / 60 - MathF.Floor(hue / 60);

		value = value * 255;
		byte v = (byte)value;
		byte p = (byte)(value * (1 - saturation));
		byte q = (byte)(value * (1 - f * saturation));
		byte t = (byte)(value * (1 - (1 - f) * saturation));

		return hi switch
		{
			0 => new(v, t, p),
			1 => new(q, v, p),
			2 => new(p, v, t),
			3 => new(p, q, v),
			4 => new(t, p, v),
			_ => new(v, p, q),
		};
	}

	private readonly record struct Color(byte R, byte G, byte B, byte A = 255);
}
