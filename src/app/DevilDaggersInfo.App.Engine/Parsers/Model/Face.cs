namespace Warp.NET.Parsers.Model;

public readonly record struct Face(ushort Position, ushort Texture, ushort Normal)
{
	public override string ToString()
		=> $"{Position}/{Texture}/{Normal}";
}
