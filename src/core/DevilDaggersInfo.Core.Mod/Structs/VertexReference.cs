namespace DevilDaggersInfo.Core.Mod.Structs;

public readonly struct VertexReference
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
	{
		if (PositionReference == TexCoordReference && PositionReference == NormalReference)
			return PositionReference.ToString();

		return $"{PositionReference}/{TexCoordReference}/{NormalReference}";
	}
}
