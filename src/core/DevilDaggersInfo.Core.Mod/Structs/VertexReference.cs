namespace DevilDaggersInfo.Core.Mod.Structs;

public struct VertexReference
{
	public VertexReference(int positionReference, int texCoordReference, int normalReference)
	{
		PositionReference = positionReference;
		TexCoordReference = texCoordReference;
		NormalReference = normalReference;
	}

	public VertexReference(int unifiedReference)
		: this(unifiedReference, unifiedReference, unifiedReference)
	{
	}

	public int PositionReference { get; }
	public int TexCoordReference { get; }
	public int NormalReference { get; }

	public override string ToString()
		=> $"{PositionReference}/{TexCoordReference}/{NormalReference}";
}
