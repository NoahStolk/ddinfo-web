using DevilDaggersInfo.App.Ui.Base;
using Warp.NET.Ui;

namespace DevilDaggersInfo.App.Ui.SurvivalEditor.Utils;

public static class ArenaEditingUtils
{
	public static PixelBounds GetRectangle(Vector2i<int> start, Vector2i<int> end)
	{
		bool startXMin = start.X < end.X;
		int minX, maxX;
		if (startXMin)
		{
			minX = start.X;
			maxX = end.X;
		}
		else
		{
			minX = end.X;
			maxX = start.X;
		}

		bool startYMin = start.Y < end.Y;
		int minY, maxY;
		if (startYMin)
		{
			minY = start.Y;
			maxY = end.Y;
		}
		else
		{
			minY = end.Y;
			maxY = start.Y;
		}

		return new(minX, minY, minX - maxX, minY - maxY);
	}

	public static bool LineIntersectsSquare(Vector2 lineA, Vector2 lineB, Vector2 squarePosition, float squareSize)
	{
		float squareHalfSize = squareSize / 2f;
		Vector2 squareTl = squarePosition + new Vector2(-squareHalfSize, -squareHalfSize);
		Vector2 squareTr = squarePosition + new Vector2(+squareHalfSize, -squareHalfSize);
		Vector2 squareBl = squarePosition + new Vector2(-squareHalfSize, +squareHalfSize);
		Vector2 squareBr = squarePosition + new Vector2(+squareHalfSize, +squareHalfSize);
		Vector2 lineNormal = GetLineNormal(lineA, lineB);

		// Calculate booleans for all 4 points; return true immediately when any of the booleans do not hold the same value.
		bool tlBehind = PointIsBehindPlane(lineA, lineNormal, squareTl);
		if (PointIsBehindPlane(lineA, lineNormal, squareTr) != tlBehind)
			return true;

		if (PointIsBehindPlane(lineA, lineNormal, squareBl) != tlBehind)
			return true;

		return PointIsBehindPlane(lineA, lineNormal, squareBr) != tlBehind;
	}

	public static Vector2 GetLineNormal(Vector2 lineA, Vector2 lineB)
	{
		float deltaX = lineB.X - lineA.X;
		float deltaY = lineB.Y - lineA.Y;
		return new(-deltaY, deltaX);
	}

	public static bool PointIsBehindPlane(Vector2 linePoint, Vector2 lineNormal, Vector2 point)
	{
		return Vector2.Dot(lineNormal, Vector2.Normalize(point - linePoint)) < 0;
	}
}
