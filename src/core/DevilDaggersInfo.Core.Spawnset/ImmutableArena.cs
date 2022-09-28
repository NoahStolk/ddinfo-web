namespace DevilDaggersInfo.Core.Spawnset;

public class ImmutableArena
{
	private readonly int _dimension;
	private readonly float[,] _heights;

	// TODO: Make sure it's not possible to modify the contents of the reference passed to the ctor.
	public ImmutableArena(int dimension, float[,] heights)
	{
		if (dimension is < 0 or > SpawnsetBinary.ArenaDimensionMax)
			throw new ArgumentOutOfRangeException(nameof(dimension), $"Dimension cannot be negative or greater than {SpawnsetBinary.ArenaDimensionMax}.");

		if (heights.GetLength(0) != dimension || heights.GetLength(1) != dimension)
			throw new ArgumentOutOfRangeException(nameof(heights), $"Arena array must be {dimension} by {dimension}.");

		_dimension = dimension;
		_heights = heights;
	}

	public float this[int x, int y]
	{
		get
		{
			if (x < 0 || x >= _dimension)
				throw new ArgumentOutOfRangeException(nameof(x), $"Parameter {x} is out of range; must not be negative, and must not be equal to or greater than {_dimension}.");
			if (y < 0 || y >= _dimension)
				throw new ArgumentOutOfRangeException(nameof(y), $"Parameter {y} is out of range; must not be negative, and must not be equal to or greater than {_dimension}.");

			return _heights[x, y];
		}
	}
}
