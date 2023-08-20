using DevilDaggersInfo.App.Ui.Practice.RunAnalysis.Data;
using ImGuiNET;
using System.Diagnostics;
using System.Numerics;

namespace DevilDaggersInfo.App.Ui.Practice.RunAnalysis;

public static class GemCollectionChild
{
	private static readonly List<int> _gemsCollected = new();
	private static readonly List<int> _gemsDespawned = new();
	private static readonly List<int> _gemsEaten = new();
	private static readonly List<int> _gemsTotal = new();

	// Allow up to an hour of data (roughly 3600 seconds in game).
	private static readonly Vector2[] _gemsCollectedPoints = new Vector2[60 * 60];
	private static readonly Vector2[] _gemsDespawnedPoints = new Vector2[60 * 60];
	private static readonly Vector2[] _gemsEatenPoints = new Vector2[60 * 60];
	private static readonly Vector2[] _gemsTotalPoints = new Vector2[60 * 60];

	private static int _maxGemsCollected;
	private static int _maxGemsDespawned;
	private static int _maxGemsEaten;
	private static int _maxGemsTotal;

	private static bool _showGemsCollected = true;
	private static bool _showGemsDespawned = true;
	private static bool _showGemsEaten = true;
	private static bool _showGemsTotal = true;

	private static readonly char[] _tooltipBuffer = new char[256];

	public static void Update(IReadOnlyList<StatisticEntry> data)
	{
		_gemsCollected.Clear();
		_gemsDespawned.Clear();
		_gemsEaten.Clear();
		_gemsTotal.Clear();

		for (int i = 0; i < data.Count; i++)
		{
			_gemsCollected.Add(data[i].GemsCollected);
			_gemsDespawned.Add(data[i].GemsDespawned);
			_gemsEaten.Add(data[i].GemsEaten);
			_gemsTotal.Add(data[i].GemsTotal);
		}

		_maxGemsCollected = _gemsCollected.Count > 0 ? _gemsCollected.Max() : 0;
		_maxGemsDespawned = _gemsDespawned.Count > 0 ? _gemsDespawned.Max() : 0;
		_maxGemsEaten = _gemsEaten.Count > 0 ? _gemsEaten.Max() : 0;
		_maxGemsTotal = _gemsTotal.Count > 0 ? _gemsTotal.Max() : 0;
	}

	public static unsafe void Render()
	{
		Debug.Assert(_gemsCollected.Count == _gemsDespawned.Count && _gemsCollected.Count == _gemsEaten.Count && _gemsCollected.Count == _gemsTotal.Count, "All lists should have the same length.");

		if (_gemsCollected.Count == 0)
			return;

		const float height = 256;
		if (ImGui.BeginChild("Gem Collection"))
		{
			ImGui.Checkbox("Gems Collected", ref _showGemsCollected);
			ImGui.Checkbox("Gems Despawned", ref _showGemsDespawned);
			ImGui.Checkbox("Gems Eaten", ref _showGemsEaten);
			ImGui.Checkbox("Gems Total", ref _showGemsTotal);

			ImDrawListPtr drawListPtr = ImGui.GetWindowDrawList();
			Vector2 pos = ImGui.GetCursorScreenPos();
			Vector2 size = new(ImGui.GetWindowWidth(), height);
			drawListPtr.AddRectFilled(pos, pos + size, 0xff000000);

			int max = Math.Max(
				_showGemsCollected ? _maxGemsCollected : 0,
				Math.Max(
					_showGemsDespawned ? _maxGemsDespawned : 0,
					Math.Max(
						_showGemsEaten ? _maxGemsEaten : 0,
						_showGemsTotal ? _maxGemsTotal : 0)));

			if (_showGemsCollected)
				RenderGraph(_gemsCollected, max, _gemsCollectedPoints, 0xff0000ff, pos, size, drawListPtr);

			if (_showGemsDespawned)
				RenderGraph(_gemsDespawned, max, _gemsDespawnedPoints, 0xff888888, pos, size, drawListPtr);

			if (_showGemsEaten)
				RenderGraph(_gemsEaten, max, _gemsEatenPoints, 0xff00ff00, pos, size, drawListPtr);

			if (_showGemsTotal)
				RenderGraph(_gemsTotal, max, _gemsTotalPoints, 0xff000066, pos, size, drawListPtr);

			Vector2 mousePos = ImGui.GetMousePos();
			if (mousePos.X >= pos.X && mousePos.X <= pos.X + size.X && mousePos.Y >= pos.Y && mousePos.Y <= pos.Y + size.Y)
			{
				int index = Math.Clamp((int)((mousePos.X - pos.X) / size.X * _gemsCollected.Count), 0, _gemsCollected.Count - 1);
				int gemsCollected = _gemsCollected[index];
				int gemsDespawned = _gemsDespawned[index];
				int gemsEaten = _gemsEaten[index];
				int gemsTotal = _gemsTotal[index];

				UnsafeCharBufferWriter writer = new(_tooltipBuffer);
				writer.Write("Gems Collected: ");
				writer.WriteLine(gemsCollected);
				writer.Write("Gems Despawned: ");
				writer.WriteLine(gemsDespawned);
				writer.Write("Gems Eaten: ");
				writer.WriteLine(gemsEaten);
				writer.Write("Gems Total: ");
				writer.WriteLine(gemsTotal);
				ImGui.SetTooltip(writer);
			}

			ImGui.EndChild();
		}

		void RenderGraph(IReadOnlyList<int> data, int maxDataEntry, Vector2[] pointsArray, uint color, Vector2 cursorScreenPos, Vector2 graphSize, ImDrawListPtr drawListPtr)
		{
			pointsArray.AsSpan().Clear();
			for (int i = 0; i < data.Count; i++)
			{
				float normalizedX = i / (float)(data.Count - 1);
				float normalizedY = data[i] / (float)maxDataEntry;
				pointsArray[i] = new(cursorScreenPos.X + normalizedX * graphSize.X, cursorScreenPos.Y + height - normalizedY * graphSize.Y);
			}

			fixed (Vector2* p = pointsArray)
				drawListPtr.AddPolyline(ref p[0], data.Count, color, ImDrawFlags.RoundCornersDefault, 1);
		}
	}
}
