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

namespace DevilDaggersInfo.Web.BlazorWasm.Client.Components.Spawnsets;

public partial class SpawnsetArena
{
	private const int _canvasSize = _tileSize * 51;
	private const int _tileSize = 8;

	private Canvas2d? _context;
	private object? _canvasReference;

	private double _canvasMouseX;
	private double _canvasMouseY;

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

		_context.ClearRect(0, 0, _canvasSize, _canvasSize + 16);
		for (int i = 0; i < SpawnsetBinary.ArenaDimension; i++)
		{
			for (int j = 0; j < SpawnsetBinary.ArenaDimension; j++)
			{
				const int tileEdgeSize = 1;

				float tileHeight = SpawnsetBinary.ArenaTiles[i, j];
				Color color = GetColorFromHeight(tileHeight);

				_context.FillStyle = (color with { A = 192 }).GetHexCode();
				_context.FillRect(i * _tileSize, j * _tileSize, _tileSize, _tileSize);

				_context.FillStyle = color.GetHexCode();
				_context.FillRect(i * _tileSize + tileEdgeSize, j * _tileSize + tileEdgeSize, _tileSize - tileEdgeSize * 2, _tileSize - tileEdgeSize * 2);
			}
		}

		for (int i = 0; i < _canvasSize / 2; i++)
		{
			_context.FillStyle = GetColorFromHeight((i - 1) * 0.5f).GetHexCode();
			_context.FillRect(i * 2, _canvasSize, 2, 16);
		}
	}

	private static Color GetColorFromHeight(float tileHeight)
	{
		float saturation = Math.Clamp((tileHeight + 1.5f) * 0.25f, 0, 1);
		float value = Math.Clamp((tileHeight + 1.5f) * 0.5f, 0, 1);
		Color color = FromHsv(tileHeight, saturation, value);
		return color;
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
