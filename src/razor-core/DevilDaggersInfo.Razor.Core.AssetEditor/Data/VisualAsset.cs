namespace DevilDaggersInfo.Razor.Core.AssetEditor.Data;

public record VisualAsset
{
	public VisualAsset(int size, bool isProhibited)
	{
		Size = size;
		IsProhibited = isProhibited;
		IsSelected = false;
	}

	public int Size { get; }

	public bool IsProhibited { get; }

	public bool IsSelected { get; set; }
}
