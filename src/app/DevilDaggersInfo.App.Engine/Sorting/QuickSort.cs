namespace Warp.NET.Sorting;

public static class QuickSort
{
	public static void Sort<T>(T[] input)
		where T : INumber<T>
		=> Sort(input, 0, input.Length - 1);

	private static void Sort<T>(T[] input, int left, int right)
		where T : INumber<T>
	{
		while (true)
		{
			if (left >= right)
				return;

			int q = Partition(input, left, right);
			Sort(input, left, q - 1);
			left = q + 1;
		}
	}

	private static int Partition<T>(T[] input, int left, int right)
		where T : INumber<T>
	{
		T pivot = input[right];

		int i = left;
		for (int j = left; j < right; j++)
		{
			if (input[j] > pivot)
				continue;

			(input[j], input[i]) = (input[i], input[j]);
			i++;
		}

		input[right] = input[i];
		input[i] = pivot;

		return i;
	}
}
