using DevilDaggersInfo.Core.Spawnset;
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

	private float _currentTime;

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

	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		if (firstRender)
			await JsRuntime.InvokeAsync<object>("registerArena", DotNetObjectReference.Create(this), "arena");

		_context = new Canvas2d("arena-canvas");

		Render();
	}

	private void Render()
	{
		if (_context == null)
			return;

		const int tileUnit = 4; // Tiles are 4 units in width/length in the game.
		float shrinkRadius = (SpawnsetBinary.ShrinkStart - _currentTime / SpawnsetBinary.GetFinalShrinkStateSecond() * (SpawnsetBinary.ShrinkStart - SpawnsetBinary.ShrinkEnd)) / tileUnit;

		_context.ClearRect(0, 0, _canvasSize, _canvasSize + 16);
		for (int i = 0; i < SpawnsetBinary.ArenaDimension; i++)
		{
			for (int j = 0; j < SpawnsetBinary.ArenaDimension; j++)
			{
				const int tileEdgeSize = 1;

				float shrinkTime = SpawnsetBinary.GetShrinkTimeForTile(i, j);
				float shrinkHeight = Math.Max(0, _currentTime - shrinkTime) / 4;

				float tileHeight = SpawnsetBinary.ArenaTiles[i, j] - shrinkHeight;
				Color color = GetColorFromHeight(tileHeight);
				if (color.R == 0 && color.G == 0 && color.B == 0)
					continue;

				_context.FillStyle = (color with { A = 192 }).GetHexCode();
				_context.FillRect(i * _tileSize, j * _tileSize, _tileSize, _tileSize);

				_context.FillStyle = color.GetHexCode();
				_context.FillRect(i * _tileSize + tileEdgeSize, j * _tileSize + tileEdgeSize, _tileSize - tileEdgeSize * 2, _tileSize - tileEdgeSize * 2);
			}
		}

		_context.StrokeStyle = "#f0f";
		_context.LineWidth = 1;
		_context.BeginPath();
		_context.Circle(_canvasSize / 2, _canvasSize / 2, shrinkRadius * _tileSize);
		_context.Stroke();

		for (int i = 0; i < _canvasSize / 2; i++)
		{
			_context.FillStyle = GetColorFromHeight((i - 4) * 0.5f).GetHexCode();
			_context.FillRect(i * 2, _canvasSize, 2, 16);
		}
	}

	private static Color GetColorFromHeight(float tileHeight)
	{
		float h = tileHeight * 3 + 12;
		float s = (tileHeight + 1.5f) * 0.25f;
		float v = (tileHeight + 2) * 0.2f;
		return FromHsv(h, s, v);
	}

	[JSInvokable]
	public async ValueTask OnMouseMove(int mouseX, int mouseY)
	{
		BoundingClientRect canvasBoundingClientRect = await JsRuntime.InvokeAsync<BoundingClientRect>("getBoundingClientRect", _canvasReference);

		_canvasMouseX = mouseX - canvasBoundingClientRect.Left;
		_canvasMouseY = mouseY - canvasBoundingClientRect.Top;
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

	private readonly record struct Color(byte R, byte G, byte B, byte A = 255)
	{
		public string GetHexCode() => $"#{R:X2}{G:X2}{B:X2}{A:X2}";
	}
}
