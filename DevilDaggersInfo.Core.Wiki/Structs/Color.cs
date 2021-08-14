namespace DevilDaggersInfo.Core.Wiki.Structs
{
	public struct Color
	{
		public Color(byte r, byte g, byte b)
		{
			R = r;
			G = g;
			B = b;
		}

		public byte R { get; }
		public byte G { get; }
		public byte B { get; }
	}
}
