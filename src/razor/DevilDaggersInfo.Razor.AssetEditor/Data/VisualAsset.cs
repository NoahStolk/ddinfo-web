namespace DevilDaggersInfo.Razor.AssetEditor.Data;

public record VisualAsset(int Size, bool IsProhibited)
{
	public bool IsSelected { get; set; }
}
